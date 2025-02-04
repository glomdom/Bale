using Bale.Bindings.Native;
using Bale.Bindings.Utilities;

using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanRenderPass : IDisposable {
    public IntPtr Handle { get; private set; }

    private readonly IntPtr _device;

    public VulkanRenderPass(IntPtr device, VkFormat swapchainImageFormat) {
        _device = device;

        var colorAttachment = new VkAttachmentDescription {
            format = swapchainImageFormat,
            samples = VkSampleCountFlags.VK_SAMPLE_COUNT_1_BIT,
            loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
            storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
            stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
            stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
            initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
            finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR
        };

        var colorAttachmentRef = new VkAttachmentReference {
            attachment = 0,
            layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
        };

        var subpass = new VkSubpassDescription {
            pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
            colorAttachmentCount = 1,
            pColorAttachments = new MarshaledStruct<VkAttachmentReference>(colorAttachmentRef)
        };

        var dependency = new VkSubpassDependency {
            srcSubpass = VulkanLow.VK_SUBPASS_EXTERNAL,
            dstSubpass = 0,
            srcStageFlags = VkPipelineStageFlags.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
            dstStageFlags = VkPipelineStageFlags.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
            srcAccessMask = 0,
            dstAccessMask = VkAccessFlags.VK_ACCESS_COLOR_ATTACHMENT_READ_BIT | VkAccessFlags.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT,
            dependencyFlags = 0,
        };

        var renderPassInfo = new VkRenderPassCreateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
            attachmentCount = 1,
            pAttachments = new MarshaledStruct<VkAttachmentDescription>(colorAttachment),
            subpassCount = 1,
            pSubpasses = new MarshaledStruct<VkSubpassDescription>(subpass),
            dependencyCount = 1,
            pDependencies = new MarshaledStruct<VkSubpassDependency>(dependency)
        };

        var result = VulkanLow.vkCreateRenderPass(_device, ref renderPassInfo, NULL, out var renderPass);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create renderpass: {result}");
        }

        Handle = renderPass;
    }
    
    public void Dispose() {
        // TODO release managed resources here
    }
}