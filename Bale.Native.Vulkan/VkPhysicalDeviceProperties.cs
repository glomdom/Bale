using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct VkPhysicalDeviceProperties {
    public uint apiVersion;
    public uint driverVersion;
    public uint vendorID;
    public uint deviceID;
    public VkPhysicalDeviceType deviceType;

    public fixed byte deviceName[256]; // null terminated
    public fixed byte pipelineCacheUUID[16]; // 16-byte UUID

    public VkPhysicalDeviceLimits limits;
    public VkPhysicalDeviceSparseProperties sparseProperties;
}
