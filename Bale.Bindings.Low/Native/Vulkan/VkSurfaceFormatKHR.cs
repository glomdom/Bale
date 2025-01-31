using System.Runtime.InteropServices;

namespace Bale.Bindings.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkSurfaceFormatKHR {
    public VkFormat format;
    public VkColorSpaceKHR colorSpace;
}