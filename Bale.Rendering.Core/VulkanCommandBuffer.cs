using Bale.Bindings.Native;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanCommandBuffer : IDisposable {
    public IntPtr Handle { get; private set; }

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
        
        VulkanLow.vkFreeCommandBuffers(_device, _commandPool, 1, Handle);
        Handle = NULL;
    }
}