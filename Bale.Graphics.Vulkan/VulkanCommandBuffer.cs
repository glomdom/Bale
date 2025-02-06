using Bale.Native.Vulkan;
using Bale.Interop;
using static Bale.Native.Core.Common;

namespace Bale.Graphics.Vulkan;

public sealed class VulkanCommandBuffer : IDisposable {
    public IntPtr Handle {
        get => _handle;
        private set => _handle = value;
    }

    private IntPtr _handle;
    private readonly IntPtr _device;
    private readonly IntPtr _commandPool;

    public VulkanCommandBuffer(IntPtr device, IntPtr commandPool) {
        _device = device;
        _commandPool = commandPool;

        var allocInfo = new VkCommandBufferAllocateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            commandPool = commandPool,
            level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY,
            commandBufferCount = 1
        };

        var result = VulkanLow.vkAllocateCommandBuffers(device, ref allocInfo, out var commandBuffer);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to allocate command buffer: {result}");
        }

        Handle = commandBuffer;
    }

    public void RecordRenderPass(IntPtr renderPass, IntPtr framebuffer, VkExtent2D extent, VkClearValue clearColor) {
        var renderPassInfo = new VkRenderPassBeginInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
            renderPass = renderPass,
            framebuffer = framebuffer,
            renderArea = new VkRect2D { offset = new VkOffset2D { x = 0, y = 0 }, extent = extent },
            clearValueCount = 1,
            pClearValues = new MarshaledStruct<VkClearValue>(clearColor)
        };

        VulkanLow.vkCmdBeginRenderPass(Handle, ref renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
        VulkanLow.vkCmdEndRenderPass(Handle);
    }

    public void Begin() {
        var beginInfo = new VkCommandBufferBeginInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
            flags = VkCommandBufferUsageFlags.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
        };

        VulkanLow.vkBeginCommandBuffer(Handle, ref beginInfo);
    }

    public void End() {
        VulkanLow.vkEndCommandBuffer(Handle);
    }

    public void Dispose() {
        if (Handle == NULL) return;

        VulkanLow.vkFreeCommandBuffers(_device, _commandPool, 1, ref _handle);
        Handle = NULL;
    }
}