using Bale.Bindings.Native;
using Bale.Interop;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanFramebuffer : IDisposable {
    public IntPtr Handle { get; private set; }
    
    private readonly IntPtr _device;

    public VulkanFramebuffer(IntPtr device, IntPtr renderpass, VkExtent2D extent, IntPtr imageView) {
        _device = device;
        
        IntPtr[] attachments = [imageView];
        var framebufferInfo = new VkFramebufferCreateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
            renderPass = renderpass,
            attachmentCount = (uint)attachments.Length,
            pAttachments = new MarshaledStructArray<IntPtr>(attachments),
            width = extent.width,
            height = extent.height,
            layers = 1
        };

        var result = VulkanLow.vkCreateFramebuffer(device, ref framebufferInfo, NULL, out var framebuffer);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create framebuffer: {result}");
        }
        
        Handle = framebuffer;
    }
    
    public void Dispose() {
        if (Handle == NULL) return;
        
        VulkanLow.vkDestroyFramebuffer(_device, Handle, NULL);
        Handle = NULL;
    }
}