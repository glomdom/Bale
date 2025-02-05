using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try {
    using var app = new VulkanApp();
    app.Run();
} catch (Exception e) {
    Log.Fatal(e, "App crashed");
} finally {
    Log.CloseAndFlush();
}