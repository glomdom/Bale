using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkRenderPassBeginInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public IntPtr renderPass;
    public IntPtr framebuffer;
    public VkRect2D renderArea;
    public uint clearValueCount;
    public IntPtr pClearValues;
}