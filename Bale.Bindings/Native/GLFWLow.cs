using System.Runtime.InteropServices;
using Bale.Bindings.Native.Vulkan;

namespace Bale.Bindings.Native;

public static partial class GLFWLow {
    static GLFWLow() {
        _ = typeof(NativeResolver);
    }

    [LibraryImport("glfw3")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool glfwInit();

    [LibraryImport("glfw3")]
    public static partial void glfwTerminate();

    [LibraryImport("glfw3")]
    public static partial IntPtr glfwCreateWindow(int width, int height, [MarshalAs(UnmanagedType.LPStr)] string title, IntPtr monitor, IntPtr share);

    [LibraryImport("glfw3")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool glfwWindowShouldClose(IntPtr window);

    [LibraryImport("glfw3")]
    public static partial void glfwPollEvents();

    [LibraryImport("glfw3")]
    public static partial void glfwWindowHint(int hint, int value);
    
    [LibraryImport("glfw3")]
    public static partial void glfwMakeContextCurrent(IntPtr window);

    [LibraryImport("glfw3")]
    public static partial void glfwSwapBuffers(IntPtr window);

    [LibraryImport("glfw3")]
    public static partial VkResult glfwCreateWindowSurface(IntPtr instance, IntPtr window, IntPtr allocator, out IntPtr surface);

    [LibraryImport("glfw3")]
    public static partial IntPtr glfwGetRequiredInstanceExtensions(out uint count);

    #region CONSTANTS

    public const int GLFW_TRUE = 1;
    public const int GLFW_FALSE = 0;

    public const int GLFW_RESIZEABLE = 0x00020003;

    public const int GLFW_CONTEXT_VERSION_MAJOR = 0x00022002;
    public const int GLFW_CONTEXT_VERSION_MINOR = 0x00022003;
    public const int GLFW_OPENGL_PROFILE = 0x00022008;
    public const int GLFW_OPENGL_CORE_PROFILE = 0x00032001;

    public const int GLFW_CLIENT_API = 0x00022001;
    public const int GLFW_NO_API = 0;

    #endregion
}