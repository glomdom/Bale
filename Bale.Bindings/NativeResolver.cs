using System.Reflection;
using System.Runtime.InteropServices;

namespace Bale.Bindings;

public static class NativeResolver {
    private static readonly Dictionary<string, Dictionary<OSPlatform, string>> LibraryMappings = new() {
        {
            "glfw3", new() {
                { OSPlatform.Windows, "glfw3.dll" },
                { OSPlatform.Linux, "libglfw.so" },
                { OSPlatform.OSX, "libglfw.dylib" }
            }
        }
    };

    static NativeResolver() {
        NativeLibrary.SetDllImportResolver(typeof(NativeResolver).Assembly, ResolveLibrary);
    }

    private static IntPtr ResolveLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchpath) {
        if (!LibraryMappings.TryGetValue(libraryName, out var mappings)) return IntPtr.Zero;

        foreach (var (platform, libraryPath) in mappings) {
            if (RuntimeInformation.IsOSPlatform(platform)) {
                return NativeLibrary.Load(libraryPath, assembly, searchpath);
            }
        }

        return IntPtr.Zero;
    }
}