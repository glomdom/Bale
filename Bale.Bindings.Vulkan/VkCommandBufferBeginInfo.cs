using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkCommandBufferBeginInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public VkCommandBufferUsageFlags flags;
    public IntPtr pInheritanceInfo;
}