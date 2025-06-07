using UnityEngine;

public class ISaveToPrefab : MonoBehaviour
{
    public string savedAs;
    public bool hasBeenSaved = false;
    public bool changeScale = false;
    public bool triggerSave = false;
    public Vector3 newScale = Vector3.one;

    public void Update()
    {
        if (triggerSave && !hasBeenSaved)
        {
            if (changeScale)
            {
                PrefabSaver.ScaleAndSaveAsPrefab(this.gameObject, newScale, savedAs);
                hasBeenSaved = true;
                triggerSave = false;
            }
            else
            {
                PrefabSaver.SaveAsPrefab(this.gameObject, gameObject.name);
                hasBeenSaved = true;
                triggerSave = false;
            }

        }
    }
}
