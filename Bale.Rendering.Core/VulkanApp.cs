using Serilog;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanApp : IDisposable {
    private readonly Window _window;
    private readonly VulkanInstance _vulkanInstance;
    private readonly VulkanSurfaceManager _vulkanSurfaceManager;
    private readonly VulkanPhysicalDeviceSelector _physicalDeviceSelector;
    private readonly VulkanLogicalDeviceManager _vulkanLogicalDeviceManager;
    private readonly VulkanSwapchainManager _vulkanSwapchainManager;
    private readonly VulkanRenderPass _renderPass;
    private readonly List<VulkanFramebuffer> _framebuffers = [];
    private readonly List<VulkanCommandBuffer> _commandBuffers = [];
    private int _currentFrame = 0;

    public VulkanApp() {
        _window = new Window(800, 600, "Bale");
        _vulkanInstance = new VulkanInstance("Bale", new Version(0, 0, 1));
        _vulkanSurfaceManager = new VulkanSurfaceManager(_vulkanInstance, _window);
        _physicalDeviceSelector = new VulkanPhysicalDeviceSelector(_vulkanInstance, _vulkanSurfaceManager);
        _vulkanLogicalDeviceManager = new VulkanLogicalDeviceManager(_physicalDeviceSelector, _vulkanSurfaceManager);
        _vulkanSwapchainManager = new VulkanSwapchainManager(_vulkanLogicalDeviceManager, _physicalDeviceSelector, _vulkanSurfaceManager);
        _renderPass = new VulkanRenderPass(_vulkanLogicalDeviceManager.Device, _vulkanSwapchainManager.SurfaceFormat.format);

        foreach (var imageView in _vulkanSwapchainManager.SwapchainImages) {
            _framebuffers.Add(new VulkanFramebuffer(_vulkanLogicalDeviceManager.Device, _renderPass.Handle, _vulkanSwapchainManager.SwapExtent, imageView));
        }

        for (var i = 0; i < _framebuffers.Count; i++) {
            _commandBuffers.Add(new VulkanCommandBuffer(_vulkanLogicalDeviceManager.Device, _vulkanLogicalDeviceManager.CommandPool));
        }
    }

    public void Run() {
        while (!_window.ShouldClose) {
            _window.PollEvents();
        }
    }

    public void Dispose() {
        foreach (var framebuffer in _framebuffers) framebuffer.Dispose();
        
        _renderPass.Dispose();
        _vulkanSwapchainManager.Dispose();
        _vulkanLogicalDeviceManager.Dispose();
        _vulkanSurfaceManager.Dispose();
        _vulkanInstance.Dispose();
        _window.Dispose();

        Log.Information("disposed all managed resources");
    }
}