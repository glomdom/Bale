using System.Runtime.InteropServices;
using Bale.Bindings.Native.Vulkan;

namespace Bale.Bindings.Native;

/// <summary>
/// Low-level exports of Vulkan functions.
/// </summary>
public static partial class VulkanLow {
    static VulkanLow() {
        _ = typeof(NativeResolver);
    }

    [LibraryImport("vulkan-1")]
    internal static partial VkResult vkCreateInstance(
        ref VkInstanceCreateInfo pCreateInfo,
        IntPtr pAllocator,
        out IntPtr pInstance
    );

    [LibraryImport("vulkan-1")]
    internal static partial void vkDestroyInstance(
        IntPtr instance,
        IntPtr pAllocator
    );
}