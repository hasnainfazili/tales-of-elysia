using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFocusable
{
   public string GetFocusableName();
   public Transform GetFocusableTransform();
   
}
