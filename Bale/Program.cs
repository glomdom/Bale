using Bale.Bindings.Vulkan;

try {
    using var app = new VulkanApp("Bale", 600, 400);
    app.Run();
} catch (Exception e) {
    Console.WriteLine($"Error occured while running app: {e.Message}");
}