using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    public GameObject PausePanel;
    public GameObject ArenaModePanel;
    public TextMeshProUGUI countDownText;
    public WaveSpawnerScript spawnerScript;
    public WaypointScript waypointScript;
    public Transform playerTransform {get; private set;}
    public bool Paused = false;
    private void Awake()
    {
        Time.timeScale = 1;

        if(instance != null)
        {
            Debug.Log("Another instance of Game Manager found in the scene");
        }
        instance = this;
    }
    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        if(SceneManager.GetActiveScene().name.Equals("Arena Scene"))
        {
            ArenaModePanel.SetActive(true);
        }
    }
    private void Update()
    {
       
        if(waypointScript.markerActive)
        {
            waypointScript.Waypoint(playerTransform);
        }

        if(spawnerScript != null)
        {
            if(spawnerScript.countDown < 4)
            {
                countDownText.gameObject.SetActive(true);
                if(spawnerScript.countDown < 1f)
                {
                    countDownText.text = "Enemies spawning!";
                }
                else
                countDownText.text = Mathf.FloorToInt(spawnerScript.countDown).ToString();
            }
            if(spawnerScript.countDown <= .3f)
            {
                countDownText.gameObject.SetActive(false);
            }
            if(SceneManager.GetActiveScene().name.Equals("Arena Scene") && ArenaModePanel.activeInHierarchy)
            { 
                if(Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E))
                {
                    ArenaModePanel.SetActive(false);
                    spawnerScript.BeginArena();
                }
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Paused) 
            {
                UnpauseGame();
            }
            else 
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Paused = true;
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void UnpauseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Paused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
       SceneController.instance.MainMenu(); 
    }
    public void Quit()
    {
       SceneController.instance.QuitGame(); 
    }
}   
