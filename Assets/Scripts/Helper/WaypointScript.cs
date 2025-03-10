using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointScript : MonoBehaviour
{

    [SerializeField] Transform currentQuestStep;
    [SerializeField] Image wayPointImage;
    [SerializeField] Vector3 offset;
    public bool markerActive = false;
    public float distanceToObjective {get; private set;}
    public void OnEnable()
    {
        EventsManager.instance.questEvents.onQuestStepCreated += CreateWaypoint;
    }
    public void OnDisable()
    {
        EventsManager.instance.questEvents.onQuestStepCreated -= CreateWaypoint;
    }

    public void CreateWaypoint(Transform questStep)
    {
        markerActive = true;
        wayPointImage.gameObject.SetActive(true);
        currentQuestStep = questStep;
    }    
    public void Waypoint(Transform playerTransform)
    {
        if(currentQuestStep != null)
        {
        float minX = wayPointImage.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        float minY = wayPointImage.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.width - minY;
        distanceToObjective = Vector3.Distance(playerTransform.position, currentQuestStep.transform.position);
        Vector2 positionOnCamera = Camera.main.WorldToScreenPoint(currentQuestStep.transform.position + offset);
        if(Vector3.Dot((currentQuestStep.transform.position - playerTransform.position),playerTransform.forward) < 0)
        {
            if(positionOnCamera.x < Screen.width / 2)
            {
                positionOnCamera.x = maxX;
            }
            else
            {
                positionOnCamera.x = minX;
            }
        }
        positionOnCamera.x = Mathf.Clamp(positionOnCamera.x, minX, maxX);
        positionOnCamera.y = Mathf.Clamp(positionOnCamera.y, minY, maxY);

        wayPointImage.transform.position = positionOnCamera;
        }
        
    }

}
