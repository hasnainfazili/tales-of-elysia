using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class _Scroll : MonoBehaviour
{
    [SerializeReference] private float popupDuration;
    [SerializeReference] private GameObject prefab;
    [SerializeReference] private MMF_Player feedbackPlayer;
    private void OnEnable()
    {
        EventsManager.instance.gameEvents.onADHDTriggered += ADHDTriggered;
    }

    private void OnDisable()
    {
        EventsManager.instance.gameEvents.onADHDTriggered -= ADHDTriggered;
    }


    private void ADHDTriggered()
    {
        if(!prefab.activeInHierarchy)
        StartCoroutine(Duration());
    }


    IEnumerator Duration()
    {
        feedbackPlayer.PlayFeedbacks();
        prefab.SetActive(true);
        yield return new WaitForSeconds(popupDuration);
        prefab.SetActive(false);
    }
}
