using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float currentScore = 0;
    public float totalScore;
    public int playerLives;
    private float currentTime;
    private float levelCompletedTime;
    private bool timerActive = false;
    public TMPro.TextMeshProUGUI currentTimeText;
    public TMPro.TextMeshProUGUI currentScoreText;


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
    }

    //This sets up the timer and resets the current score.
    public void GameStart()
    {
        StartTimer();
    }

    //This will add players current score to their total score.
    public void LevelEnd()
    {
        levelCompletedTime = currentTime;
        StopTimer();
        if (levelCompletedTime >= 600)
        {
            totalScore += currentScore;
        }
        else if(levelCompletedTime >= 480)
        {
            totalScore += (currentScore * 1.25f);
        }
        else if(levelCompletedTime >= 420)
        {
            totalScore += (currentScore * 1.5f);
        }
        else if (levelCompletedTime >= 360)
        {
            totalScore += (currentScore * 1.75f);
        }
        else if (levelCompletedTime >= 300)
        {
            totalScore += (currentScore * 2f);
        }
        else if (levelCompletedTime >= 240)
        {
            totalScore += (currentScore * 2.5f);
        }
        else if (levelCompletedTime >= 180)
        {
            totalScore += (currentScore * 3f);
        }
        else if (levelCompletedTime < 180)
        {
            totalScore += (currentScore * 4f);
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
}
