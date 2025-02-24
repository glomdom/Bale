using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkQueueFamilyProperties {
    public VkQueueFlags queueFlags;
    public uint queueCount;
    public uint timestampValidBits;
    public VkExtent3D minImageTransferGranularity;
}