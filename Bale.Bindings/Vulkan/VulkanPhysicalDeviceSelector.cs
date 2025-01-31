using System.Runtime.InteropServices;
using System.Text;
using Bale.Bindings.Native;
using Bale.Bindings.Native.Vulkan;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanPhysicalDeviceSelector {
    private readonly IntPtr _instanceHandle;
    private readonly IntPtr _surface;

    public IntPtr PhysicalDevice { get; }

    public VulkanPhysicalDeviceSelector(IntPtr instanceHandle, IntPtr surface) {
        _instanceHandle = instanceHandle;
        _surface = surface;
        PhysicalDevice = PickPhysicalDevice();
        if (PhysicalDevice == NULL) {
            throw new Exception("Failed to find a suitable GPU");
        }
        Console.WriteLine("Selected Vulkan-compatible GPU.");
    }

    private IntPtr PickPhysicalDevice() {
        uint deviceCount = 0;
        VulkanLow.vkEnumeratePhysicalDevices(_instanceHandle, ref deviceCount, NULL);

        if (deviceCount == 0) {
            throw new Exception("Failed to find GPUs with Vulkan support");
        }

        var devices = new IntPtr[deviceCount];
        VulkanLow.vkEnumeratePhysicalDevices(
            _instanceHandle,
            ref deviceCount,
            Marshal.UnsafeAddrOfPinnedArrayElement(devices, 0)
        );

        // Rate them and pick the best
        return devices
            .Select(d => (device: d, score: RateDeviceSuitability(d)))
            .Where(d => d.score > 0)
            .OrderByDescending(d => d.score)
            .FirstOrDefault().device;
    }

    private int RateDeviceSuitability(IntPtr device) {
        VulkanLow.vkGetPhysicalDeviceProperties(device, out var properties);
        var name = GetDeviceName(ref properties);
        Console.WriteLine($"Checking GPU: {name}");

        if (!CheckQueueFamilies(device)) {
            return 0;
        }

        int score = (properties.deviceType == VkPhysicalDeviceType.VK_PHYSICAL_DEVICE_TYPE_DISCRETE_GPU)
            ? 1000
            : 0;
        score += (int)properties.limits.maxImageDimension2D;

        Console.WriteLine($"GPU '{name}' suitability score: {score}");
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

            VulkanLow.vkGetPhysicalDeviceSurfaceSupportKHR(device, i, _surface, out var presentSupport);
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
