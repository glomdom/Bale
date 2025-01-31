using Bale.Bindings.Native.Vulkan;
using Microsoft.Extensions.Logging;
using static Bale.Bindings.Native.VulkanLow;
using static Bale.Bindings.Native.GLFWLow;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanSurfaceManager : IDisposable {
    private readonly VulkanInstance _vulkanInstance;
    private readonly Window _window;
    private readonly ILogger<VulkanSurfaceManager> _logger;
    private IntPtr _surface;

    public VulkanSurfaceManager(VulkanInstance vulkanInstance, Window window, ILogger<VulkanSurfaceManager> logger) {
        _vulkanInstance = vulkanInstance;
        _window = window;
        _logger = logger;

        var result = glfwCreateWindowSurface(_vulkanInstance.Handle, _window.Handle, NULL, out _surface);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create Vulkan surface: {result}");
        }

        _logger.LogInformation("created Vulkan surface");
    }
    
    public IntPtr Surface => _surface;
    
    public void Dispose() {
        if (_surface == NULL) return;
        
        vkDestroySurfaceKHR(_vulkanInstance.Handle, _surface, NULL);
        _surface = NULL;
    }
}