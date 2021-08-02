using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private float currentScore;
    private float totalScore;
    private int currentLives;
    private string scoreMod;
    private float currentTime;
    private float levelCompletedTime;
    private bool timerActive = false;
    public TMPro.TextMeshProUGUI currentTimeText;
    public TMPro.TextMeshProUGUI currentScoreText;
    public TMPro.TextMeshProUGUI currentLivesText;


    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            currentTime += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = "Time : " + time.Minutes.ToString() + ":" + time.Seconds.ToString();
        currentScoreText.text = "Score : " + currentScore;
        currentLivesText.text = "Lives: x " + currentLives;
    }

    //This sets up the timer and resets the current score.
    public void GameStart()
    {
        currentLives = 3;
        currentScore = 0;
        currentTime = 0;
        StartTimer();
    }

    //This will add players current score to their total score.
    public void LevelEnd()
    {
        levelCompletedTime = currentTime;

        StopTimer();
        bool totalingScore = true;
        while(totalingScore == true)
        {
            if (levelCompletedTime >= 600)
            {
                totalScore += currentScore;
            }
            else if(levelCompletedTime >= 480)
            {
                totalScore += (currentScore * 1.25f);
                scoreMod = "x 1.25";
            }
            else if(levelCompletedTime >= 420)
            {
                totalScore += (currentScore * 1.5f);
                scoreMod = "x 1.5";
            }
            else if (levelCompletedTime >= 360)
            {
                totalScore += (currentScore * 1.75f);
                scoreMod = "x 1.75";
            }
            else if (levelCompletedTime >= 300)
            {
                totalScore += (currentScore * 2f);
                scoreMod = "x 2";
            }
            else if (levelCompletedTime >= 240)
            {
                totalScore += (currentScore * 2.5f);
                scoreMod = "x 2.5";
            }
            else if (levelCompletedTime >= 180)
            {
                totalScore += (currentScore * 3f);
                scoreMod = "x 3";
            }
            else if (levelCompletedTime < 180)
            {
                totalScore += (currentScore * 4f);
                scoreMod = "x 4";
            }
            totalingScore = false;
        }

    }

    public void StartTimer()
    {
        timerActive = true;
    }
    public void StopTimer()
    {
        timerActive = false;
    }
    public void AddScore(int score)
    {
        currentScore += score;
    }
    public void OnPlayerDeath()
    {
        if (currentLives > 0)
        {
            currentLives--;
            if (currentLives < 0)
            {
                currentLives = 0;
            }
            SceneLoader.instance.RestartLevel();
            currentTime = 0;
            currentScore = 0;
        }
        else
        {
            LevelEnd();
            GameOver();
        }
    }
    public void GameOver()
    {
        LevelEnd();
        SceneLoader.instance.LoadGameOver();
    }
    public void MoveToNextLevel()
    {
        SceneLoader.instance.LoadNextLevel();
        currentScore = 0;
        currentTime = 0;
    }
}
