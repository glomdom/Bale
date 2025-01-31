using Bale.Bindings.Native.Vulkan;
using static Bale.Bindings.Native.VulkanLow;
using static Bale.Bindings.Native.GLFWLow;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanSurfaceManager : IDisposable {
    private readonly IntPtr _instanceHandle;
    private readonly IntPtr _surface;

    public VulkanSurfaceManager(IntPtr instanceHandle, GLFWWindow window) {
        _instanceHandle = instanceHandle;
        
        var result = glfwCreateWindowSurface(_instanceHandle, window.Handle, NULL, out _surface);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create Vulkan surface: {result}");
        }
        
        Console.WriteLine("Created Vulkan surface successfully");
    }
    
    public IntPtr Surface => _surface;
    
    public void Dispose() {
        if (_surface == NULL) return;
        
        vkDestroySurfaceKHR(_instanceHandle, _surface, NULL);
    }
}