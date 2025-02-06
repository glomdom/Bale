using Serilog;
using Bale.Native.GLFW;
using Bale.Native.Vulkan;
using Bale.Platform.GLFW;
using static Bale.Native.Core.Common;

namespace Bale.Graphics.Vulkan;

public sealed class VulkanSurfaceManager : IDisposable {
    private readonly VulkanInstance _vulkanInstance;
    private readonly Window _window;
    private IntPtr _surface;

    public VulkanSurfaceManager(VulkanInstance vulkanInstance, Window window) {
        _vulkanInstance = vulkanInstance;
        _window = window;

        var result = GLFWLow.glfwCreateWindowSurface(_vulkanInstance.Handle, _window.Handle, NULL, out _surface);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create Vulkan surface: {result}");
        }

        Log.Information("created Vulkan surface");
    }

    public IntPtr Surface => _surface;

    public void Dispose() {
        if (_surface == NULL) return;

        VulkanLow.vkDestroySurfaceKHR(_vulkanInstance.Handle, _surface, NULL);
        _surface = NULL;
    }
}