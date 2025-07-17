using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{

    [Header("Game Variables")]

    public PlayerController player;
    public float time;
    public bool timeActive;

    [Header("Game UI")]

    public TMP_Text gameUI_score;
    public TMP_Text gameUI_health;
    public TMP_Text gameUI_time;

    [Header("Countdown UI")]
    public int countdown;
    public TMP_Text countdownText;

    [Header("End Screen UI")]
    public TMP_Text endUI_score;
    public TMP_Text endUI_time;

    [Header("Screens")]
    public GameObject countdownUI;
    public GameObject gameUI;
    public GameObject endUI;

    // Start is called before the first frame update
    void Start()
    {
        /* player = GameObject.Find("Steve").GetComponent<PlayerController>(); */

        // Make sure the timer is set to zero
        time = 0;

        // Disable player movement
        player.enabled = false;

        // Set screen to the countdown
        SetScreen(countdownUI);

        // Start coroutine
        StartCoroutine(CountDownRoutine());

    }

    IEnumerator CountDownRoutine()
    {
       countdownText.gameObject.SetActive(true);
        countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "AURA";
        yield return new WaitForSeconds(1f);

        // Enable player movement
        player.enabled = true;

        // Start the game
        startGame();
    }

    void startGame()
    {

        // Locks cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set the screen to see stats
        SetScreen(gameUI);

        // Start the timer
        timeActive = true;
    }

    public void endGame()
    {
        // End the timer
        timeActive = false;

        // Disable player movement
        player.enabled = false;

        // Set UI to display stats
        endUI_time.text = "Total Speedrun Time: " + (time * 1).ToString("F2");
        endUI_score.text = "Ender Pearls Collected: " + player.coinCount;

        // Unlocks cursor

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        SetScreen(endUI);

    }

    public void OnRestartButton()
    {
        // Restart the game to play again
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Keep track of the time that goes by
        if(timeActive)
        {
            time += Time.deltaTime;
        }

        // Set UI to display stats
        gameUI_score.text = "Ender Pearls: " + player.coinCount;
        gameUI_health.text = "Hearts: " + player.health;
        gameUI_time.text = "Speedrun Clock: " + (time * 1).ToString("F2");
    }

    public void SetScreen(GameObject screen)
    {
        // Disable all other screens
        gameUI.SetActive(false);
        countdownUI.SetActive(false);
        endUI.SetActive(false);

        // Activate the requested screen
        screen.SetActive(true);
    }
}
