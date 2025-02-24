﻿using System.Reflection;
using System.Runtime.InteropServices;

namespace Bale.Native.Core;

public static class NativeResolver {
    private static readonly Dictionary<string, Dictionary<OSPlatform, string>> LibraryMappings = new() {
        {
            "glfw3", new Dictionary<OSPlatform, string> {
                { OSPlatform.Windows, "glfw3.dll" },
                { OSPlatform.Linux, "libglfw.so" },
                { OSPlatform.OSX, "libglfw.dylib" }
            }
        }, {
            "vulkan-1", new Dictionary<OSPlatform, string> {
                { OSPlatform.Windows, "vulkan-1.dll" },
                // { OSPlatform.Linux, "libglfw.so" },
                // { OSPlatform.OSX, "libglfw.dylib" }
            }
        }
    };

    static NativeResolver() {
        NativeLibrary.SetDllImportResolver(typeof(NativeResolver).Assembly, ResolveLibrary);
    }

    private static IntPtr ResolveLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath) {
        if (!LibraryMappings.TryGetValue(libraryName, out var mappings)) return IntPtr.Zero;
        if (mappings == null) {
            throw new PlatformNotSupportedException("Mappings not found for the platform: " + libraryName);
        }

        foreach (var (platform, libraryPath) in mappings) {
            if (RuntimeInformation.IsOSPlatform(platform)) {
                return NativeLibrary.Load(libraryPath, assembly, searchPath);
            }
        }

        return IntPtr.Zero;
    }
}