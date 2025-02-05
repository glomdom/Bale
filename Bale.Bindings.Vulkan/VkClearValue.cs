using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Explicit)]
public struct VkClearValue {
    [FieldOffset(0)] public VkClearColorValue color;
    [FieldOffset(0)] public VkClearDepthStencilValue depthStencil;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct VkClearColorValue {
    [FieldOffset(0)] public fixed float float32[4];
    [FieldOffset(0)] public fixed int int32[4];
    [FieldOffset(0)] public fixed uint uint32[4];
}

[StructLayout(LayoutKind.Explicit)]
public struct VkClearDepthStencilValue {
    [FieldOffset(0)] public float depth;
    [FieldOffset(0)] public uint stencil;
}