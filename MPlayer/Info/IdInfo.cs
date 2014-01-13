/*
 * IdInfo
 * 
 * Copyright (c) 2014, Joshua Park
 */

namespace MPlayer.Info
{
    /// <summary>
    /// Stores ID info
    /// </summary>
    public class IdInfo
    {
        public string Id;
        public string Value;

        public IdInfo() { }
        public IdInfo(string id, string value)
        {
            this.Id = id;
            this.Value = value;
        }
    }
}