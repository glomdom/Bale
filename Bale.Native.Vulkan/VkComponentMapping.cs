using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkComponentMapping {
    public VkComponentSwizzle r;
    public VkComponentSwizzle g;
    public VkComponentSwizzle b;
    public VkComponentSwizzle a;
}