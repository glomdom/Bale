using System.Runtime.InteropServices;

namespace Bale.Bindings.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkInstanceCreateInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public uint flags;
    public IntPtr pApplicationInfo;
    public uint enabledLayerCount;
    public IntPtr ppEnabledLayerNames;
    public uint enabledExtensionCount;
    public IntPtr ppEnabledExtensionNames;
}