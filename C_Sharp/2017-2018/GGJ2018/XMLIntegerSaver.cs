using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class XMLSerializationExample : MonoBehaviour {

    public int sceneInteger;
    public string path;

    public void SaveNewInteger(int newInt)
    {
        SaveToXML(newInt);
        print("Saved new integer: " + newInt + " | sceneInteger is now: " + sceneInteger);
    }

    public int GetTheCorrectInteger()
    {
        sceneInteger = LoadFromXML<int>();
        print(sceneInteger + " with the captured value");
        return sceneInteger;
    }

	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveToXML(sceneInteger);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            sceneInteger = LoadFromXML<int>();
        }
	}

    void SaveToXML(object obj, bool appendFile = false)
    {
        var serializer = new XmlSerializer(obj.GetType());

        using (var stream = new StreamWriter(Application.dataPath + path, appendFile))
        {
            serializer.Serialize(stream, obj);
        }
    }

    T LoadFromXML<T>()
    {
        var serializer = new XmlSerializer(typeof(T));

        using (var stream = new StreamReader(Application.dataPath + path))
        {
            object obj = serializer.Deserialize(stream);
            return (T)obj;
        }
    }
}
