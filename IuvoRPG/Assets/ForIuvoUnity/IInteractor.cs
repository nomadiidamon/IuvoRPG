using UnityEngine;

public interface IInteractor : IObjectBehavior
{
    void Interact(IInteractable interaction);
}
