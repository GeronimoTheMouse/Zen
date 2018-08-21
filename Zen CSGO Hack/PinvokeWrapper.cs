using System;
using System.Runtime.InteropServices;

namespace Zen_CSGO_Hack
{
    //A class that contains all function used from our library
    class PinvokeWrapper
    {
        [DllImport("CSMemLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadAddInt(IntPtr address, IntPtr processHandle);

        [DllImport("CSMemLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float ReadAddFloat(IntPtr address, IntPtr processHandle);

        [DllImport("CSMemLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteFloat(IntPtr address, IntPtr processHandle, float value);

        [DllImport("CSMemLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteBool(IntPtr address, IntPtr processHandle, bool value);

        [DllImport("CSMemLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTheKeyState(int keyValue);

        [DllImport("CSMemLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetWindowLng(IntPtr hWnd, int nIndex);

        [DllImport("CSMemLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetWindowLng(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("CSMemLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DoubleClick();

        [DllImport("CSMemLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MoveMouseTo(int x, int y);
    }
}
