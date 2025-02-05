using System.Text;

namespace Bale.Bindings.Utilities;

public sealed class MarshaledString : IDisposable {
    private SafeHGlobalHandle? _handle;

    public MarshaledString(string str) {
        ArgumentNullException.ThrowIfNull(str);

        var bytes = Encoding.UTF8.GetBytes(str);
        var byteCount = bytes.Length + 1; // +1 for \0

        _handle = new SafeHGlobalHandle(byteCount);
        unsafe {
            var destination = new Span<byte>(_handle.DangerousGetHandle().ToPointer(), byteCount);
            bytes.CopyTo(destination);
            destination[bytes.Length] = 0; // \0
        }
    }

    ~MarshaledString() => Dispose(false);

    public static implicit operator IntPtr(MarshaledString ms) => ms._handle?.DangerousGetHandle() ?? IntPtr.Zero;
    
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