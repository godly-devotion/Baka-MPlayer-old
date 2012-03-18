/****************************
* Settings (by Joshua Park) *
* updated 3/8/2012          *
****************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Windows.Forms;

public class Setting
{
    public string Name;
    public object Value;

    public Setting(string name, object value)
    {
        this.Name = name;
        this.Value = value;
    }
}

public class Settings
{
    /// <summary>
    /// NOTE : SET THE DEFAULT VALUES BEFORE HAND HERE!
    /// </summary>
    private void defaultSettings()
    {
        settings.Clear();
        settings.Add(new Setting("LastFile", ""));
        settings.Add(new Setting("ShowIcon", true));
        settings.Add(new Setting("Volume", 50));
        settings.Add(new Setting("ShowTimeRemaining", true));
        settings.Add(new Setting("MinimizeToTray", false));
        settings.Add(new Setting("HidePopup", false));
    }

    #region Global Objects

    // Array of Settings
    private readonly List<Setting> settings = new List<Setting>();

    // Appends to the end of the file to create the xml config file name.
    private const string xmlExtention = ".xml";
    private int ExceptionRetries;

    // used datasets to do the writing of the information.
    DataSet configDataSet;
    DataTable configDataTable;

    #endregion

    #region Functions

    public Settings()
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

        foreach (Setting s in settings)
            configDataTable.Columns.Add(s.Name, s.Value.GetType());

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
            SaveConfig();
        else
        {
            try
            {
                defaultSettings();
                configDataSet = new DataSet();
                configDataSet.ReadXml(AppPath + xmlExtention);
                DataRow r = configDataSet.Tables[0].Rows[0];

                for (int i = 0; i < settings.Count; i++)
                    settings[i].Value = r[i];

                configDataSet.Dispose();
            }
            catch (Exception)
            {
                if (ExceptionRetries++ < 1) // Don't try more than once before throwing.
                    SaveConfig(); // Likely, there was a new config field added so call the write.
                else
                    throw;
            }
        }
    }

    /// <summary>
    /// Gets setting with string value
    /// </summary>
    public string GetStringValue(string name)
    {
        return (string)settings.Find(item => item.Name.Equals(name)).Value;
    }

    /// <summary>
    /// Gets setting with int value
    /// </summary>
    public int GetIntValue(string name)
    {
        return Convert.ToInt32(settings.Find(item => item.Name.Equals(name)).Value);
    }

    /// <summary>
    /// Gets setting with boolean value
    /// </summary>
    public bool GetBoolValue(string name)
    {
        return Convert.ToBoolean(settings.Find(item => item.Name.Equals(name)).Value);
    }

    /// <summary>
    /// Sets setting value
    /// </summary>
    public void SetConfig(object value, string name)
    {
        settings.Find(item => item.Name.Equals(name)).Value = Convert.ChangeType(value, value.GetType());
    }

    /// <summary>
    /// Saves the settings to file.
    /// </summary>
    public void SaveConfig()
    {
        configDataSet = new DataSet();
        BuildSchema();
        DataRow r = configDataSet.Tables[0].NewRow();

        foreach (Setting item in settings)
            r[item.Name] = Convert.ChangeType(item.Value, item.Value.GetType());

        configDataSet.Tables["ConfigDataTable"].Rows.Add(r);
        configDataSet.WriteXml(AppPath + xmlExtention);

        // read new config
        readConfig();

        // clean up
        configDataTable.Dispose();
        configDataSet.Dispose();
    }

    #endregion
}