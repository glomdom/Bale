using System.Runtime.InteropServices;

namespace Bale.Native.Vulkan;

[StructLayout(LayoutKind.Sequential)]
public struct VkSurfaceFormatKHR {
    public VkFormat format;
    public VkColorSpaceKHR colorSpace;
}