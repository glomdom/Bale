using System.Runtime.InteropServices;

namespace Bale.Bindings.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkDeviceCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public VkDeviceCreateFlags flags;
    public uint queueCreateInfoCount;
    public IntPtr pQueueCreateInfos;
    
    [Obsolete("Deprecated and should not be used.")]
    public uint enabledLayerCount;

    [Obsolete("Deprecated and should not be used.")]
    public IntPtr ppEnabledLayerNames;

    public uint enabledExtensionCount;
    public IntPtr ppEnabledExtensionNames;
    public IntPtr pEnabledFeatures;
}