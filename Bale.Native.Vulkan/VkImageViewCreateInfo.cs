using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkImageViewCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public VkImageViewCreateFlags flags;
    public IntPtr image;
    public VkImageViewType viewType;
    public VkFormat format;
    public VkComponentMapping components;
    public VkImageSubresourceRange subresourceRange;
}