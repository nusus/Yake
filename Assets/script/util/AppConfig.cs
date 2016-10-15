using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class AppConfig {
    private static AppConfig s_AppConfig;

    private string m_ConfigFilePath;
    private bool m_IsInit;
    private XmlDocument m_XmlDoc;

    private Dictionary<string, string> m_AttributePool;

    private AppConfig()
    {
        m_ConfigFilePath = Application.persistentDataPath + @"\yake.config.xml";

        m_IsInit = false;

        m_XmlDoc = new XmlDocument();

        m_AttributePool = new Dictionary<string, string>();
    }

    public static AppConfig GetInstance()
    {
        if(null == s_AppConfig)
        {
            s_AppConfig = new AppConfig();
        }
        if (!s_AppConfig.m_IsInit)
        {
            s_AppConfig.Init();
            s_AppConfig.m_IsInit = true;
        }
        return s_AppConfig;
    }

    private void Init()
    {
        m_XmlDoc.Load(m_ConfigFilePath);
    }

    public string GetValueByName(string name)
    {
        string value;
        if(m_AttributePool.TryGetValue(name, out value))
        {
            return value;
        }

        XmlNode root = m_XmlDoc.SelectSingleNode("GameInfo");
        XmlNodeList xnl = root.ChildNodes;
        foreach (XmlNode xn in xnl)
        {
            if(xn.Attributes["name"].Value.Equals(name))
            {
                m_AttributePool.Add(name, xn.Attributes["value"].Value);
                return xn.Attributes["value"].Value;
            }            
        }
        return null;
    }

    public bool IsDebugMode()
    {
        return bool.Parse(GetInstance().GetValueByName("debugMode"));
    }

}
