using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkRect2D {
    public VkOffset2D offset;
    public VkExtent2D extent;
}