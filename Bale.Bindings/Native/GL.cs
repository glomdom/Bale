using System.Runtime.InteropServices;

namespace Bale.Bindings;

public static partial class GL {
    public delegate void glClearColorDelegate(float red, float green, float blue, float alpha);
    public delegate void glClearDelegate(uint mask);
    
    static GL() {
        _ = typeof(NativeResolver);
        
        if (!gladLoadGL()) {
            throw new Exception("glad not loaded");
        }
        
        glClearColor = Marshal.GetDelegateForFunctionPointer<glClearColorDelegate>(
            NativeLibrary.GetExport(NativeLibrary.Load("opengl32"), "glClearColor")
        );

        glClear = Marshal.GetDelegateForFunctionPointer<glClearDelegate>(
            NativeLibrary.GetExport(NativeLibrary.Load("opengl32"), "glClear")
        );
    }
    
    [LibraryImport("glad")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool gladLoadGL();

    public static glClearColorDelegate glClearColor;
    public static glClearDelegate glClear;
}