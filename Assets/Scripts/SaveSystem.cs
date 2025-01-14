using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System;

public class SaveSystem : MonoBehaviour
{
    private static Dictionary<string, Dictionary<string,string>> save = new Dictionary<string, Dictionary<string, string>>();

    public static bool busy_save;
    public static bool busy_load;
    static string way;

    public static void start_base() 
    {
        if (string.IsNullOrEmpty(way))
        {
            if (!PlayerPrefs.HasKey("reset_files"))
            {
                way = $"{Application.persistentDataPath}";
                new_way();
            }
            else way = $"{Application.persistentDataPath}/";
        }
    }
    

    public static async Task load_from_file(string save_name)
    {
            try
            {
                Dictionary<string, string> load = new Dictionary<string, string>();
                SaveData save_data = new SaveData();
                if (save.ContainsKey(save_name)) save[save_name].Clear();
                if (File.Exists(way + save_name + ".data"))
                {
                    using StreamReader file = new StreamReader(way + save_name + ".data");
                    var load_str = await file.ReadToEndAsync();
                    save_data = JsonUtility.FromJson<SaveData>(load_str);
                    file.Close();
                }

                for (int i = 0; i < save_data.list_1.Count; i++)
                {
                    load.Add(save_data.list_1[i], save_data.list_2[i]);
                }
                if (save.ContainsKey(save_name)) save[save_name] = load;
                else save.Add(save_name, load);
            }
            catch (Exception e) { Debug.Log(e); }
    }

    public static async Task save_to_file(string save_name)
    {
        if (save.ContainsKey(save_name))
        {
            try
            {
                SaveData save_data = new SaveData();
                using StreamWriter file = new StreamWriter(way + save_name + ".data");

                foreach (KeyValuePair<string, string> dictionary in save[save_name])
                {
                    save_data.list_1.Add(dictionary.Key);
                    save_data.list_2.Add(dictionary.Value);
                }
                await file.WriteAsync(JsonUtility.ToJson(save_data));
                file.Close();
            }
            catch (Exception e) { Debug.Log(e); }
        }
    }

    public class SaveData
    {
        public List<string> list_1 = new List<string>();
        public List<string> list_2 = new List<string>();
    }

     public static async Task<string> get_value(string save_name, string save_key)
    {
        if (save.ContainsKey(save_name))
        {
            if(save[save_name].ContainsKey(save_key)) return save[save_name][save_key];
            else return null;
        }
        else
        {
            await load_from_file(save_name);
            if (save[save_name].ContainsKey(save_key)) return save[save_name][save_key];
            else return null;
        };
    }
     public static async Task<Dictionary<string, string>> get_dict(string save_name)
    {
        if (save.ContainsKey(save_name))
        {
            return save[save_name];
        }
        else
        {
            await load_from_file(save_name);
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


    static async void new_way()
    {
      
        if (!PlayerPrefs.HasKey("reset_files"))
        {
            PlayerPrefs.SetInt("reset_files", 1);
            
            string[] files = Directory.GetFiles(Application.persistentDataPath.Replace(Application.productName, ""));

            foreach (string file in files)
            {
                using StreamReader fileR = new StreamReader(file);
                var file_data = await fileR.ReadToEndAsync();
                string path = file.Replace(Application.productName, $"{Application.productName}/");
                using StreamWriter fileW = new StreamWriter(path);
                await fileW.WriteAsync(file_data);
            }
            way = $"{Application.persistentDataPath}/";
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
    }
}