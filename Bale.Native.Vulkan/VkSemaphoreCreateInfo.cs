using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkSemaphoreCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public uint flags; // reserved
}