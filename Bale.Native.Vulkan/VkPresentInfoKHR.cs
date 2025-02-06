using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkPresentInfoKHR {
    public VkStructureType sType;
    public IntPtr pNext;
    public uint waitSemaphoreCount;
    public IntPtr pWaitSemaphores;
    public uint swapchainCount;
    public IntPtr pSwapchains;
    public IntPtr pImageIndices;
    public IntPtr pResults;
}