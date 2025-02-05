using System.Runtime.CompilerServices;

namespace Bale.Interop.Utilities;

public sealed class MarshaledStructArray<T> : IDisposable where T : unmanaged {
    private readonly SafeHGlobalHandle? _handle;
    private bool _disposed;

    public MarshaledStructArray(T[] items) {
        ArgumentNullException.ThrowIfNull(items);

        var length = items.Length;
        var elementSize = Unsafe.SizeOf<T>();
        var totalBytes = checked(elementSize * length);
        _handle = new SafeHGlobalHandle(totalBytes);

        unsafe {
            fixed (T* sourcePtr = items) {
                Buffer.MemoryCopy(sourcePtr, (void*)_handle.DangerousGetHandle(), totalBytes, totalBytes);
            }
        }
    }

    ~MarshaledStructArray() => Dispose(false);

    public static implicit operator IntPtr(MarshaledStructArray<T> arr) => arr._handle?.DangerousGetHandle() ?? IntPtr.Zero;

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing) {
        if (_disposed) return;

        if (disposing) {
            _handle?.Dispose();
        }

        _disposed = true;
    }
}