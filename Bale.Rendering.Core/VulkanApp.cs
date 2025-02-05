using Serilog;
using Bale.Bindings.Vulkan;

public sealed class VulkanApp : IDisposable {
    private readonly Window _window;
    private readonly VulkanInstance _vulkanInstance;
    private readonly VulkanSurfaceManager _vulkanSurfaceManager;
    private readonly VulkanPhysicalDeviceSelector _physicalDeviceSelector;
    private readonly VulkanLogicalDeviceManager _vulkanLogicalDeviceManager;
    private readonly VulkanSwapchainManager _vulkanSwapchainManager;

    public VulkanApp() {
        _window = new Window(800, 600, "Bale");
        _vulkanInstance = new VulkanInstance("Bale", new Version(0, 0, 1));
        _vulkanSurfaceManager = new VulkanSurfaceManager(_vulkanInstance, _window);
        _physicalDeviceSelector = new VulkanPhysicalDeviceSelector(_vulkanInstance, _vulkanSurfaceManager);
        _vulkanLogicalDeviceManager = new VulkanLogicalDeviceManager(_physicalDeviceSelector, _vulkanSurfaceManager);
        _vulkanSwapchainManager = new VulkanSwapchainManager(_vulkanLogicalDeviceManager, _physicalDeviceSelector, _vulkanSurfaceManager);
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

        Log.Information("Disposed all managed resources");
    }
}