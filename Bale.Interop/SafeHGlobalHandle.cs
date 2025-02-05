using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Bale.Interop;

public sealed class SafeHGlobalHandle : SafeHandleZeroOrMinusOneIsInvalid {
    public SafeHGlobalHandle(int size) : base(true) {
        ArgumentOutOfRangeException.ThrowIfNegative(size);
        SetHandle(Marshal.AllocHGlobal(size));
    }

    protected override bool ReleaseHandle() {
        if (!IsInvalid) Marshal.FreeHGlobal(handle);

        return true;
    }
}