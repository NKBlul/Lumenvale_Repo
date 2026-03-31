using UnityEngine;

public interface IInteractable
{
    string GetInteractionPrompt(/*GameObject trigger*/);
    void OnInteract();
}
