using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkComponentMapping {
    public VkComponentSwizzle r;
    public VkComponentSwizzle g;
    public VkComponentSwizzle b;
    public VkComponentSwizzle a;
}