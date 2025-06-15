using UnityEngine;

public class PlayerAnimatorHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] public Animator animator;


    [SerializeField] public Context playerContext { get; set; }
    public ContextPlayerHandlerKey HandlerKey => ContextPlayerHandlerKey.AnimatorHandler;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
