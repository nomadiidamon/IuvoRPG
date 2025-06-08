using UnityEngine;

public class ProvideCover : MonoBehaviour, IInteractable
{
    Collider IInteractable.InteractionArea { get; set; }
    GameObject IObjectBehavior.BehavioralObject { get; set; }

    

    protected void Awake()
    {
        
    }

    bool IInteractable.CanInteract(IInteractor instigator)
    {
        if (instigator == null) return false; return true;
    }

    void IObjectBehavior.PerformBehavior()
    {
        // if the target has a cover state, enter it

    }


    public virtual void SwitchCover(ProvideCover newCover)
    {

    }

    public virtual void EnterCover(ProvideCover newCover)
    {

    }

    public virtual void ExitCover(ProvideCover newCover)
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        

    }

    public void OnTriggerExit(Collider other)
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        
    }
}
