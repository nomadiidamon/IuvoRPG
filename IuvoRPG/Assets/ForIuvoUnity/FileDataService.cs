using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataService : IDataService
{
    ISerializer serializer;
    string dataPath;
    string fileExtension;

    public FileDataService(ISerializer serializer)
    {
        dataPath = Application.persistentDataPath;
        fileExtension = "json";
        this.serializer = serializer;
    }

    string GetPathToFile(string fileName)
    {
        return Path.Combine(dataPath, string.Concat(fileName, ".", fileExtension));
    }


    public void Delete(string name)
    {
        string fileLocation = GetPathToFile(name);

        if (File.Exists(fileLocation))
        {
            File.Delete(fileLocation);
        }
    }

    public void DeleteAll()
    {
        foreach (string filePath in Directory.GetFiles(dataPath))
        {
            File.Delete(filePath);
        }
    }

    public IEnumerable<string> ListSaves()
    {
        foreach (string filePath in Directory.EnumerateFiles(dataPath))
        {
            if (Path.GetExtension(filePath) == fileExtension)
            {
                yield return Path.GetFileNameWithoutExtension(filePath);
            }
        }
    }

    public GameData Load(string name)
    {
        string fileLocation = GetPathToFile(name);

        if (!File.Exists(fileLocation))
        {
            throw new ArgumentException($"No Persisted GameData with name '{name}.");
        }

        return serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
    }

    public void Save(GameData data, bool overwrite = true)
    {
        string fileLocation = GetPathToFile(data.Name);
        if (!overwrite && File.Exists(fileLocation))
        {
            throw new IOException($"The file '{data.Name}.{fileExtension}' already exists and cannot be overwitten.");
        }

        File.WriteAllText(fileLocation, serializer.Serialize(data));
    }
}
