using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveFile
{
    public static void SavePlayerFile(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.qqq";
        FileStream stream = new FileStream(path, FileMode.Create);

        // something you want to save
        PlayerData data = new PlayerData(player);
        // Player data = null; // example

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayerFile()
    {
        string path = Application.persistentDataPath + "/player.qqq";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);


            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }

    public static void SaveSceneFile(Treasure treasure)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/scene.qqq";
        FileStream stream = new FileStream(path, FileMode.Create);

        // something you want to save
        ItemData data = new ItemData(treasure);
         //Player data = null; // example

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ItemData LoadSceneFile()
    {
        string path = Application.persistentDataPath + "/scene.qqq";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);


            ItemData data = formatter.Deserialize(stream) as ItemData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
}
