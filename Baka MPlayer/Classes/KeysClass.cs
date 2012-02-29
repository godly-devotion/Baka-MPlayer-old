using System.Windows.Forms;

public static class KeysClass
{
    public static Keys GetNumKey(int numKey)
    {
        switch (numKey)
        {
            case 0:
                return Keys.D0;
            case 1:
                return Keys.D1;
            case 2:
                return Keys.D2;
            case 3:
                return Keys.D3;
            case 4:
                return Keys.D4;
            case 5:
                return Keys.D5;
            case 6:
                return Keys.D6;
            case 7:
                return Keys.D7;
            case 8:
                return Keys.D8;
            case 9:
                return Keys.D9;
            default:
                return Keys.Escape;
        }
    }
}