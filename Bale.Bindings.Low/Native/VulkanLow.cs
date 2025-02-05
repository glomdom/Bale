using System.Runtime.InteropServices;
using Bale.Bindings.Vulkan;

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

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkGetPhysicalDeviceSurfaceCapabilitiesKHR(
        IntPtr device,
        IntPtr surface,
        out VkSurfaceCapabilitiesKHR pSurfaceCapabilities
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkGetPhysicalDeviceSurfaceFormatsKHR(
        IntPtr physicalDevice,
        IntPtr surface,
        ref uint pSurfaceFormatCount,
        IntPtr pSurfaceFormats
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkGetPhysicalDeviceSurfacePresentModesKHR(
        IntPtr physicalDevice,
        IntPtr surface,
        ref uint pPresentModeCount,
        IntPtr pPresentModes
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkCreateSwapchainKHR(
        IntPtr device,
        ref VkSwapchainCreateInfoKHR pCreateInfo,
        IntPtr pAllocator,
        out IntPtr pSwapchain
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkGetSwapchainImagesKHR(
        IntPtr device,
        IntPtr swapchain,
        ref uint pSwapchainImageCount,
        IntPtr pSwapchainImages
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkDestroySwapchainKHR(
        IntPtr device,
        IntPtr swapchain,
        IntPtr pAllocator
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkDeviceWaitIdle(IntPtr device);

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkCreateRenderPass(
        IntPtr device,
        ref VkRenderPassCreateInfo pCreateInfo,
        IntPtr pAllocator,
        out IntPtr pRenderPass
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkDestroyRenderPass(
        IntPtr device,
        IntPtr renderPass,
        IntPtr pAllocator
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkCreateFramebuffer(
        IntPtr device,
        ref VkFramebufferCreateInfo pCreateInfo,
        IntPtr pAllocator,
        out IntPtr pFramebuffer
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkDestroyFramebuffer(
        IntPtr device,
        IntPtr framebuffer,
        IntPtr pAllocator
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkAllocateCommandBuffers(
        IntPtr device,
        ref VkCommandBufferAllocateInfo pAllocateInfo,
        out IntPtr pCommandBuffers
    );

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkBeginCommandBuffer(
        IntPtr commandBuffer,
        ref VkCommandBufferBeginInfo pBeginInfo
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkEndCommandBuffer(IntPtr commandBuffer);

    [LibraryImport("vulkan-1")]
    internal static partial void vkFreeCommandBuffers(
        IntPtr device,
        IntPtr commandPool,
        uint commandBufferCount,
        ref IntPtr commandBuffers
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkCmdBeginRenderPass(
        IntPtr commandBuffer,
        ref VkRenderPassBeginInfo pBeginInfo,
        VkSubpassContents contents
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkCmdEndRenderPass(IntPtr commandBuffer);

    public const uint VK_SUBPASS_EXTERNAL = unchecked((uint)-1);
}