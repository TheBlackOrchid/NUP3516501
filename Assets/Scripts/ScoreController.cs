using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    public GameManager gameManager;
    public StateMachine stateMachine;
    public PlayGamesController playGamesController;
    public ShareInfo gameOverShareInfo;
    public Spawn spawn;
    public Text scoreText;
    public Text bestText;
    public int minScore = 1;

    public int score { get; private set; }

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        stateMachine = GameObject.FindWithTag("GameManager").GetComponent<StateMachine>();
        score = 0;
        SetText();
    }

    public void ScoreUp()
    {
        score++;
        SetText();
        gameManager.MoveHoles();
        gameManager.CloseHoles();
        gameManager.IncreaseDifficulty();
        gameManager.ChangeColor();
        gameOverShareInfo.text = string.Format(gameOverShareInfo.format, score);
    }

    public void Wasted()
    {
        playGamesController.SubmitScore(score);
        if (score > playGamesController.best)
            playGamesController.best = score;
        if (score >= minScore)
            stateMachine.GameOver();
        else
            score = 0;
        SetText();
        SetBestScore(playGamesController.best);
        spawn.KillAll();
        gameManager.MoveHoles();
        gameManager.CloseHoles();
        gameManager.ResetDifficulty(); // check if score is more than x. if so - show wasted UI, else do nothing
        gameManager.ChangeColor();
    }

    public void SetScore(int value)
    {
        score = value;
        SetText();
    }

    public void SetBestScore(int best)
    {
        if (playGamesController.hasBest)
        {
            SetText(bestText, "BEST " + best);
        }
        else
        {
            SetText(bestText, "");
        }
    }

    void SetText()
    {
        scoreText.text = score + "";
    }

    void SetText(Text text, string str)
    {
        text.text = str;
    }
}
