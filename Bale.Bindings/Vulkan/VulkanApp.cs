namespace Bale.Bindings.Vulkan;

public sealed class VulkanApp : IDisposable {
    private readonly GLFWWindow _window;
    private readonly VulkanInstance _vulkanInstance;
    private readonly VulkanSurfaceManager _vulkanSurfaceManager;
    private readonly VulkanLogicalDeviceManager _vulkanLogicalDeviceManager;

    public VulkanApp(string appName, int width, int height) {
        _window = new GLFWWindow(width, height, appName);
        _vulkanInstance = new VulkanInstance(appName, new Version(0, 0, 1));
        _vulkanSurfaceManager = new VulkanSurfaceManager(_vulkanInstance.Handle, _window);

        var vulkanPhysicalDeviceSelector = new VulkanPhysicalDeviceSelector(_vulkanInstance.Handle, _vulkanSurfaceManager.Surface);
        _vulkanLogicalDeviceManager = new VulkanLogicalDeviceManager(vulkanPhysicalDeviceSelector.PhysicalDevice, _vulkanSurfaceManager.Surface);
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
    }
}