/*
 * Copyright (c) 2007, CodeChimp (from codeproject.com)
 */

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

public class PortableSettingsProvider : SettingsProvider
{
    // XML Root Node
    const string SETTINGSROOT = "Settings";
    
    public override void Initialize(string name, NameValueCollection col)
    {
        base.Initialize(this.ApplicationName, col);
    }

    public override string ApplicationName
    {
        get
        {
            if (Application.ProductName.Trim().Length > 0)
            {
                return Application.ProductName;
            }
            var fi = new FileInfo(Application.ExecutablePath);
            return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
        }
        set { }
    }

    public override string Name
    {
        get { return "PortableSettingsProvider"; }
    }
    public virtual string GetAppSettingsPath()
    {
        // used to determine where to store the settings
        var fi = new FileInfo(Application.ExecutablePath);
        return fi.DirectoryName;
    }

    public virtual string GetAppSettingsFilename()
    {
        // used to determine the filename to store the settings
        return ApplicationName + ".settings";
    }

    public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection propvals)
    {
        // iterate through the settings to be stored
        // only dirty settings are included in propvals, and only ones relevant to this provider
        foreach (SettingsPropertyValue propval in propvals)
        {
            SetValue(propval);
        }

        try
        {
            SettingsXML.Save(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
        }
        catch (Exception)
        {
            // ignore if cant save, device been ejected
        }
    }

    public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
    {
        // create new collection of values
        var values = new SettingsPropertyValueCollection();

        // iterate through the settings to be retrieved
        foreach (SettingsProperty setting in props)
        {
            var value = new SettingsPropertyValue(setting)
            {
                IsDirty = false,
                SerializedValue = GetValue(setting)
            };
            values.Add(value);
        }
        return values;
    }

    private XmlDocument _settingsXML;

    private XmlDocument SettingsXML
    {
        get
        {
            // if we dont hold an xml document, try opening one
            // if it doesnt exist then create a new one ready
            if (_settingsXML != null) return _settingsXML;
            _settingsXML = new XmlDocument();

            try
            {
                _settingsXML.Load(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PortableSettingsProvider: {0}", ex.Message);

                // create new document
                XmlDeclaration dec = _settingsXML.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
                _settingsXML.AppendChild(dec);

                XmlNode nodeRoot = _settingsXML.CreateNode(XmlNodeType.Element, SETTINGSROOT, "");
                _settingsXML.AppendChild(nodeRoot);
            }

            return _settingsXML;
        }
    }

    private string GetValue(SettingsProperty setting)
    {
        string ret;

        try
        {
            if (IsRoaming(setting))
                ret = SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + setting.Name).InnerText;
            else
                ret = SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + Environment.MachineName + "/" + setting.Name).InnerText;
        }
        catch (Exception ex)
        {
            if (setting.DefaultValue != null)
                ret = setting.DefaultValue.ToString();
            else
                ret = string.Empty;
        }
        return ret;
    }

    private void SetValue(SettingsPropertyValue propVal)
    {
        XmlElement settingNode;

        // determine if the setting is roaming
        // if roaming then the value is stored as an element under the root
        // otherwise it is stored under a machine name node 
        try
        {
            if (IsRoaming(propVal.Property))
                settingNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + propVal.Name);
            else
                settingNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + Environment.MachineName + "/" + propVal.Name);
        }
        catch (Exception ex)
        {
            settingNode = null;
        }

        // check to see if the node exists, if so then set its new value
        if (settingNode != null)
        {
            settingNode.InnerText = propVal.SerializedValue.ToString();
        }
        else
        {
            if (IsRoaming(propVal.Property))
            {
                //Store the value as an element of the Settings Root Node
                settingNode = SettingsXML.CreateElement(propVal.Name);
                settingNode.InnerText = propVal.SerializedValue.ToString();
                SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(settingNode);
            }
            else
            {
                //Its machine specific, store as an element of the machine name node,
                //creating a new machine name node if one doesnt exist.
                XmlElement machineNode;
                try
                {

                    machineNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + Environment.MachineName);
                }
                catch (Exception ex)
                {
                    machineNode = SettingsXML.CreateElement(Environment.MachineName);
                    SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(machineNode);
                }

                if (machineNode == null)
                {
                    machineNode = SettingsXML.CreateElement(Environment.MachineName);
                    SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(machineNode);
                }

                settingNode = SettingsXML.CreateElement(propVal.Name);
                settingNode.InnerText = propVal.SerializedValue.ToString();
                machineNode.AppendChild(settingNode);
            }
        }
    }

    private bool IsRoaming(SettingsProperty prop)
    {
        //Determine if the setting is marked as Roaming
        foreach (DictionaryEntry d in prop.Attributes)
        {
            var a = (Attribute)d.Value;
            if (a is SettingsManageabilityAttribute)
            {
                return true;
            }
        }
        return false;
    }
}