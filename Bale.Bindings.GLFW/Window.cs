using Serilog;

using static Bale.Bindings.Common;
using static Bale.Bindings.Native.GLFWLow;

namespace Bale.Bindings.Vulkan;

public sealed class Window : IDisposable {
    private IntPtr _window;

    public Window(int width, int height, string title) {
        if (!glfwInit()) {
            throw new Exception("Failed to initialize GLFW");
        }
        
        glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);
        glfwWindowHint(GLFW_RESIZEABLE, GLFW_FALSE);

        _window = glfwCreateWindow(width, height, title, NULL, NULL);
        if (_window == NULL) {
            throw new Exception("Failed to create GLFW window");
        }
        
        Log.Information("created GLFW window");
    }

    public IntPtr Handle => _window;
    public bool ShouldClose => glfwWindowShouldClose(_window);

    public void PollEvents() => glfwPollEvents();
    
    public void Dispose() {
        if (_window == NULL) return;
        glfwDestroyWindow(_window);
        _window = NULL;
        
        glfwTerminate();
    }
}