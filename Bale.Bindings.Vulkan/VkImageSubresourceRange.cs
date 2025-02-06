using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkImageSubresourceRange {
    public VkImageAspectFlags aspectMask;
    public uint baseMipLevel;
    public uint levelCount;
    public uint baseArrayLayer;
    public uint layerCount;
}