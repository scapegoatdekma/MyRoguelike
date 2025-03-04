using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // ����������� ��������� ����
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }
    public GameState currentState; // ���������� ��� �������� �������� ��������� ����

    public GameState previousState; // ���������� ����������� ��������� ����

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;


    [Header("Current Stat Displays")]
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentMightDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentMagnetDisplay;

    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedDisplay;
    public Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit; // ����������� �� ������� � ��������
    float stopwatchTime; // ������������ �������, ���������� � ������� ������� �����������.
    public Text stopwatchDisplay;


    // ���������� ��� ��������, ��������� �� ����
    public bool isGameOver = false;

    // ���������� ��� ��������, ����� �� ����� � ���� ���������
    public bool choosingUpgrade;

    // ������ �� ������ ������
    public GameObject playerObject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }

        DisableScreens();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:

                // ���, ������� ����������� ����� ���� ��������
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:

                // ���, ������� ����������� ����� ���� �� �����
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:

                // ���, ������� ����������� ����� ���� ���������
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; // ���������� ����
                    Debug.Log("GAME IS OVER!");
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; // ���������� ����
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("����������� ��������� ����!");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; // ������������� ����
            pauseScreen.SetActive(true);
            Debug.Log("���� ��������������.");
        }
    }
    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // ������������ ����
            pauseScreen.SetActive(false);
            Debug.Log("���� ������������");
        }
    }

    void CheckForPauseAndResume()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.name;
    }
    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }
    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if(chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.Log("���� �� �������� �� ��������� � ������ ���������� (������ GameManager)");
            return;
        }

        // ����������� ������� ������ �� ����� �����������
        for(int i = 0; i < chosenWeaponsUI.Count; i++) 
        {
            // ��������, �� �������� �� ������� null
            if (chosenWeaponsData[i].sprite)
            {
                // �������� � ������������� ������
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;  
            }
            else
            {
                // ���� ������ � ����� ��� ��������� ������ ( ����� �� ���� ������ ��������)
                chosenWeaponsUI[i].enabled = false;
            }
        } 

        // ����������� ������� ��������� ��������� �� ����� �����������
        for(int i = 0; i < chosenPassiveItemsUI.Count; i++) 
        {
            // ��������, �� �������� �� ������� null
            if (chosenPassiveItemsData[i].sprite)
            {
                // �������� � ������������� ������
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;  
            }
            else
            {
                // ���� ������ � ����� ��� ��������� ������ ( ����� �� ���� ������ ��������)
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime; 

        UpdateStopwatchDisplay();
        
        if(stopwatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopwatchDisplay()
    {
        // ������� ������ � ������� ��� ����������� �� ������
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        // ��������� ����� �� ������
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }
    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
