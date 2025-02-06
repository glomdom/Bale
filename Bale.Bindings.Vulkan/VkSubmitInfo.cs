using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkSubmitInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public uint waitSemaphoreCount;
    public IntPtr pWaitSemaphores;
    public IntPtr pWaitDstStageMask;
    public uint commandBufferCount;
    public IntPtr pCommandBuffers;
    public uint signalSemaphoreCount;
    public IntPtr pSignalSemaphores;
}