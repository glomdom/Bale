using Microsoft.Extensions.Logging;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanApp : IDisposable {
    private readonly Window _window;
    private readonly VulkanInstance _vulkanInstance;
    private readonly VulkanSurfaceManager _vulkanSurfaceManager;
    private readonly VulkanLogicalDeviceManager _vulkanLogicalDeviceManager;
    private readonly ILogger<VulkanApp> _logger;

    public VulkanApp(
        Window window,
        VulkanInstance vulkanInstance,
        VulkanSurfaceManager vulkanSurfaceManager,
        VulkanLogicalDeviceManager vulkanLogicalDeviceManager,
        ILogger<VulkanApp> logger
    ) {
        _logger = logger;
        _window = window;
        _vulkanInstance = vulkanInstance;
        _vulkanSurfaceManager = vulkanSurfaceManager;
        _vulkanLogicalDeviceManager = vulkanLogicalDeviceManager;
    }

    public void Run() {
        while (!_window.ShouldClose) {
            _window.PollEvents();
        }
    }

    public void Dispose() {
        _vulkanLogicalDeviceManager.Dispose();
        _vulkanSurfaceManager.Dispose();
        _vulkanInstance.Dispose();
        _window.Dispose();

        _logger.LogInformation("disposed all managed objects");
    }
}