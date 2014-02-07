/*
 * KeyCodeEventArgs.cs
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System;
using System.Windows.Forms;

namespace Baka_MPlayer.GlobalKeyHook
{
    public class KeyCodeEventArgs : EventArgs
    {
        public Keys KeyCode { get; private set; }

        public KeyCodeEventArgs(Keys keyCode)
        {
            KeyCode = keyCode;
        }
    }
}
