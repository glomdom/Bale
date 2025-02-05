using System.Runtime.InteropServices;
using System.Text;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Utilities;

public sealed class MarshaledStringArray : IDisposable {
    private readonly SafeHGlobalHandle?[] _stringHandles;
    private SafeHGlobalHandle? _ptrArray;

    public MarshaledStringArray(string?[] strings) {
        ArgumentNullException.ThrowIfNull(strings);

        var count = strings.Length;
        _stringHandles = new SafeHGlobalHandle?[count];
        _ptrArray = new SafeHGlobalHandle(count * IntPtr.Size);

        unsafe {
            var ptr = (IntPtr*)_ptrArray.DangerousGetHandle().ToPointer();
            for (var i = 0; i < count; i++) {
                var s = strings[i];

                if (s is null) {
                    _stringHandles[i] = null;
                    ptr[i] = IntPtr.Zero;
                } else {
                    var bytes = Encoding.ASCII.GetBytes(s);
                    var byteCount = bytes.Length + 1;
                    _stringHandles[i] = new SafeHGlobalHandle(byteCount);
                    
                    var destination = new Span<byte>(_stringHandles[i]!.DangerousGetHandle().ToPointer(), byteCount);
                    bytes.CopyTo(destination);
                    destination[bytes.Length] = 0;
                    ptr[i] = _stringHandles[i]!.DangerousGetHandle();
                }
            }
        }
    }

    ~MarshaledStringArray() => Dispose(false);

    public static implicit operator IntPtr(MarshaledStringArray msa) => msa._ptrArray?.DangerousGetHandle() ?? IntPtr.Zero;

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing) {
        foreach (var handle in _stringHandles) {
            handle?.Dispose();
        }
        
        _ptrArray?.Dispose();
        _ptrArray = null;
    }
}