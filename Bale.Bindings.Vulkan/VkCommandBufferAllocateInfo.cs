using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkCommandBufferAllocateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public IntPtr commandPool;
    public VkCommandBufferLevel level;
    public uint commandBufferCount;
}