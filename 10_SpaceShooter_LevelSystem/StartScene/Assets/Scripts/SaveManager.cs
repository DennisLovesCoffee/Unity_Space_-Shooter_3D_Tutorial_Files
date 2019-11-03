using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Xml.Serialization;
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Load();
        }
    }

    private SaveClass saveClass;

    public void AddGold()
    {
        saveClass.gold++;
        Save();
    }

    public int GetGold()
    {
        return saveClass.gold;
    }

    public void Save()
    {
        string serializedObject = Serialize(saveClass);
        PlayerPrefs.SetString("saveFile", serializedObject);
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("saveFile"))
        {
            //load deserialize
            saveClass = Deserialize(PlayerPrefs.GetString("saveFile"));
        }
        else
        {
            //create a file and save
            Debug.Log("Creating new file");
            saveClass = new SaveClass();
            Save();
        }
    }

    private string Serialize(SaveClass toBeSerialized)
    {
        XmlSerializer xml = new XmlSerializer(typeof(SaveClass));
        StringWriter writer = new StringWriter();
        xml.Serialize(writer, toBeSerialized);
        return writer.ToString();
    }

    private SaveClass Deserialize(string xmlSerialized)
    {
        XmlSerializer xml = new XmlSerializer(typeof(SaveClass));
        StringReader reader = new StringReader(xmlSerialized);
        return xml.Deserialize(reader) as SaveClass;
    }

}
