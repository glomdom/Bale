using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkAttachmentReference {
    public uint attachment;
    public VkImageLayout layout;
}