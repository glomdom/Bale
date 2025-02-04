using Bale.Bindings.Vulkan;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services => {
        services.AddLogging(builder => {
            builder.ClearProviders();
            builder.AddSerilog();
        });
        
        services.AddSingleton<Window>(provider => {
            var logger = provider.GetRequiredService<ILogger<Window>>();
            return new Window(800, 600, "Bale", logger);
        });

        services.AddSingleton<VulkanInstance>(provider => {
            var logger = provider.GetRequiredService<ILogger<VulkanInstance>>();
            return new VulkanInstance("Bale", new Version(0, 0, 1), logger);
        });

        services.AddSingleton<VulkanSurfaceManager>();
        services.AddSingleton<VulkanPhysicalDeviceSelector>();
        services.AddSingleton<VulkanLogicalDeviceManager>();
        services.AddSingleton<VulkanSwapchainManager>();
        
        services.AddSingleton<VulkanApp>();
    })
    .Build();

try {
    using var scope = host.Services.CreateScope();
    using var app = scope.ServiceProvider.GetRequiredService<VulkanApp>();
    app.Run();
} catch (Exception e) {
    Log.Fatal(e, "App crashed");
} finally {
    Log.CloseAndFlush();
}