using System.Runtime.InteropServices;
using System.Text;
using Serilog;
using Bale.Native.Vulkan;
using static Bale.Native.Core.Common;

namespace Bale.Graphics.Vulkan;

public sealed class VulkanPhysicalDeviceSelector {
    public IntPtr PhysicalDevice { get; }

    private readonly VulkanInstance _vulkanInstance;
    private readonly VulkanSurfaceManager _surfaceManager;

    public VulkanPhysicalDeviceSelector(
        VulkanInstance vulkanInstance,
        VulkanSurfaceManager surfaceManager
    ) {
        _vulkanInstance = vulkanInstance;
        _surfaceManager = surfaceManager;

        PhysicalDevice = PickPhysicalDevice();
        if (PhysicalDevice == NULL) {
            throw new Exception("Failed to find Vulkan-compatible GPU");
        }

        Log.Information("selected physical device");
    }

    private IntPtr PickPhysicalDevice() {
        uint deviceCount = 0;
        VulkanLow.vkEnumeratePhysicalDevices(_vulkanInstance.Handle, ref deviceCount, NULL);

        if (deviceCount == 0) {
            throw new Exception("Failed to find GPUs with Vulkan support");
        }

        var devices = new IntPtr[deviceCount];
        VulkanLow.vkEnumeratePhysicalDevices(
            _vulkanInstance.Handle,
            ref deviceCount,
            Marshal.UnsafeAddrOfPinnedArrayElement(devices, 0)
        );

        return devices
            .Select(d => (device: d, score: RateDeviceSuitability(d)))
            .Where(d => d.score > 0)
            .OrderByDescending(d => d.score)
            .FirstOrDefault().device;
    }

    private int RateDeviceSuitability(IntPtr device) {
        VulkanLow.vkGetPhysicalDeviceProperties(device, out var properties);

        var name = GetDeviceName(ref properties);
        Log.Information("checking {gpu}", name);

        if (!CheckQueueFamilies(device)) {
            return 0;
        }

        var score = properties.deviceType == VkPhysicalDeviceType.VK_PHYSICAL_DEVICE_TYPE_DISCRETE_GPU ? 1000 : 0;
        score += (int)properties.limits.maxImageDimension2D;

        Log.Information("{gpu} has suitability score of {score}", name, score);

        return score;
    }

    private bool CheckQueueFamilies(IntPtr device) {
        uint queueFamilyCount = 0;

        VulkanLow.vkGetPhysicalDeviceQueueFamilyProperties(device, ref queueFamilyCount, NULL);
        if (queueFamilyCount == 0) return false;

        var queueFamilies = new VkQueueFamilyProperties[queueFamilyCount];
        VulkanLow.vkGetPhysicalDeviceQueueFamilyProperties(
            device,
            ref queueFamilyCount,
            Marshal.UnsafeAddrOfPinnedArrayElement(queueFamilies, 0)
        );

        bool hasGraphicsQueue = false, hasPresentationQueue = false;

        for (uint i = 0; i < queueFamilyCount; i++) {
            if ((queueFamilies[i].queueFlags & VkQueueFlags.VK_QUEUE_GRAPHICS_BIT) != 0) {
                hasGraphicsQueue = true;
            }

            VulkanLow.vkGetPhysicalDeviceSurfaceSupportKHR(device, i, _surfaceManager.Surface, out var presentSupport);
            if (presentSupport == TRUE) {
                hasPresentationQueue = true;
            }

            if (hasGraphicsQueue && hasPresentationQueue) {
                return true;
            }
        }

        return false;
    }

    private static string GetDeviceName(ref VkPhysicalDeviceProperties props) {
        unsafe {
            fixed (byte* namePtr = props.deviceName) {
                return Encoding.ASCII.GetString(namePtr, 256).Split('\0')[0];
            }
        }
    }
}