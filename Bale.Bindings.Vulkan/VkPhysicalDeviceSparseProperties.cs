using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkPhysicalDeviceSparseProperties {
    public uint residencyStandard2DBlockShape;
    public uint residencyStandard2DMultisampleBlockShape;
    public uint residencyStandard3DBlockShape;
    public uint residencyAlignedMipSize;
    public uint residencyNonResidentStrict;
}