using System.Runtime.InteropServices;
using Bale.Bindings.Native;
using Bale.Bindings.Native.Vulkan;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanInstance : IDisposable {
    public IntPtr Handle;

    public VulkanInstance(string appName, Version version) {
        var extensions = GetGlfwRequiredExtensions();
        var pAppName = Marshal.StringToHGlobalAnsi(appName);

        var appInfo = new VkApplicationInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO,
            pApplicationName = pAppName,
            applicationVersion = Vk.MakeApiVersion(0, (uint)version.Major, (uint)version.Minor, (uint)version.Build),
            apiVersion = Vk.MakeApiVersion(0, 1, 4, 0)
        };

        var createInfo = new VkInstanceCreateInfo {
            sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
            pApplicationInfo = Marshal.AllocHGlobal(Marshal.SizeOf<VkApplicationInfo>()),
            enabledExtensionCount = (uint)extensions.Length,
            ppEnabledExtensionNames = MarshalExtensions(extensions)
        };

        Marshal.StructureToPtr(appInfo, createInfo.pApplicationInfo, false);

        var result = VulkanLow.vkCreateInstance(ref createInfo, NULL, out Handle);

        Marshal.FreeHGlobal(createInfo.pApplicationInfo);
        Marshal.FreeHGlobal(pAppName);
        FreeExtensions(createInfo.ppEnabledExtensionNames, extensions.Length);
        Console.WriteLine("freed marshals");

        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create Vulkan instance: {result}");
        }
    }

    private string[] GetGlfwRequiredExtensions() {
        var ptr = GLFW.glfwGetRequiredInstanceExtensions(out var count);
        if (ptr == IntPtr.Zero)
            throw new Exception("GLFW failed to get required Vulkan extensions.");

        var extensions = new string[count];
        for (var i = 0; i < count; i++) {
            var extensionPtr = Marshal.ReadIntPtr(ptr, i * IntPtr.Size);
            extensions[i] = Marshal.PtrToStringAnsi(extensionPtr)!;
        }
        
        Console.WriteLine("got extension names");

        return extensions;
    }

    private IntPtr MarshalExtensions(string[] extensions) {
        var ptrArray = Marshal.AllocHGlobal(extensions.Length * IntPtr.Size);
        for (var i = 0; i < extensions.Length; i++) {
            var extensionPtr = Marshal.StringToHGlobalAnsi(extensions[i]);
            Marshal.WriteIntPtr(ptrArray, i * IntPtr.Size, extensionPtr);
        }
        
        Console.WriteLine("marshalled extensions");

        return ptrArray;
    }

    private void FreeExtensions(IntPtr ptr, int count) {
        for (var i = 0; i < count; i++) {
            var extensionPtr = Marshal.ReadIntPtr(ptr, i * IntPtr.Size);
            Marshal.FreeHGlobal(extensionPtr);
        }

        Marshal.FreeHGlobal(ptr);
    }

    public void Dispose() {
        if (Handle == NULL) return;

        VulkanLow.vkDestroyInstance(Handle, NULL);
        Handle = NULL;
    }
}