#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class PrefabSaver
{
    public static void SaveAsPrefab(GameObject obj, string path)
    {
        string assetPath = $"Assets/Prefabs/{path}.prefab";
        PrefabUtility.SaveAsPrefabAssetAndConnect(obj, assetPath, InteractionMode.UserAction);
        Debug.Log("Prefab saved to: " + assetPath);
    }


    public static void ScaleAndSaveAsPrefab(GameObject obj, Vector3 newScale, string path)
    {
        obj.transform.localScale = newScale;
        string assetPath = $"Assets/Prefabs/{path}.prefab";
        PrefabUtility.SaveAsPrefabAssetAndConnect(obj, assetPath, InteractionMode.UserAction);
        Debug.Log("Prefab saved to: " + assetPath);
    }

}
#endif
