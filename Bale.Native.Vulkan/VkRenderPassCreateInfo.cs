using System.Runtime.InteropServices;
using Bale.Native.Vulkan;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkRenderPassCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public VkRenderPassCreateFlags flags;
    public uint attachmentCount;
    public IntPtr pAttachments;
    public uint subpassCount;
    public IntPtr pSubpasses;
    public uint dependencyCount;
    public IntPtr pDependencies;
}