using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;
    public Dictionary<float, WaitForSeconds> waitForSecondsDictionary = new Dictionary<float, WaitForSeconds>();



    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);

    }

}
