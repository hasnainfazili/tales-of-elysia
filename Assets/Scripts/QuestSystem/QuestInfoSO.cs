using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSo", menuName = ("ScriptableObjects/QuestInfoSo"), order = 1)]
public class QuestInfoSO : ScriptableObject
{
   [field : SerializeField] public string id {get; private set;}

   [Header("General")]
   public string displayName;
   [Header("Steps")]
   public GameObject[] questStepPrefabs;
   [Header("Rewards")]
   public string description;

   private void OnValidate(){
    #if UNITY_EDITOR
    id = this.name;
    UnityEditor.EditorUtility.SetDirty(this);
    #endif
   }
}
