using System.Runtime.InteropServices;
using Bale.Bindings.Native;
using Bale.Bindings.Native.Vulkan;
using static Bale.Bindings.Common;
using Microsoft.Extensions.Logging;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanSwapChainManager : IDisposable {
    private readonly VulkanLogicalDeviceManager _deviceManager;
    private readonly VulkanPhysicalDeviceSelector _physicalDeviceSelector;
    private readonly VulkanSurfaceManager _surfaceManager;
    private readonly ILogger<VulkanSwapChainManager> _logger;
    private IntPtr _swapChain;
    private VkSurfaceFormatKHR _surfaceFormat;
    private VkPresentModeKHR _presentMode;
    private VkExtent2D _swapExtent;
    private IntPtr[] _swapChainImages = [];

    public IntPtr SwapChain => _swapChain;
    public VkExtent2D SwapExtent => _swapExtent;
    public VkSurfaceFormatKHR SurfaceFormat => _surfaceFormat;
    public IntPtr[] SwapChainImages => _swapChainImages;

    public VulkanSwapChainManager(VulkanLogicalDeviceManager deviceManager,
        VulkanPhysicalDeviceSelector physicalDeviceSelector,
        VulkanSurfaceManager surfaceManager,
        ILogger<VulkanSwapChainManager> logger
    ) {
        _deviceManager = deviceManager;
        _physicalDeviceSelector = physicalDeviceSelector;
        _surfaceManager = surfaceManager;
        _logger = logger;

        CreateSwapChain();
    }

    public void Dispose() { }

    private void CreateSwapChain() {
        var physicalDevice = _physicalDeviceSelector.PhysicalDevice;
        var surface = _surfaceManager.Surface;

        _logger.LogInformation("querying swapchain support");

        QuerySwapChainSupport(physicalDevice, surface, out var capabilities, out var formats, out var presentModes);
        
        _surfaceFormat = ChooseSurfaceFormat(formats);
        _presentMode = ChoosePresentMode(presentModes);
        _swapExtent = ChooseSwapExtent(capabilities);

        var imageCount = capabilities.minImageCount + 1;
        if (capabilities.maxImageCount > 0 && imageCount > capabilities.maxImageCount) {
            imageCount = capabilities.maxImageCount;
        }
        
        var swapChainCreateInfo = new VkSwapchainCreateInfoKHR {
            sType = VkStructureType.VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
            surface = surface,
            minImageCount = imageCount,
            imageFormat = _surfaceFormat.format,
            imageColorSpace = _surfaceFormat.colorSpace,
            imageExtent = _swapExtent,
            imageArrayLayers = 1,
            imageUsage = VkImageUsageFlags.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
            preTransform = capabilities.currentTransform,
            compositeAlpha = VkCompositeAlphaFlagsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR,
            presentMode = _presentMode,
            clipped = TRUE
        };
        
        var result = VulkanLow.vkCreateSwapchainKHR(_deviceManager.Device, ref swapChainCreateInfo, NULL, out _swapChain);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create swap chain: {result}");
        }
        
        _logger.LogInformation("created Vulkan swapchain successfully");

        VulkanLow.vkGetSwapchainImagesKHR(_deviceManager.Device, _swapChain, ref imageCount, NULL);
        _swapChainImages = new IntPtr[imageCount];
        VulkanLow.vkGetSwapchainImagesKHR(_deviceManager.Device, _swapChain, ref imageCount, Marshal.UnsafeAddrOfPinnedArrayElement(_swapChainImages, 0));
        
        _logger.LogInformation("retrieved {imageCount} swapchain images", imageCount);
    }

    private void QuerySwapChainSupport(IntPtr physicalDevice, IntPtr surface, out VkSurfaceCapabilitiesKHR capabilities, out VkSurfaceFormatKHR[] formats,
        out VkPresentModeKHR[] presentModes) {
        VulkanLow.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, out capabilities);

        uint formatCount = 0;
        VulkanLow.vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, ref formatCount, NULL);
        formats = new VkSurfaceFormatKHR[formatCount];
        VulkanLow.vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, ref formatCount, Marshal.UnsafeAddrOfPinnedArrayElement(formats, 0));

        uint presentModeCount = 0;
        VulkanLow.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, ref presentModeCount, NULL);
        presentModes = new VkPresentModeKHR[presentModeCount];
        VulkanLow.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, ref presentModeCount, Marshal.UnsafeAddrOfPinnedArrayElement(presentModes, 0));
    }

    private VkSurfaceFormatKHR ChooseSurfaceFormat(VkSurfaceFormatKHR[] formats) {
        var preferredFormat = formats.FirstOrDefault(f =>
            f.format == VkFormat.VK_FORMAT_B8G8R8A8_SRGB &&
            f.colorSpace == VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR
        );

        return preferredFormat.format != 0 ? preferredFormat : formats[0];
    }

    private VkPresentModeKHR ChoosePresentMode(VkPresentModeKHR[] presentModes) {
        return presentModes.Contains(VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR)
            ? VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR
            : VkPresentModeKHR.VK_PRESENT_MODE_FIFO_KHR;
    }

    private VkExtent2D ChooseSwapExtent(VkSurfaceCapabilitiesKHR capabilities) {
        return new VkExtent2D {
            width = capabilities.currentExtent.width,
            height = capabilities.currentExtent.height
        };
    }
}