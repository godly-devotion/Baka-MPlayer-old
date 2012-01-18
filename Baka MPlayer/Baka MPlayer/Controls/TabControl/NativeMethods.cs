/*
 * This code is provided under the Code Project Open Licence (CPOL)
 * See http://www.codeproject.com/info/cpol10.aspx for details
*/

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    /// <summary>
    /// Description of NativeMethods.
    /// </summary>
    //[SecurityPermission(SecurityAction.Assert, Flags=SecurityPermissionFlag.UnmanagedCode)]
    internal sealed class NativeMethods
    {
        private NativeMethods()
        {
        }

        #region Windows Constants

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public const int WM_GETTABRECT =
            0x130a;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public const int WS_EX_TRANSPARENT
            = 0x20;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public const int WM_SETFONT = 0x30;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public const int WM_FONTCHANGE =
            0x1d;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public const int WM_HSCROLL =
            0x114;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public const int TCM_HITTEST =
            0x130D;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public const int WM_PAINT = 0xf;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public const int WS_EX_LAYOUTRTL =
            0x400000;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public const int
            WS_EX_NOINHERITLAYOUT = 0x100000;

        #endregion

        #region Content Alignment

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public static readonly
            ContentAlignment AnyRightAlign = ContentAlignment.BottomRight | ContentAlignment.MiddleRight |
                                             ContentAlignment.TopRight;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public static readonly
            ContentAlignment AnyLeftAlign = ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft |
                                            ContentAlignment.TopLeft;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public static readonly
            ContentAlignment AnyTopAlign = ContentAlignment.TopRight | ContentAlignment.TopCenter |
                                           ContentAlignment.TopLeft;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public static readonly
            ContentAlignment AnyBottomAlign = ContentAlignment.BottomRight | ContentAlignment.BottomCenter |
                                              ContentAlignment.BottomLeft;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public static readonly
            ContentAlignment AnyMiddleAlign = ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter |
                                              ContentAlignment.MiddleLeft;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] public static readonly
            ContentAlignment AnyCenterAlign = ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter |
                                              ContentAlignment.TopCenter;

        #endregion

        #region User32.dll

//        [DllImport("user32.dll"), SecurityPermission(SecurityAction.Demand)]
//		public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        public static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            //	This Method replaces the User32 method SendMessage, but will only work for sending
            //	messages to Managed controls.
            Control control = Control.FromHandle(hWnd);
            if (control == null)
            {
                return IntPtr.Zero;
            }

            var message = new Message();
            message.HWnd = hWnd;
            message.LParam = lParam;
            message.WParam = wParam;
            message.Msg = msg;

            MethodInfo wproc = control.GetType().GetMethod("WndProc"
                                                           , BindingFlags.NonPublic
                                                             | BindingFlags.InvokeMethod
                                                             | BindingFlags.FlattenHierarchy
                                                             | BindingFlags.IgnoreCase
                                                             | BindingFlags.Instance);

            var args = new object[] {message};
            wproc.Invoke(control, args);

            return ((Message) args[0]).Result;
        }


//		[DllImport("user32.dll")]
//		public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);
//		
//		[DllImport("user32.dll")]
//		[return: MarshalAs(UnmanagedType.Bool)]
//		public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);
//

        #endregion

        #region Misc Functions

        public static int LoWord(IntPtr dWord)
        {
            return dWord.ToInt32() & 0xffff;
        }

        public static int HiWord(IntPtr dWord)
        {
            if ((dWord.ToInt32() & 0x80000000) == 0x80000000)
                return (dWord.ToInt32() >> 16);
            return (dWord.ToInt32() >> 16) & 0xffff;
        }

        [SuppressMessage("Microsoft.Security", "CA2106:SecureAsserts")]
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static IntPtr ToIntPtr(object structure)
        {
            IntPtr lparam = IntPtr.Zero;
            lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, lparam, false);
            return lparam;
        }

        #endregion

        #region Windows Structures and Enums

        [Flags]
        public enum TCHITTESTFLAGS
        {
            TCHT_NOWHERE = 1,
            TCHT_ONITEMICON = 2,
            TCHT_ONITEMLABEL = 4,
            TCHT_ONITEM = TCHT_ONITEMICON | TCHT_ONITEMLABEL
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct TCHITTESTINFO
        {
            public TCHITTESTINFO(Point location)
            {
                pt = location;
                flags = TCHITTESTFLAGS.TCHT_ONITEM;
            }

            public Point pt;
            public TCHITTESTFLAGS flags;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public int fErase;
            public RECT rcPaint;
            public int fRestore;
            public int fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(Rectangle r)
            {
                this.left = r.Left;
                this.top = r.Top;
                this.right = r.Right;
                this.bottom = r.Bottom;
            }

            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }

            public static RECT FromIntPtr(IntPtr ptr)
            {
                var rect = (RECT) Marshal.PtrToStructure(ptr, typeof (RECT));
                return rect;
            }

            public Size Size
            {
                get { return new Size(this.right - this.left, this.bottom - this.top); }
            }
        }

        #endregion
    }
}