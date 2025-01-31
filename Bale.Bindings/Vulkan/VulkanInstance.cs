using System.Runtime.InteropServices;
using Bale.Bindings.Native;
using Bale.Bindings.Native.Vulkan;
using Bale.Bindings.Utilities;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanInstance : IDisposable {
    public IntPtr Handle;

    public VulkanInstance(string appName, Version version) {
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

        var createInfo = new VkInstanceCreateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
            pApplicationInfo = pAppInfo,
            enabledExtensionCount = (uint)extensions.Length,
            ppEnabledExtensionNames = pExtensions
        };

        var result = VulkanLow.vkCreateInstance(ref createInfo, NULL, out Handle);

        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create Vulkan instance: {result}");
        }
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
        if (Handle == NULL) return;

        VulkanLow.vkDestroyInstance(Handle, NULL);
        Handle = NULL; 
    }
}