﻿namespace Bale.Bindings.Vulkan;

[Flags]
public enum VkFramebufferCreateFlags : uint {
    VK_FRAMEBUFFER_CREATE_IMAGELESS_BIT = 0x00000001,
    VK_FRAMEBUFFER_CREATE_IMAGELESS_BIT_KHR = VK_FRAMEBUFFER_CREATE_IMAGELESS_BIT,
}