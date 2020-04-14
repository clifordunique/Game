using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TmpSaver : Singleton<TmpSaver>
{
    Dictionary<string, float> save1;
    Dictionary<string, float> save2;
    bool active = false;
    bool saved;

    public void Init()
    {
        if (active) return;
        DontDestroyOnLoad(this);
        save1 = new Dictionary<string, float>();
        save2 = new Dictionary<string, float>();
        active = true;
    }

    //change a vlue of a field
    //automatically create one if the field havent been create
    public void ChangeValue(int fileOption, string field, float v)
    {
        if (fileOption == 1)
        {
            if (!save1.ContainsKey(field)) save1.Add(field, v);
            else save1[field] = v;
        }
        if (fileOption == 2)
        {
            if (!save2.ContainsKey(field)) save2.Add(field, v);
            else save2[field] = v;
        }
    }

    //add a new field (e.g. the number player get hit) to dictionary with initial value 0
    public void AddFieldToSave(int fileOption, string field)
    {
        if (fileOption == 1)
        {
            if (!save1.ContainsKey(field)) save1.Add(field, 0);

        }
        if (fileOption == 2)
        {
            if (!save1.ContainsKey(field)) save2.Add(field, 0);
        }
    }
    
    public void WriteFile()
    {
        if (saved) return;
        saved = true;
        StreamWriter f1 = new StreamWriter(Application.dataPath + "/file1.csv");
        foreach(string key in save1.Keys)
        {
            f1.WriteLine(key + "," + save1[key]);
        }
        f1.Close();
        StreamWriter f2 = new StreamWriter(Application.dataPath + "/file2.csv");
        foreach (string key in save2.Keys)
        {
            f2.WriteLine(key + "," + save2[key]);
        }
        f2.Close();
    }
}
