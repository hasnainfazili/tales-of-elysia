using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusableObject : MonoBehaviour, IFocusable
{
    [SerializeField] string Name;

    public string GetFocusableName()
    {
        return Name;
    }
    public Transform GetFocusableTransform()
    {
        return transform;
    }
}
