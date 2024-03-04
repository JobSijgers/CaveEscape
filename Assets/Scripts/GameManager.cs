using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private PlayerTracker tracker;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject deathScreen;

    [SerializeField] private GameObject[] pauseDisables;
    [SerializeField] private GameObject[] pauseEnables;

    [SerializeField] private GameObject[] menuDisables;
    [SerializeField] private GameObject[] menuEnables;

    public EGameSate currentState = EGameSate.Menu;
    private Coroutine moveToDeathScreenRoutine;
    private Coroutine moveToWinScreenRoutine;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Time.timeScale = 0f;
    }
    public void StartGame()
    {
        AudioManager.Instance.Play("Music");
        Time.timeScale = 1f;
        currentState = EGameSate.Playing;
        tracker.enabled = true;
        foreach (GameObject go in menuEnables)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in menuDisables)
        {
            go.SetActive(true);
        }
    }
    public void GoToMenu(int state)
    {
        AudioManager.Instance.Stop("Music");
        Time.timeScale = 1.0f;
        switch (state)
        {
            case 1:
                if (moveToDeathScreenRoutine != null)
                {
                    StopCoroutine(moveToDeathScreenRoutine);
                }
                deathScreen.SetActive(false);
                break;
            case 2:
                if (moveToWinScreenRoutine != null)
                {
                    StopCoroutine(moveToWinScreenRoutine);
                }
                winScreen.SetActive(false); 
                break;
        }

        StartCoroutine(tracker.MoveToMenu());
        foreach (GameObject go in pauseEnables)
        {
            go.SetActive(false);
        }
    }
    public void EnableMenu()
    {
        foreach (GameObject go in menuEnables)
        {
            go.SetActive(true);
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        currentState = EGameSate.Playing;
        foreach (GameObject go in pauseDisables)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in pauseEnables)
        {
            go.SetActive(false);
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        currentState = EGameSate.Paused;
        foreach (GameObject go in pauseDisables)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in pauseEnables)
        {
            go.SetActive(true);
        }
    }
    public void LostGame()
    {
        AudioManager.Instance.Stop("Music");
        currentState = EGameSate.Dead;
        foreach (GameObject go in pauseDisables)
        {
            go.SetActive(false);
        }
        moveToDeathScreenRoutine = StartCoroutine(tracker.MoveToDeathScreen());
    }
    public void EnableDeathScreen()
    {
        deathScreen.SetActive(true);
    }
    public void WonGame()
    {
        AudioManager.Instance.Stop("Music");

        currentState = EGameSate.Won;
        foreach (GameObject go in pauseDisables)
        {
            go.SetActive(false);
        }
        moveToWinScreenRoutine = StartCoroutine(tracker.MoveToWinScreen());
    }
    public void EnableWinscreen()
    {
        winScreen.SetActive(true);
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
    private void Update()
    {
        if (currentState == EGameSate.Menu || currentState == EGameSate.Dead || currentState == EGameSate.Won)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == EGameSate.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
}
