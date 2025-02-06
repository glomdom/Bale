using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkCommandPoolCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public VkCommandPoolCreateFlags flags;
    public uint queueFamilyIndex;
}