/// <summary>
/// Window Messages
/// </summary>
public static class WM
{
    public const int SETCURSOR = 0x0020;
    public const int WINDOWPOSCHANGING = 0x0046;
    public const int SYSCOMMAND = 0x112;
    public const int MOUSEMOVE = 0x0200;
}

/// <summary>
/// For WM_SYSCOMMAND
/// </summary>
public static class SC
{
    public const int MINIMIZE = 0xf020;
    public const int RESTORE = 0xf120;
}

/// <summary>
/// HookType: Valid hook types for SetWindowsHookEx
/// </summary>
public static class WH
{
    public const int KEYBOARD = 2;
}
