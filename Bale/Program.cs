using Serilog;

using Bale.Application;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

try {
    using var app = new VulkanApp();
    app.Run();
} catch (Exception e) {
    Log.Fatal(e, "App crashed");
} finally {
    Log.CloseAndFlush();
}