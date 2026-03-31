using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public string GetInteractionPrompt(/*GameObject trigger*/)
    {
        Debug.Log($"can interact with {gameObject.name}");
        return "lol";
    }

    public void OnInteract()
    {
        Debug.Log("interacting");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
