using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // определение состояния игры
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }
    public GameState currentState; // переменная для хранения текущего состояния игры

    public GameState previousState; // сохранение предыдущего состояние игры

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
    public float timeLimit; // Ограничение по времени в секундах
    float stopwatchTime; // Отслеживание времени, прошедшего с момента запуска секундомера.
    public Text stopwatchDisplay;


    // переменная для проверки, закончена ли игра
    public bool isGameOver = false;

    // переменная для проверки, вошел ли игрок в меню апгрейдов
    public bool choosingUpgrade;

    // ссылка на обьект игрока
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

                // код, который выполняется когда идет геймплей
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:

                // код, который выполняется когда игра на паузе
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:

                // код, который выполняется когда игра закончена
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; // остановить игру
                    Debug.Log("GAME IS OVER!");
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; // остановить игру
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("НЕИЗВЕСТНОЕ СОСТОЯНИЕ ИГРЫ!");
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
            Time.timeScale = 0f; // останавливаем игру
            pauseScreen.SetActive(true);
            Debug.Log("ИГРА ПРИОСТАНОВЛЕНА.");
        }
    }
    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // возобновляем игру
            pauseScreen.SetActive(false);
            Debug.Log("ИГРА ВОЗОБНОВЛЕНА");
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
            Debug.Log("Один из массивов не совпадает с длиной требуемого (смотри GameManager)");
            return;
        }

        // Присваиваем спрайты оружия на экран результатов
        for(int i = 0; i < chosenWeaponsUI.Count; i++) 
        {
            // Проверка, не является ли элемент null
            if (chosenWeaponsData[i].sprite)
            {
                // Включаем и устанавливаем спрайт
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;  
            }
            else
            {
                // Если оружия в слоте нет выключаем спрайт ( чтобы не было белого квадрата)
                chosenWeaponsUI[i].enabled = false;
            }
        } 

        // Присваиваем спрайты пассивных предметов на экран результатов
        for(int i = 0; i < chosenPassiveItemsUI.Count; i++) 
        {
            // Проверка, не является ли элемент null
            if (chosenPassiveItemsData[i].sprite)
            {
                // Включаем и устанавливаем спрайт
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;  
            }
            else
            {
                // Если оружия в слоте нет выключаем спрайт ( чтобы не было белого квадрата)
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
        // Считаем минуты и секунды для отображения на экране
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        // Обновляем время на экране
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
