using System.Runtime.InteropServices;

namespace Bale.Bindings.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkApplicationInfo {
    public VkStructureType sType;
    public IntPtr pNext;
    public IntPtr pApplicationName;
    public uint applicationVersion;
    public IntPtr pEngineName;
    public uint engineVersion;
    public uint apiVersion;
}