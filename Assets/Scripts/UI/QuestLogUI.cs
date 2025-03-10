using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class QuestLogUI : MonoBehaviour
{
   [SerializeField] private GameObject contentParent;
   [SerializeField] private QuestLogScrollingList scrollingList;
   [SerializeField] private TextMeshProUGUI questDisplayNameText;
   [SerializeField] private TextMeshProUGUI questStatusText;
   [SerializeField] private TextMeshProUGUI questDescription;
   [SerializeField] private TextMeshProUGUI questObj;
   [SerializeField] private GameObject questPopupContainer;
   [SerializeField] private TextMeshProUGUI questPopUpText;
   private Button firstSelectedButton;
   public static QuestLogUI instance {get; private set;}

   private void OnEnable()
   {
        EventsManager.instance.miscEvents.onQuestLogToggled += QuestLogToggled;
        EventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        EventsManager.instance.miscEvents.onObjectiveUpdate += UpdateObj;

        EventsManager.instance.questEvents.onStartQuest += StartedQuestPopup;
        EventsManager.instance.questEvents.onFinishQuest += FinishedQuestPopup;

   }

   private void OnDisable() 
   {
        EventsManager.instance.miscEvents.onQuestLogToggled -= QuestLogToggled;
        EventsManager.instance.miscEvents.onObjectiveUpdate -= UpdateObj;
        EventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        EventsManager.instance.questEvents.onStartQuest -= StartedQuestPopup;
        EventsManager.instance.questEvents.onFinishQuest -= FinishedQuestPopup;

   }

   private void Awake()
   {
     instance = this;
   }
 
   private void QuestLogToggled()
   {
     if(contentParent.activeInHierarchy) {
          HideUI();
     } 
     else 
     {
          ShowUI();
     }
   }

   private void ShowUI()
   {
     contentParent.SetActive(true);
     //Disablemovement;
     if(firstSelectedButton != null) {
          firstSelectedButton.Select();
     }
   }
   private void HideUI()
   {
     contentParent.SetActive(false);
     //Reenable player movement
     EventSystem.current.SetSelectedGameObject(null);
   }
   private void QuestStateChange(Quests quest)
   {
     QuestLogButton questLogButton = scrollingList.CreateButtonIfNotExists(quest, () => {
          SetQuestLogInfo(quest);
     });
     
     if(firstSelectedButton == null) 
     {
          firstSelectedButton = questLogButton.button;
     }
   }
   private void SetQuestLogInfo(Quests quest)
   {
     if(quest.state.Equals(QuestState.FORGOTTEN))
     {
          questDisplayNameText.text = "????";
          questStatusText.text = "Hmm I'm forgetting something.";
          questDescription.text = "";
     }
     else 
     {
          questDisplayNameText.text = quest.info.displayName;
          questStatusText.text = quest.GetFullStatus();
          questDescription.text = quest.GetQuestDescription();
     }
    
   }
   private void UpdateObj(string text)
  {
      questObj.text = text;
   }
  public AudioClip QuestStartSound;
  public AudioClip QuestFinishedSound;
  public AudioClip ObjUpdateSound;
   private void StartedQuestPopup(string id)
   {
     questPopupContainer.SetActive(true);
     questPopUpText.text = "Quest Started: " +  id;
     SoundFXManager.instance.PlaySingleSoundFXClip(QuestStartSound, transform.position, 1f);
     StartCoroutine(HideText(questPopupContainer));

   }
   private void FinishedQuestPopup(string id)
   {
     questPopupContainer.SetActive(true);
     questPopUpText.text = "Quest Finished: " +  id;
     SoundFXManager.instance.PlaySingleSoundFXClip(QuestFinishedSound, transform.position, 1f);

     StartCoroutine(HideText(questPopupContainer));
   }

   IEnumerator HideText(GameObject container)
   {
      yield return new WaitForSecondsRealtime(5f);
      container.SetActive(false);
      questObj.gameObject.SetActive(true);
      SoundFXManager.instance.PlaySingleSoundFXClip(ObjUpdateSound, transform.position, 1f);
   }
}
