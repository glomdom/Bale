using Serilog;
using Bale.Bindings.Native;
using Bale.Interop;
using static Bale.Bindings.Common;

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

    private IntPtr _imageAvailableSemaphore;
    private IntPtr _renderFinishedSemaphore;
    private int _currentFrame = 0;

    public VulkanApp() {
        _window = new Window(800, 600, "Bale");
        _vulkanInstance = new VulkanInstance("Bale", new Version(0, 0, 1));
        _vulkanSurfaceManager = new VulkanSurfaceManager(_vulkanInstance, _window);
        _physicalDeviceSelector = new VulkanPhysicalDeviceSelector(_vulkanInstance, _vulkanSurfaceManager);
        _vulkanLogicalDeviceManager = new VulkanLogicalDeviceManager(_physicalDeviceSelector, _vulkanSurfaceManager);
        _vulkanSwapchainManager = new VulkanSwapchainManager(_vulkanLogicalDeviceManager, _physicalDeviceSelector, _vulkanSurfaceManager);
        _renderPass = new VulkanRenderPass(_vulkanLogicalDeviceManager.Device, _vulkanSwapchainManager.SurfaceFormat.format);

        foreach (var imageView in _vulkanSwapchainManager.SwapchainImageViews) {
            _framebuffers.Add(new VulkanFramebuffer(_vulkanLogicalDeviceManager.Device, _renderPass.Handle, _vulkanSwapchainManager.SwapExtent, imageView));
        }

        for (var i = 0; i < _framebuffers.Count; i++) {
            _commandBuffers.Add(new VulkanCommandBuffer(_vulkanLogicalDeviceManager.Device, _vulkanLogicalDeviceManager.CommandPool));
        }
        
        CreateSyncObjects();
    }

    public void Run() {
        while (!_window.ShouldClose) {
            _window.PollEvents();
            DrawFrame();

            _currentFrame = (_currentFrame + 1) % _framebuffers.Count;
        }
    }

    public void Dispose() {
        foreach (var framebuffer in _framebuffers) framebuffer.Dispose();
        foreach (var commandBuffer in _commandBuffers) commandBuffer.Dispose();

        _renderPass.Dispose();
        _vulkanSwapchainManager.Dispose();
        _vulkanLogicalDeviceManager.Dispose();
        _vulkanSurfaceManager.Dispose();
        _vulkanInstance.Dispose();
        _window.Dispose();

        Log.Information("disposed all managed resources");
    }

    private void CreateSyncObjects() {
        var semaphoreInfo = new VkSemaphoreCreateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO
        };

        if (
            VulkanLow.vkCreateSemaphore(_vulkanLogicalDeviceManager.Device, ref semaphoreInfo, NULL, out _imageAvailableSemaphore) != VkResult.VK_SUCCESS ||
            VulkanLow.vkCreateSemaphore(_vulkanLogicalDeviceManager.Device, ref semaphoreInfo, NULL, out _renderFinishedSemaphore) != VkResult.VK_SUCCESS
        ) {
            throw new Exception("Failed to create synchronization objects for Vulkan rendering");
        }
        
        Log.Debug("created imageview sync objects");
    }

    private void DrawFrame() {
        var commandBuffer = _commandBuffers[_currentFrame];
        commandBuffer.Begin();

        var clearValue = new VkClearValue();
        unsafe {
            clearValue.color.float32[0] = 1.0f; // red
            clearValue.color.float32[1] = 1.0f; // green
            clearValue.color.float32[2] = 0.0f; // blue
            clearValue.color.float32[3] = 1.0f; // alpha
        }

        commandBuffer.RecordRenderPass(_renderPass.Handle, _framebuffers[_currentFrame].Handle, _vulkanSwapchainManager.SwapExtent, clearValue);
        commandBuffer.End();

        var submitInfo = new VkSubmitInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO,
            commandBufferCount = 1,
            pCommandBuffers = new MarshaledStructArray<IntPtr>([commandBuffer.Handle])
        };

        VulkanLow.vkQueueSubmit(_vulkanLogicalDeviceManager.GraphicsQueue, 1, ref submitInfo, NULL);
        VulkanLow.vkQueueWaitIdle(_vulkanLogicalDeviceManager.GraphicsQueue);

        var presentInfo = new VkPresentInfoKHR {
            sType = VkStructureType.VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
            swapchainCount = 1,
            pSwapchains = new MarshaledStructArray<IntPtr>([_vulkanSwapchainManager.Swapchain]),
            pImageIndices = new MarshaledValue<uint>((uint)_currentFrame)
        };

        var result = VulkanLow.vkQueuePresentKHR(_vulkanLogicalDeviceManager.PresentQueue, ref presentInfo);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to present swapchain image: {result}");
        }
    }
}