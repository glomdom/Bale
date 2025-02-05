using System.Runtime.CompilerServices;

namespace Bale.Interop.Utilities;

public sealed class MarshaledStruct<T> : IDisposable where T : unmanaged {
    private SafeHGlobalHandle? _handle;

    public MarshaledStruct(T value) {
        var size = Unsafe.SizeOf<T>();
        _handle = new SafeHGlobalHandle(size);

        unsafe {
            *(T*)_handle.DangerousGetHandle().ToPointer() = value;
        }
    }

    ~MarshaledStruct() => Dispose(false);

    public static implicit operator IntPtr(MarshaledStruct<T> ms) => ms._handle?.DangerousGetHandle() ?? IntPtr.Zero;

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing) {
        if (_handle is null || _handle.IsClosed) return;

        _handle.Dispose();
        _handle = null;
    }
}