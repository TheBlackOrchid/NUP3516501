using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

	public Text scoreText;

	private GameManager gM;
	private int score;

	void Start()
	{
        gM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
		score = 0;
		SetText();
	}

	public void ScoreUp()
	{
		score++;
		SetText();
		gM.MoveHoles();
        gM.IncreaseBallSpeed();
	}

	public void Wasted()
	{
		score = 0;
		SetText();
		gM.MoveHoles();
        gM.ResetBallSpeed();
        // check if score is more than x. if so - show wasted UI, else do nothing
	}

	void SetText()
	{
		scoreText.text = score + "";
	}

}
