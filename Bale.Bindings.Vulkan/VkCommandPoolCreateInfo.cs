using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkCommandPoolCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public VkCommandPoolCreateFlags flags;
    public uint queueFamilyIndex;
}