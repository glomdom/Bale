using System.Runtime.InteropServices;
using Bale.Bindings.Native;
using Bale.Interop.Utilities;
using Microsoft.Extensions.Logging;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanInstance : IDisposable {
    public IntPtr Handle => _handle;
    private IntPtr _handle;

    private readonly ILogger<VulkanInstance> _logger;

    public VulkanInstance(string appName, Version version, ILogger<VulkanInstance> logger) {
        _logger = logger;

        string[] validationLayers = ["VK_LAYER_KHRONOS_validation"];
        var extensions = GetGlfwRequiredExtensions();
        using var pAppName = new MarshaledString(appName);

        var appInfo = new VkApplicationInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO,
            pApplicationName = pAppName,
            applicationVersion = Vk.MakeApiVersion(0, (uint)version.Major, (uint)version.Minor, (uint)version.Build),
            apiVersion = Vk.MakeApiVersion(0, 1, 4, 0)
        };

        using var pAppInfo = new MarshaledStruct<VkApplicationInfo>(appInfo);
        using var pExtensions = new MarshaledStringArray(extensions);
        
        #if DEBUG
        var allExtensions = extensions.ToList();
        if (!allExtensions.Contains("VK_EXT_debug_utils")) {
            allExtensions.Add("VK_EXT_debug_utils");
        }

        using var pAllExtensions = new MarshaledStringArray(allExtensions.ToArray());
        #else
        using var pAllExtensions = pExtensions;
        #endif

        var createInfo = new VkInstanceCreateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
            pApplicationInfo = pAppInfo,
            enabledExtensionCount = (uint)extensions.Length,
            ppEnabledExtensionNames = pExtensions,
            #if DEBUG
            enabledLayerCount = (uint)validationLayers.Length,
            ppEnabledLayerNames = new MarshaledStringArray(validationLayers),
            #else
            enabledLayerCount = 0,
            ppEnabledLayerNames = IntPtr.Zero,
            #endif
        };

        var result = VulkanLow.vkCreateInstance(ref createInfo, NULL, out _handle);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create Vulkan instance: {result}");
        }
        
        _logger.LogInformation("created Vulkan instance");
    }

    private string[] GetGlfwRequiredExtensions() {
        var ptr = GLFWLow.glfwGetRequiredInstanceExtensions(out var count);
        if (ptr == IntPtr.Zero) {
            throw new Exception("GLFW failed to get required Vulkan extensions.");
        }

        var extensions = new string[count];
        for (var i = 0; i < count; i++) {
            var extensionPtr = Marshal.ReadIntPtr(ptr, i * IntPtr.Size);
            extensions[i] = Marshal.PtrToStringAnsi(extensionPtr)!;
        }

        return extensions;
    }

    public void Dispose() {
        if (_handle == NULL) return;

        VulkanLow.vkDestroyInstance(_handle, NULL);
        _handle = NULL;
    }
}