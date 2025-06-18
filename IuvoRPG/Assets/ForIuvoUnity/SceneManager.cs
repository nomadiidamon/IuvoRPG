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

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
