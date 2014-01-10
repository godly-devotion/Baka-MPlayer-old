/*
 * ID_Info
 * 
 * Copyright (c) 2014, Joshua Park
 */

namespace MPlayer.Info
{
    /// <summary>
    /// Stores ID info
    /// </summary>
    public class ID_Info
    {
        public string ID;
        public string Value;

        public ID_Info() { }
        public ID_Info(string ID, string value)
        {
            this.ID = ID;
            this.Value = value;
        }
    }
}
