using System.Runtime.CompilerServices;

namespace Bale.Interop.Utilities;

public sealed class MarshaledValue<T> : IDisposable where T : unmanaged {
    private SafeHGlobalHandle? _handle;

    public MarshaledValue(T value) {
        var size = Unsafe.SizeOf<T>();
        _handle = new SafeHGlobalHandle(size);
        
        unsafe {
            *(T*)_handle.DangerousGetHandle().ToPointer() = value;
        }
    }

    public static implicit operator IntPtr(MarshaledValue<T> mv) => mv._handle?.DangerousGetHandle() ?? IntPtr.Zero;

    ~MarshaledValue() => Dispose(false);

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