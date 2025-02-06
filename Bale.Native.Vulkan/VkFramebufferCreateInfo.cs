using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkFramebufferCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public VkFramebufferCreateFlags flags;
    public IntPtr renderPass;
    public uint attachmentCount;
    public IntPtr pAttachments;
    public uint width;
    public uint height;
    public uint layers;
}