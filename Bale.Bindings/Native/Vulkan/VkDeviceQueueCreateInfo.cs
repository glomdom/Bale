using System.Runtime.InteropServices;

namespace Bale.Bindings.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkDeviceQueueCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public VkDeviceQueueCreateFlags flags;
    public uint queueFamilyIndex;
    public uint queueCount;
    public IntPtr pQueuePriorities;
}