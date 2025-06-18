using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem>
{
    [SerializeField] public GameData gameData;
    IDataService dataService;

    protected override void Awake()
    {
        base.Awake();
        dataService = new FileDataService(new JsonSerializer());
    }

    public void NewGame()
    {
        gameData = new GameData
        {
            Name = "New Game",
            CurrentLevelName = "firstMovementTest"
        };
        SceneManager.Instance.LoadScene(gameData.CurrentLevelName);

    }

    public void SaveGame() => dataService.Save(gameData);

    

    public void LoadGame(string gameName)
    {
        gameData = dataService.Load(gameName);

        if (String.IsNullOrWhiteSpace(gameData.CurrentLevelName))
        {
            gameData.CurrentLevelName = "firstMovementTest";
        }

        SceneManager.Instance.LoadScene(gameData.CurrentLevelName);
    }

    public void ReloadGame() => LoadGame(gameData.CurrentLevelName);

    public void DeleteGame() => dataService.Delete(gameData.Name);

    void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISavable, new()
    {
        var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
        if (entity != null)
        {
            if (data == null)
            {
                data = new TData { Id = entity.Id };
            }
            entity.Bind(data);
        }
    }

    void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISavable, new()
    {
        var entities = FindObjectsByType<T>(FindObjectsSortMode.None);
        foreach (var entity in entities)
        {
            var data = datas.FirstOrDefault(d => d.Id == entity.Id);
            if (data == null)
            {
                data = new TData { Id = entity.Id };
                datas.Add(data);
            }
            entity.Bind(data);
        }
    }

}
