using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    public GameManager gameManager;
    public StateMachine stateMachine;
    public Spawn spawn;
    public Text scoreText;
    public int minScore = 1;

    private int score;

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
    }

    public void Wasted()
    {
        if (score >= minScore)
            stateMachine.NextState();
        else
            score = 0;
        SetText();
        spawn.KillAll();
        gameManager.MoveHoles();
        gameManager.CloseHoles();
        gameManager.ResetDifficulty(); // check if score is more than x. if so - show wasted UI, else do nothing
        gameManager.ChangeColor();
    }

    public void SetScore(int value)
    {
        score = value;
    }

    void SetText()
    {
        scoreText.text = score + "";
    }

}
