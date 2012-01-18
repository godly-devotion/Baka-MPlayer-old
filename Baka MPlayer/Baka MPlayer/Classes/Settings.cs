// Settings Class
// by Joshua Park
// ~ updated 8/2/2011

using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Windows.Forms;

// Note 1: remember to change the namespace according to the project name!
namespace Baka_MPlayer.Classes
{
    public class SettingsClass
    {
        /// <summary>
        /// NOTE 2: SET THE DEFAULT VALUES BEFORE HAND HERE!
        /// </summary>
        private void defaultSettings()
        {
            // Names: settings[0].Add(/* Setting Name */);
            settings[0] = new ArrayList
            {
                "LastFile",
                "ShowIcon",
                "Volume",
                "ShowTimeRemaining",
                "MinimizeToTray",
                "HidePopup"
            };

            // Values: settings[1].Add(/* Setting Value */);
            settings[1] = new ArrayList
            {
                "",
                true,
                50,
                true,
                false,
                false
            };
        }

        #region Global Objects
        // 2D array of ArrayList of settings
        //   string   object
        // [   0   ][    1   ]
        // [ names ][ values ]
        private ArrayList[] settings = new ArrayList[2];

        // Appends to the end of the file to create the xml config file name.
        private const string xmlExtention = ".xml";
        private int ExceptionRetries;

        // used datasets to do the writing of the information.
        DataSet configDataSet;
        DataTable configDataTable;
        #endregion

        #region Functions
        public SettingsClass()
        {
            defaultSettings();
            if (!File.Exists(AppPath + xmlExtention))
            {
                // config file does not exist so create one
                BuildSchema();
            }
            readConfig();
        }

        /// <summary>
        /// Gets the current applications path.
        /// </summary>
        private static string AppPath
        {
            get { return Application.ExecutablePath; }
        }

        /// <summary>
        /// In the event that the file is missing, we need to recreate the 
        /// schema of the xml preparatory to writing the data.
        /// </summary>
        private void BuildSchema()
        {
            configDataTable = new DataTable("ConfigDataTable");
            configDataSet = new DataSet();

            /*IEnumerator enmName = settings[0].GetEnumerator();
            while (enmName.MoveNext())
                configDataTable.Columns.Add((string)enmName.Current, enmName.Current.GetType());*/
            for (int i = 0; i < settings[0].Count; i++)
                configDataTable.Columns.Add((string)settings[0][i], settings[1][i].GetType());

            configDataSet.Tables.Add(this.configDataTable);
        }

        /// <summary>
        /// Reads the values from the config file.
        /// If this routine finds the file missing, it re-creates it.
        /// and then returns default values.
        /// </summary>
        private void readConfig()
        {
            // If config doesn't exist, create it.
            if (!File.Exists(AppPath + xmlExtention))
                saveConfig();
            else
            {
                try
                {
                    // clear all settings
                    settings[1].Clear();
                    configDataSet = new DataSet();
                    configDataSet.ReadXml(AppPath + xmlExtention);
                    DataRow r = configDataSet.Tables[0].Rows[0];

                    for (int i = 0; i < settings[0].Count; i++)
                        settings[1].Add(r[i]);

                    configDataSet.Dispose();
                }
                catch (Exception)
                {
                    if (ExceptionRetries++ < 1) // Don't try more than once before throwing.
                        saveConfig(); // Likely, there was a new config field added so call the write.
                    else
                        throw;
                }
            }
        }

        /// <summary>
        /// Gets setting with string value
        /// </summary>
        public string getStringValue(string name)
        {
            int index = settings[0].IndexOf(name);
            if (index.Equals(-1))
                throw new Exception(string.Format("\"{0}\" is not a valid setting!", name));
            return settings[1][index].ToString();
        }

        /// <summary>
        /// Gets setting with int value
        /// </summary>
        public int getIntValue(string name)
        {
            int index = settings[0].IndexOf(name);
            if (index.Equals(-1))
                throw new Exception(string.Format("\"{0}\" is not a valid setting!", name));
            return Convert.ToInt32(settings[1][index]);
        }

        /// <summary>
        /// Gets setting with boolean value
        /// </summary>
        public bool getBoolValue(string name)
        {
            int index = settings[0].IndexOf(name);
            if (index.Equals(-1))
                throw new Exception(string.Format("\"{0}\" is not a valid setting!", name));
            return Convert.ToBoolean(settings[1][index]);
        }

        /// <summary>
        /// Sets setting value
        /// </summary>
        public void setConfig(object value, string name)
        {
            settings[1][settings[0].IndexOf(name)] = Convert.ChangeType(value, value.GetType());
        }

        /// <summary>
        /// Saves the settings to file.
        /// </summary>
        public void saveConfig()
        {
            configDataSet = new DataSet();
            BuildSchema();
            DataRow r = configDataSet.Tables[0].NewRow();

            for (int i = 0; i < settings[0].Count; i++)
                r[(string)settings[0][i]] = Convert.ChangeType(settings[1][i], settings[1][i].GetType());

            configDataSet.Tables["ConfigDataTable"].Rows.Add(r);
            configDataSet.WriteXml(AppPath + xmlExtention);

            // read new config
            readConfig();

            // clean up
            r = null;
            configDataTable.Dispose();
            configDataSet.Dispose();
        }
        #endregion
    }
}
