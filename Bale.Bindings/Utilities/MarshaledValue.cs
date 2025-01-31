using System.Runtime.InteropServices;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Utilities;

public sealed class MarshaledValue<T> : IDisposable where T : unmanaged {
    private IntPtr _ptr;

    public MarshaledValue(T value) {
        unsafe {
            _ptr = Marshal.AllocHGlobal(sizeof(T));
            *(T*)_ptr = value;
        }
    }

    public static implicit operator IntPtr(MarshaledValue<T> mv) => mv._ptr;
    
    public void Dispose() {
        if (_ptr == NULL) return;
        
        Marshal.FreeHGlobal(_ptr);
        _ptr = NULL;
    }
}