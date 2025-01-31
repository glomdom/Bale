using System.Runtime.InteropServices;
using Bale.Bindings.Native.Vulkan;

namespace Bale.Bindings.Native;

/// <summary>
/// Low-level exports of Vulkan functions.
/// </summary>
public static partial class VulkanLow {
    static VulkanLow() {
        _ = typeof(NativeResolver);
    }

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkCreateInstance(
        ref VkInstanceCreateInfo pCreateInfo,
        IntPtr pAllocator,
        out IntPtr pInstance
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkDestroyInstance(
        IntPtr instance,
        IntPtr pAllocator
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkEnumeratePhysicalDevices(
        IntPtr instance,
        ref uint pPhysicalDeviceCount,
        IntPtr pPhysicalDevices
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkGetPhysicalDeviceProperties(
        IntPtr physicalDevice,
        out VkPhysicalDeviceProperties pProperties
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkGetPhysicalDeviceQueueFamilyProperties(
        IntPtr physicalDevice,
        ref uint pQueueFamilyPropertiesCount,
        IntPtr pQueueFamilyProperties
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkDestroySurfaceKHR(
        IntPtr instance,
        IntPtr surface,
        IntPtr allocator
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkGetPhysicalDeviceSurfaceSupportKHR(
        IntPtr device,
        uint queueFamilyIndex,
        IntPtr surface,
        out uint pSupported
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkCreateDevice(
        IntPtr physicalDevice,
        ref VkDeviceCreateInfo pCreateInfo,
        IntPtr pAllocator,
        out IntPtr pDevice
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkDestroyDevice(
        IntPtr device,
        IntPtr pAllocator
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkGetDeviceQueue(
        IntPtr device,
        uint queueFamilyIndex,
        uint queueIndex,
        out IntPtr pQueue
    );
}