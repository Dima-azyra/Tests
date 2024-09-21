using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    private static Dictionary<string, Dictionary<string,string>> save = new Dictionary<string, Dictionary<string, string>>();

    public static bool busy_save;
    static string way = Application.persistentDataPath;

    public static void load_from_file(string save_name)
    {
        Dictionary<string, string> load = new Dictionary<string, string>();
        SaveData save_data = new SaveData();

        if (save.ContainsKey(save_name)) save[save_name].Clear();
        if (File.Exists(way + save_name + ".data"))
        {
            StreamReader file = new StreamReader(way + save_name + ".data");
            save_data = JsonUtility.FromJson<SaveData>(file.ReadLine());
            file.Close();
        }

        for (int i = 0; i < save_data.list_1.Count; i++)
        {
            load.Add(save_data.list_1[i], save_data.list_2[i]);
        }
        if (save.ContainsKey(save_name)) save[save_name] = load;
        else save.Add(save_name, load);
    }

    public static bool save_to_file(string save_name)
    {
        if (!busy_save)
        {
            busy_save = true;
            if (save.ContainsKey(save_name))
            {
                SaveData save_data = new SaveData();
                StreamWriter file = new StreamWriter(way + save_name + ".data");
               
                foreach (KeyValuePair<string, string> dictionary in save[save_name])
                {
                    save_data.list_1.Add(dictionary.Key);
                    save_data.list_2.Add(dictionary.Value);
                }
                file.WriteLine(JsonUtility.ToJson(save_data));
                file.Close();
            }
            return busy_save = false;
        }
        else return true;
    }

    public class SaveData
    {
        public List<string> list_1 = new List<string>();
        public List<string> list_2 = new List<string>();
    }

    public static string get_value(string save_name, string save_key)
    {
        if (save.ContainsKey(save_name))
        {
            if(save[save_name].ContainsKey(save_key)) return save[save_name][save_key];
            else return null;
        }
        else
        {
            load_from_file(save_name);
            if (save[save_name].ContainsKey(save_key)) return save[save_name][save_key];
            else return null;
        };
    }
    public static Dictionary<string, string> get_dict(string save_name)
    {
        if (save.ContainsKey(save_name))
        {
            return save[save_name];
        }
        else
        {
            load_from_file(save_name);
            if (save.ContainsKey(save_name))
            {
                return save[save_name];
            }
            else return new Dictionary<string, string>();
        }
           
    }

    public static void set_save(string save_name, string key, string value)
    {
        if (save.ContainsKey(save_name))
        {
            if (save[save_name].ContainsKey(key))
            {
                if (!value.Equals("")) save[save_name][key] = value;
                else save[save_name].Remove(key);
            }
            else
            {
                if (!value.Equals("")) save[save_name].Add( key, value);
            }
        }
        else
        {
            if (!value.Equals("")) save.Add(save_name, new Dictionary<string, string>() { { key, value } });
        }
    }
    public static void delete_key(string save_name, string key)
    {
        if (save.ContainsKey(save_name))
        {
           save[save_name].Remove(key);
        }
    }

    public static void clear_save_and_file(string save_name)
    {
        if (save.ContainsKey(save_name))
        {
            save[save_name].Clear();
            if (File.Exists(way + save_name + ".data"))
                File.Delete(way + save_name + ".data");
        }
    }
}