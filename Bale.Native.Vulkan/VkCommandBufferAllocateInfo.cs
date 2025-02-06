using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkCommandBufferAllocateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public IntPtr commandPool;
    public VkCommandBufferLevel level;
    public uint commandBufferCount;
}