using System.Runtime.InteropServices;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Utilities;

public sealed class MarshaledStructArray<T> : IDisposable where T : struct {
    private IntPtr _ptr;
    private readonly int _elementSize;
    private readonly int _length;

    public MarshaledStructArray(T[] items) {
        _length = items.Length;
        _elementSize = Marshal.SizeOf<T>();
        _ptr = Marshal.AllocHGlobal(_length * _elementSize);

        for (var i = 0; i < _length; i++) {
            Marshal.StructureToPtr(items[i], _ptr + i * _elementSize, false);
        }
    }

    public static implicit operator IntPtr(MarshaledStructArray<T> arr) => arr._ptr;

    public void Dispose() {
        if (_ptr == NULL) return;

        for (var i = 0; i < _length; i++) {
            Marshal.DestroyStructure<T>(_ptr + i * _elementSize);
        }
        
        Marshal.FreeHGlobal(_ptr);
        _ptr = NULL;
    }
}