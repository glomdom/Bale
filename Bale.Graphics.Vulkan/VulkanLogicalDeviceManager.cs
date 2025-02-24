﻿using System.Runtime.InteropServices;
using Serilog;
using Bale.Native.Vulkan;
using Bale.Interop;
using static Bale.Native.Core.Common;

namespace Bale.Graphics.Vulkan;

public sealed class VulkanLogicalDeviceManager : IDisposable {
    private readonly VulkanPhysicalDeviceSelector _physicalDeviceSelector;
    private readonly VulkanSurfaceManager _surfaceManager;
    private IntPtr _device;
    private IntPtr _commandPool;
    private IntPtr _graphicsQueue;
    private IntPtr _presentQueue;
    private uint _graphicsFamily;

    public IntPtr Device => _device;
    public IntPtr CommandPool => _commandPool;
    public IntPtr GraphicsQueue => _graphicsQueue;
    public IntPtr PresentQueue => _presentQueue;

    public VulkanLogicalDeviceManager(
        VulkanPhysicalDeviceSelector physicalDeviceSelector,
        VulkanSurfaceManager surfaceManager
    ) {
        _physicalDeviceSelector = physicalDeviceSelector;
        _surfaceManager = surfaceManager;

        CreateDeviceAndQueues(_physicalDeviceSelector.PhysicalDevice, _surfaceManager.Surface);
        CreateCommandPool(_physicalDeviceSelector.PhysicalDevice);
    }

    private void CreateCommandPool(IntPtr physicalDevice) {
        var poolInfo = new VkCommandPoolCreateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
            flags = VkCommandPoolCreateFlags.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
            queueFamilyIndex = _graphicsFamily,
        };

        var result = VulkanLow.vkCreateCommandPool(_device, ref poolInfo, NULL, out _commandPool);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create command pool: {result}");
        }

        Log.Information("created command pool");
    }

    private void CreateDeviceAndQueues(IntPtr physicalDevice, IntPtr surface) {
        Log.Information("selecting queue families");

        uint queueFamilyCount = 0;
        VulkanLow.vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, ref queueFamilyCount, NULL);

        var queueFamilies = new VkQueueFamilyProperties[queueFamilyCount];
        VulkanLow.vkGetPhysicalDeviceQueueFamilyProperties(
            physicalDevice,
            ref queueFamilyCount,
            Marshal.UnsafeAddrOfPinnedArrayElement(queueFamilies, 0)
        );

        int graphicsFamily = -1, presentFamily = -1;
        for (uint i = 0; i < queueFamilyCount; i++) {
            if ((queueFamilies[i].queueFlags & VkQueueFlags.VK_QUEUE_GRAPHICS_BIT) != 0 && graphicsFamily == -1) {
                graphicsFamily = (int)i;
            }

            VulkanLow.vkGetPhysicalDeviceSurfaceSupportKHR(physicalDevice, i, surface, out var support);
            if (support == TRUE && presentFamily == -1) {
                presentFamily = (int)i;
            }

            if (graphicsFamily != -1 && presentFamily != -1) {
                break;
            }
        }

        if (graphicsFamily == -1 || presentFamily == -1) {
            throw new Exception("Failed to find required Vulkan queue families");
        }

        Log.Information("graphics queue family: {GraphicsFamily}, present queue family: {PresentFamily}", graphicsFamily, presentFamily);

        using var priority = new MarshaledValue<float>(1.0f);
        var queueCreateInfos = new List<VkDeviceQueueCreateInfo>();

        var graphicsQueueInfo = new VkDeviceQueueCreateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
            queueFamilyIndex = (uint)graphicsFamily,
            queueCount = 1,
            pQueuePriorities = priority
        };
        queueCreateInfos.Add(graphicsQueueInfo);

        if (presentFamily != graphicsFamily) {
            var presentQueueInfo = new VkDeviceQueueCreateInfo {
                sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                queueFamilyIndex = (uint)presentFamily,
                queueCount = 1,
                pQueuePriorities = priority
            };
            queueCreateInfos.Add(presentQueueInfo);
        }

        using var marshaledQueueInfos = new MarshaledStructArray<VkDeviceQueueCreateInfo>(queueCreateInfos.ToArray());

        string[] extensions = ["VK_KHR_swapchain"];
        using var marshaledExtensions = new MarshaledStringArray(extensions);

        var deviceCreateInfo = new VkDeviceCreateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
            queueCreateInfoCount = (uint)queueCreateInfos.Count,
            pQueueCreateInfos = marshaledQueueInfos,
            enabledExtensionCount = (uint)extensions.Length,
            ppEnabledExtensionNames = marshaledExtensions
        };

        var result = VulkanLow.vkCreateDevice(physicalDevice, ref deviceCreateInfo, NULL, out _device);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create Vulkan logical device: {result}");
        }

        VulkanLow.vkGetDeviceQueue(_device, (uint)graphicsFamily, 0, out _graphicsQueue);
        VulkanLow.vkGetDeviceQueue(_device, (uint)presentFamily, 0, out _presentQueue);
        _graphicsFamily = (uint)graphicsFamily;

        Log.Information("logical device created");
    }

    public void Dispose() {
        if (_commandPool != NULL) {
            VulkanLow.vkDestroyCommandPool(_device, _commandPool, NULL);
            _commandPool = NULL;
        }

        if (_device == NULL) return;
        VulkanLow.vkDeviceWaitIdle(_device);
        VulkanLow.vkDestroyDevice(_device, NULL);
        _device = NULL;
    }
}