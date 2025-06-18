using UnityEngine;

public class JsonSerializer : ISerializer
{
    public string Serialize<T>(T json)
    {
        return JsonUtility.ToJson(json, true);
    }

    public T Deserialize<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }



}
