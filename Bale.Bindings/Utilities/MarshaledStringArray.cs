using System.Runtime.InteropServices;
using static Bale.Bindings.Common;

namespace Bale.Bindings.Utilities;

public sealed class MarshaledStringArray : IDisposable {
    private IntPtr _ptrArray;
    private readonly IntPtr[] _stringPtrs;

    public MarshaledStringArray(string[] strings) {
        _stringPtrs = new IntPtr[strings.Length];
        _ptrArray = Marshal.AllocHGlobal(strings.Length * IntPtr.Size);

        for (var i = 0; i < strings.Length; i++) {
            _stringPtrs[i] = Marshal.StringToHGlobalAnsi(strings[i]);
            Marshal.WriteIntPtr(_ptrArray, i * IntPtr.Size, _stringPtrs[i]);
        }
    }

    public static implicit operator IntPtr(MarshaledStringArray msa) => msa._ptrArray;

    public void Dispose() {
        foreach (var ptr in _stringPtrs) {
            Marshal.FreeHGlobal(ptr);
        }

        if (_ptrArray == NULL) return;

        Marshal.FreeHGlobal(_ptrArray);
        _ptrArray = NULL;
    }
}