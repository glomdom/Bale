using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkSubpassDependency {
    public uint srcSubpass;
    public uint dstSubpass;
    public VkPipelineStageFlags srcStageFlags;
    public VkPipelineStageFlags dstStageFlags;
    public VkAccessFlags srcAccessMask;
    public VkAccessFlags dstAccessMask;
    public VkDependencyFlags dependencyFlags;
}