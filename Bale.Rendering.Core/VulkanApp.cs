using Microsoft.Extensions.Logging;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanApp : IDisposable {
    private readonly Window _window;
    private readonly VulkanInstance _vulkanInstance;
    private readonly VulkanSurfaceManager _vulkanSurfaceManager;
    private readonly VulkanLogicalDeviceManager _vulkanLogicalDeviceManager;
    private readonly VulkanSwapChainManager _vulkanSwapChainManager;
    private readonly ILogger<VulkanApp> _logger;

    public VulkanApp(
        Window window,
        VulkanInstance vulkanInstance,
        VulkanSurfaceManager vulkanSurfaceManager,
        VulkanLogicalDeviceManager vulkanLogicalDeviceManager,
        VulkanSwapChainManager vulkanSwapChainManager,
        ILogger<VulkanApp> logger
    ) {
        _logger = logger;
        _window = window;
        _vulkanInstance = vulkanInstance;
        _vulkanSurfaceManager = vulkanSurfaceManager;
        _vulkanLogicalDeviceManager = vulkanLogicalDeviceManager;
        _vulkanSwapChainManager = vulkanSwapChainManager;
    }

    public void Run() {
        while (!_window.ShouldClose) {
            _window.PollEvents();
        }
    }

    public void Dispose() {
        _vulkanSwapChainManager.Dispose();
        _vulkanLogicalDeviceManager.Dispose();
        _vulkanSurfaceManager.Dispose();
        _vulkanInstance.Dispose();
        _window.Dispose();

        _logger.LogInformation("disposed all managed objects");
    }
}