using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class PlayGamesController : MonoBehaviour
{
    private int best;

    // Use this for initialization
    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        SignIn();
    }

    public void Initialize()
    {
//        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
//            .EnableSavedGames() // enables saving game progress.
//            .Build();
//
//        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    public void SignIn()
    {
        // authenticate user:
        Time.timeScale = 0.001f;
        Social.localUser.Authenticate((bool success) =>
            {
                // handle success or failure
                if (success)
                {
                    Time.timeScale = 1;
                    //Debug.Log("You've successfuly logged in");
                }
                else
                {
                    Time.timeScale = 1;
                    Debug.Log("Log in failed!");
                }
            });
    }

    public void SubmitScore(int score)
    {
        // post score 12345 to leaderboard ID "Cfji293fjsie_QA")
        Social.ReportScore(score, Constants.leaderboard_stick_leaderboard, (bool success) =>
            {
                // handle success or failure
            });
    }

    public int GetBestScore()
    {
        //LeaderboardScoreData data = new LeaderboardScoreData(Constants.leaderboard_stick_leaderboard);
        int score = 0;
        //LoadScore();
        PlayGamesPlatform.Instance.LoadScores(
            Constants.leaderboard_stick_leaderboard,
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            (data) =>
            {
                if (data.Valid)
                    score = (int)data.PlayerScore.value;
            });
        return score;
    }

    void LoadScore()
    {
//        PlayGamesPlatform.Instance.LoadScores(
//            Constants.leaderboard_stick_leaderboard,
//            LeaderboardStart.PlayerCentered,
//            1,
//            LeaderboardCollection.Public,
//            LeaderboardTimeSpan.AllTime,
//            (data) =>
//            {
//                best = (int)data.PlayerScore.value;
//            });
        PlayGamesPlatform.Instance.LoadScores(
            Constants.leaderboard_stick_leaderboard,
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            (data) =>
            {
                if (data.Valid)
                    best = (int)data.PlayerScore.value;
            });
    }

    public void ShowLeaderboardUI()
    {
        // show leaderboard UI
        PlayGamesPlatform.Instance.ShowLeaderboardUI(Constants.leaderboard_stick_leaderboard);
    }

}
