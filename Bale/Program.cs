using Bale.Bindings.Native.Vulkan;
using Bale.Bindings.Vulkan;
using static Bale.Bindings.Native.GLFW;
using static Bale.Bindings.Common;

if (!glfwInit()) {
    Console.WriteLine("failed to initialize glfw");
}

glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);
glfwWindowHint(GLFW_RESIZEABLE, GLFW_FALSE);

var window = glfwCreateWindow(600, 400, "i hate C#", NULL, NULL);
if (window == NULL) {
    Console.WriteLine("failed to create window");
    glfwTerminate();
}

using var vulkanInstance = new VulkanInstance("Bale", new Version(1, 0, 0));
var result = glfwCreateWindowSurface(vulkanInstance.Handle, window, NULL, out var surface);

if (result != VkResult.VK_SUCCESS) {
    throw new Exception($"failed to create surface {result}");
}

Console.WriteLine($"created surface with result {result}");

while (!glfwWindowShouldClose(window)) {
    glfwPollEvents();
}

glfwTerminate();