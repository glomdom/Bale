using System.Runtime.InteropServices;
using System.Text;
using Bale.Bindings.Native;
using Bale.Bindings.Native.Vulkan;
using static Bale.Bindings.Native.GLFW;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanApp : IDisposable {
    private IntPtr _window;
    private readonly VulkanInstance _vulkanInstance;
    private IntPtr _surface;
    private IntPtr _physicalDevice;

    public VulkanApp(string appName, int width, int height) {
        InitGLFW();
        CreateWindow(width, height, appName);

        _vulkanInstance = new VulkanInstance(appName, new Version(1, 0, 0));
        CreateSurface();
        PickPhysicalDevice();
    }

    public void Run() {
        while (!glfwWindowShouldClose(_window)) {
            glfwPollEvents();
        }
    }

    public void Dispose() {
        if (_surface != NULL) {
            VulkanLow.vkDestroySurfaceKHR(_vulkanInstance.Handle, _surface, NULL);
            _surface = NULL;
        }

        if (_vulkanInstance.Handle != NULL) {
            _vulkanInstance.Dispose();
        }
        
        glfwTerminate();
    }
    
    private void InitGLFW() {
        if (!glfwInit()) {
            throw new Exception("Failed to initialize GLFW");
        }

        glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);
        glfwWindowHint(GLFW_RESIZEABLE, GLFW_FALSE);
    }

    private void CreateWindow(int width, int height, string title) {
        _window = glfwCreateWindow(width, height, title, NULL, NULL);
        if (_window == NULL) {
            throw new Exception("Failed to create GLFW window");
        }
    }

    private void CreateSurface() {
        var result = glfwCreateWindowSurface(_vulkanInstance.Handle, _window, NULL, out _surface);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create Vulkan surface: {result}");
        }

        Console.WriteLine($"Created Vulkan surface with result: {result}");
    }

    private void PickPhysicalDevice() {
        uint deviceCount = 0;
        VulkanLow.vkEnumeratePhysicalDevices(_vulkanInstance.Handle, ref deviceCount, NULL);

        if (deviceCount == 0) {
            throw new Exception("Failed to find GPUs with Vulkan support");
        }
        
        var devices = new IntPtr[deviceCount];
        VulkanLow.vkEnumeratePhysicalDevices(_vulkanInstance.Handle, ref deviceCount, Marshal.UnsafeAddrOfPinnedArrayElement(devices, 0));

        _physicalDevice = devices
            .Select(d => (device: d, score: RateDeviceSuitability(d)))
            .Where(d => d.score > 0)
            .OrderByDescending(d => d.score)
            .FirstOrDefault().device;

        if (_physicalDevice == NULL) {
            throw new Exception("Failed to find a suitable GPU");
        }
        
        Console.WriteLine("Selected Vulkan-compatible GPU");
    }

    private int RateDeviceSuitability(IntPtr device) {
        VulkanLow.vkGetPhysicalDeviceProperties(device, out var properties);

        var deviceName = GetDeviceName(ref properties);
        Console.WriteLine($"Checking GPU: {deviceName}");

        if (!CheckQueueFamilies(device)) {
            return 0; // not suitable
        }
        
        var score = properties.deviceType == VkPhysicalDeviceType.VK_PHYSICAL_DEVICE_TYPE_DISCRETE_GPU ? 1000 : 0;
        score += (int)properties.limits.maxImageDimension2D;
        
        Console.WriteLine($"GPU '{deviceName}' suitability score: {score}");
        
        return score;
    }

    private bool CheckQueueFamilies(IntPtr device) {
        uint queueFamilyCount = 0;
        VulkanLow.vkGetPhysicalDeviceQueueFamilyProperties(device, ref queueFamilyCount, NULL);

        if (queueFamilyCount == 0) {
            return false;
        }
        
        var queueFamilies = new VkQueueFamilyProperties[queueFamilyCount];
        VulkanLow.vkGetPhysicalDeviceQueueFamilyProperties(device, ref queueFamilyCount, Marshal.UnsafeAddrOfPinnedArrayElement(queueFamilies, 0));

        return queueFamilies.Any(q => (q.queueFlags & VkQueueFlags.VK_QUEUE_GRAPHICS_BIT) != 0);
    }

    private static string GetDeviceName(ref VkPhysicalDeviceProperties properties) {
        unsafe {
            fixed (byte* namePtr = properties.deviceName) {
                return Encoding.ASCII.GetString(namePtr, 256).Split('\0')[0];
            }
        }
    }
}