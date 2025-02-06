using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkSemaphoreCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public uint flags; // reserved
}