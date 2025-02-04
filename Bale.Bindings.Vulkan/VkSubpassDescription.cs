using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkSubpassDescription {
    public VkSubpassDescriptionFlags flags;
    public VkPipelineBindPoint pipelineBindPoint;
    public uint inputAttachmentCount;
    public IntPtr pInputAttachments;
    public uint colorAttachmentCount;
    public IntPtr pColorAttachments;
    public IntPtr pResolveAttachments;
    public IntPtr pDepthStencilAttachment;
    public uint preserveAttachmentCount;
    public IntPtr pPreserveAttachments;
}