﻿using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class PlayGamesController : MonoBehaviour
{
    public bool pauseOnLogin;
    public bool NoGPGMode;

    public int best { get; set; }

    public bool hasBest { get; set; }

    private ILeaderboard lb;

    // Use this for initialization
    void Awake()
    {
        Initialize();
        best = -1;
        if (pauseOnLogin)
            Time.timeScale = 0.01f;
        lb = PlayGamesPlatform.Instance.CreateLeaderboard();
        lb.id = Constants.leaderboard_stick_top_players;
        lb.timeScope = TimeScope.AllTime;
    }

    void Start()
    {
        SignIn();
        //GetBestScore();
    }

    void Update()
    {
        if (best == -1)
        {
            if (!NoGPGMode)
            {
                if (!lb.loading)
                    GetBestScore(lb);
            }
        }
    }

    public void Initialize()
    {
//        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
//            .EnableSavedGames() // enables saving game progress.
//            .Build();
//
//        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = false;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    public void SignIn()
    {
        // authenticate user:
        if (pauseOnLogin)
            Time.timeScale = 0.001f;
        Social.localUser.Authenticate((bool success) =>
            {
                // handle success or failure
                if (success)
                {
                    NoGPGMode = false;
                    if (pauseOnLogin)
                        Time.timeScale = 1;
                    GetBestScore(lb);
                    //Debug.Log("You've successfuly logged in");
                }
                else
                {
                    NoGPGMode = true;
                    #if UNITY_EDITOR
                    if (pauseOnLogin)
                        Time.timeScale = 1;
                    #elif UNITY_ANDROID
                    //SignIn();
                    #endif
                    Debug.Log("Log in failed!");
                }
            });
    }

    public void SubmitScore(int score)
    {
        // post score 12345 to leaderboard ID "Cfji293fjsie_QA")
        if (!NoGPGMode)
        {
            Social.ReportScore(score, Constants.leaderboard_stick_top_players, (bool success) =>
                {
                    // handle success or failure
                });
        }
    }

    public void GetBestScore(ILeaderboard lb)
    {
        lb.LoadScores(ok =>
            {
                hasBest = ok;
                if (ok)
                {
                    best = (int)lb.localUserScore.value;
                    if (pauseOnLogin)
                        Time.timeScale = 1;
                }
                else
                {
                    #if UNITY_EDITOR
                    if (pauseOnLogin)
                        Time.timeScale = 1;
                    #elif UNITY_ANDROID
                    //GetBestScore();
                    #endif
                }
            });
    }

    public void ShowLeaderboardUI()
    {
        // show leaderboard UI
        if (!NoGPGMode)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(Constants.leaderboard_stick_top_players);
        }
    }

}
