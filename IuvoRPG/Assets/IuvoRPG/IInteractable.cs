using UnityEngine;

public interface IInteractable : IObjectBehavior
{
    Collider InteractionArea { get; set; }
    bool CanInteract(IInteractor instigator);
}
