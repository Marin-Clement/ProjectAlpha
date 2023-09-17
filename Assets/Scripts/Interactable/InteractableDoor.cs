using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractableDoor : InteractableObject
{
    protected override void Interact()
    {
        Debug.Log("Interacting with door");
    }
}