using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyVFX : MonoBehaviour
{
   void Awake()
   {
     Destroy(gameObject, 1f);
   }
}
