using System.Runtime.InteropServices;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Utilities;

public sealed class MarshaledStruct<T> : IDisposable where T : struct {
    private IntPtr _ptr;

    public MarshaledStruct(T value) {
        _ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
        Marshal.StructureToPtr(value, _ptr, false);
    }

    public static implicit operator IntPtr(MarshaledStruct<T> ms) => ms._ptr;

    public void Dispose() {
        if (_ptr == NULL) return;

        Marshal.FreeHGlobal(_ptr);
        _ptr = NULL;
    }
}