using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class QuestLogButton : MonoBehaviour, ISelectHandler
{
    public Button button {get; private set;}
    private TextMeshProUGUI buttonText;
    private UnityAction onSelectAction;
    public void Initialize(string displayName, UnityAction selectAction)
    {
        this.button = this.GetComponent<Button>();
        this.buttonText = this.GetComponentInChildren<TextMeshProUGUI>();
        this.buttonText.text = displayName;
        this.onSelectAction = selectAction;
    }

    public void OnSelect(BaseEventData baseEvent) 
    {
        onSelectAction();
    }
}
