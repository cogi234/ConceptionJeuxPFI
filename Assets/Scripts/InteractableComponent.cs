using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableComponent : MonoBehaviour
{
    public UnityEvent onInteract = new UnityEvent();

    public void Interact()
    {
        onInteract.Invoke();
    }
}
