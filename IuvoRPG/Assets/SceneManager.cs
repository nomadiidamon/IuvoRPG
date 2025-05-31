using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public Camera cam;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
       
        cam = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
