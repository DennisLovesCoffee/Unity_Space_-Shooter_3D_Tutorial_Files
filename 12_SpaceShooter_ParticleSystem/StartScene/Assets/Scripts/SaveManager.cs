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
            //ResetValues();
        }
    }

    private SaveClass saveClass;


    private void ResetValues()
    {
        saveClass.gold = 100;
        saveClass.levelsCompleted = -1;
        saveClass.ownedSpacesips = new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0 };
        Save();
    }

    public bool IsSpaceshipowned(int idx)
    {
        if(saveClass.ownedSpacesips[idx] == 1)
        {
            return true;
        }
        else
        {
            return false;
        }

        //return saveClass.ownedSpacesips[idx] == 1;
    }

    public void PurchaseSpaceship(int idx)
    {
        saveClass.ownedSpacesips[idx] = 1;
        Save();
    }

    public void RemoveGold(int amt)
    {
        saveClass.gold -= amt;
        Save();
    }

    public void CompletedNextLevel()
    {
        saveClass.levelsCompleted++;
        Save();
    }

    public int GetLevelsCompleted()
    {
        return saveClass.levelsCompleted;
    }

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
