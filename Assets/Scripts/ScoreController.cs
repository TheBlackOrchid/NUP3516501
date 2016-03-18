using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public Text scoreText;

    private int score;
    
    void Start() {
        score = 0;
        SetText();
    }
    
	public void ScoreUp()
	{
		score++;
		SetText ();
	}

	public void Wasted()
	{
		score = 0;
		SetText ();
	}

	void SetText() {
		scoreText.text = score + "";
	}

}
