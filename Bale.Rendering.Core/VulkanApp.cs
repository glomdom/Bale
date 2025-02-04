using Microsoft.Extensions.Logging;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanApp : IDisposable {
    private readonly Window _window;
    private readonly VulkanInstance _vulkanInstance;
    private readonly VulkanSurfaceManager _vulkanSurfaceManager;
    private readonly VulkanLogicalDeviceManager _vulkanLogicalDeviceManager;
    private readonly VulkanSwapchainManager _vulkanSwapchainManager;
    private readonly ILogger<VulkanApp> _logger;

    public VulkanApp(
        Window window,
        VulkanInstance vulkanInstance,
        VulkanSurfaceManager vulkanSurfaceManager,
        VulkanLogicalDeviceManager vulkanLogicalDeviceManager,
        VulkanSwapchainManager vulkanSwapchainManager,
        ILogger<VulkanApp> logger
    ) {
        _logger = logger;
        _window = window;
        _vulkanInstance = vulkanInstance;
        _vulkanSurfaceManager = vulkanSurfaceManager;
        _vulkanLogicalDeviceManager = vulkanLogicalDeviceManager;
        _vulkanSwapchainManager = vulkanSwapchainManager;
    }

    public void Run() {
        while (!_window.ShouldClose) {
            _window.PollEvents();
        }
    }

    public void Dispose() {
        _vulkanSwapchainManager.Dispose();
        _vulkanLogicalDeviceManager.Dispose();
        _vulkanSurfaceManager.Dispose();
        _vulkanInstance.Dispose();
        _window.Dispose();

        _logger.LogInformation("disposed all managed objects");
    }
}