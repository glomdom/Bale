using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkExtent3D {
    public uint width;
    public uint height;
    public uint depth;
}