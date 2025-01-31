using Bale.Bindings.Native.Vulkan;
using static Bale.Bindings.Native.GLFW;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Vulkan;

public sealed class VulkanApp : IDisposable {
    private IntPtr _window;
    private readonly VulkanInstance _vulkanInstance;
    private IntPtr _surface;

    public VulkanApp(string appName, int width, int height) {
        InitGLFW();
        CreateWindow(width, height, appName);

        _vulkanInstance = new VulkanInstance(appName, new Version(1, 0, 0));
        CreateSurface();
    }

    private void InitGLFW() {
        if (!glfwInit()) {
            throw new Exception("Failed to initialize GLFW");
        }

        glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);
        glfwWindowHint(GLFW_RESIZEABLE, GLFW_FALSE);
    }

    private void CreateWindow(int width, int height, string title) {
        _window = glfwCreateWindow(width, height, title, NULL, NULL);
        if (_window == NULL) {
            throw new Exception("Failed to create GLFW window");
        }
    }

    private void CreateSurface() {
        var result = glfwCreateWindowSurface(_vulkanInstance.Handle, _window, NULL, out _surface);
        if (result != VkResult.VK_SUCCESS) {
            throw new Exception($"Failed to create Vulkan surface: {result}");
        }

        Console.WriteLine($"Created Vulkan surface with result: {result}");
    }

    public void Run() {
        while (!glfwWindowShouldClose(_window)) {
            glfwPollEvents();
        }
    }

    public void Dispose() {
        glfwTerminate();
        _vulkanInstance?.Dispose();
    }
}