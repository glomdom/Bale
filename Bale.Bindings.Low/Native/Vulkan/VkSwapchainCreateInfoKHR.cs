using System.Runtime.InteropServices;

namespace Bale.Bindings.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkSwapchainCreateInfoKHR {
    public VkStructureType sType;
    public IntPtr pNext;
    public VkSwapchainCreateFlagsKHR flags;
    public IntPtr surface;
    public uint minImageCount;
    public VkFormat imageFormat;
    public VkColorSpaceKHR imageColorSpace;
    public VkExtent2D imageExtent;
    public uint imageArrayLayers;
    public VkImageUsageFlags imageUsage;
    public VkSharingMode imageSharingMode;
    public uint queueFamilyIndexCount;
    public IntPtr pQueueFamilyIndices;
    public VkSurfaceTransformFlagsKHR preTransform;
    public VkCompositeAlphaFlagsKHR compositeAlpha;
    public VkPresentModeKHR presentMode;
    public uint clipped;
    public IntPtr oldSwapchain;
}