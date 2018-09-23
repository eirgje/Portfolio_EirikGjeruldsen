using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class XMLSerialization : MonoBehaviour {

    private static string path_pickups;
    private static string path_spawnPoint;
    private static string path_gameData;


    private void Start()
    {
        path_pickups = Application.dataPath + "/Pickups-save";
        //print(path_pickups);

        path_spawnPoint = Application.dataPath + "/SpawnPoint-save";
        //print(path_spawnPoint);

        path_gameData = Application.dataPath + "/GameData-save";
        //print(path_gameData);
    }

    /// <summary>
    /// Used to save the data of type #object# in first parameter.
    /// </summary>
    /// <param name="newdata"></param>
    /// <param name="path"></param>
    public void SaveData(object newdata, string path)
    {
        SaveToXML(newdata, path);
    }

    /// <summary>
    /// Get the file paths for saved files.
    /// <para>0 = Pick ups</para>
    /// <para>1 = Spawn point</para>
    /// <para>2 = Game Data</para>
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string GetPath(int input)
    {
        if (input == 0)
            return path_pickups;
        else if (input == 1)
            return path_spawnPoint;
        else if (input == 2)
            return path_gameData;

        print("Could not find PATH, returning #empty# string");
        return "";

    }

    #region Load Functions (return functions)


    public Saving_GameData LoadGameData()
    {
        return LoadFromXML<Saving_GameData>(path_gameData);
    }

    public List<Saving_PickUps> LoadPickupListData()
    {
        return LoadFromXML<List<Saving_PickUps>>(path_pickups);
    }

    public Saving_SpawnPoint LoadSpawnPointData()
    {
        return LoadFromXML<Saving_SpawnPoint>(path_spawnPoint);
    }

    #endregion


    void SaveToXML(object obj, string path, bool appendFile = false)
    {
        var serializer = new XmlSerializer(obj.GetType());

        using (var stream = new StreamWriter(path, appendFile))
        {
            serializer.Serialize(stream, obj);
            //stream.Close();
        }

    }

    T LoadFromXML<T>(string path)
    {
        var serializer = new XmlSerializer(typeof(T));

        using (var stream = new StreamReader(path))
        {
            object obj = serializer.Deserialize(stream);
            //stream.Close();
            return (T)obj;
        }
    }
}
