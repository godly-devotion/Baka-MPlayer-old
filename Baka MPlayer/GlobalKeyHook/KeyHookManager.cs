/*
 * HookManager.cs
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Win32;

namespace Baka_MPlayer.GlobalKeyHook
{
    public class KeyHookManager : IDisposable
    {
        #region DLL Imports

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private readonly HookProc myCallbackDelegate;
        private static IntPtr hHook;

        [DllImport("user32.dll")]
        protected static extern IntPtr SetWindowsHookEx(int code, HookProc func, IntPtr hInstance, int threadID);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        public KeyHookManager()
        {
            // set keyboard hook
            this.myCallbackDelegate = this.callbackFunction_KeyboardHook;
            hHook = SetWindowsHookEx(WH.KEYBOARD, this.myCallbackDelegate, IntPtr.Zero, AppDomain.GetCurrentThreadId());
        }

        private int callbackFunction_KeyboardHook(int code, IntPtr wParam, IntPtr lParam)
        {
            // checks bit 30 of WM_KEYDOWN to see the previous key state
            bool isBitSet = (lParam.ToInt64() & (1 << 30)) == 0;

            if (code.Equals(3) && isBitSet)
            {
                OnKeyDown(new KeyCodeEventArgs((Keys)wParam.ToInt32()));
            }

            // return the value returned by CallNextHookEx
            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        public event EventHandler<KeyCodeEventArgs> KeyDownEvent;

        protected virtual void OnKeyDown(KeyCodeEventArgs e)
        {
            if (KeyDownEvent != null)
            {
                KeyDownEvent(this, e);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnhookWindowsHookEx(hHook);
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
