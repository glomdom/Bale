using System.Runtime.InteropServices;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Utilities;

public sealed class MarshaledString : IDisposable {
    private IntPtr _ptr;

    public MarshaledString(string str) {
        _ptr = Marshal.StringToHGlobalAnsi(str);
    }

    public static implicit operator IntPtr(MarshaledString ms) => ms._ptr;
    
    public void Dispose() {
        if (_ptr == NULL) return;

        Marshal.FreeHGlobal(_ptr);
        _ptr = NULL;
    }
}