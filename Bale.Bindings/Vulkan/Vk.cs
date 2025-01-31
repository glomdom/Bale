namespace Bale.Bindings.Vulkan;

public static class Vk {
    public static uint MakeApiVersion(uint variant, uint major, uint minor, uint patch) => (variant << 29) | (major << 22) | (minor << 12) | patch;
}