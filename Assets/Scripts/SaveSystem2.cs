using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class SaveSystem2 : MonoBehaviour
{
    public static string path;

    private static Dictionary<string, Dictionary<string, string>> save = new Dictionary<string, Dictionary<string, string>>();

    public static void start_base()     //запустить вначале
    {
        if (path == null) path = Application.persistentDataPath;
    }

    public static void set_data(string folder, string file, object obj)
    {
        string data = JsonUtility.ToJson(obj);
        try
        {
            save[folder].Add(file, data);
            save_data(folder, file, data);
        }
        catch
        {
            try
            {
                save[folder][file] = data;
                save_data(folder, file, data);
            }
            catch
            {
                save.Add(folder, new Dictionary<string, string> { { file, data } });
                save_data(folder, file, data);
            }
        }
    }

    public static async Task<string> get_data(string folder, string file)
    {
        try
        {
            return save[folder][file];
        }
        catch
        {
            string data = load_data(folder, file);
            if (data != null)
            {
                await save_data(folder, file, data);
                return data;
            }
            else return null;
        }
    }

    static async Task save_data(string folder, string file, string data)
    {
        try
        {
            var area = Directory.CreateDirectory($"{path}/{folder}");
            using StreamWriter streamWriter = new StreamWriter($"{area}/{file}");
            await streamWriter.WriteAsync(data);
            streamWriter.Close();
        }
        catch { }
    }

    public static string load_data(string folder, string file)
    {
        try
        {
            StreamReader streamReader = new StreamReader($"{path}/{folder}/{file}");
            string data = streamReader.ReadLine();
            streamReader.Close();
            return data;
        }
        catch { return null; }
    }
    public static void delete_data(string folder, string file)
    {
        try
        {
            Directory.Delete($"{path}/{folder}/{file}");
            save[folder].Remove(file);
        }
        catch { }
    }
}
