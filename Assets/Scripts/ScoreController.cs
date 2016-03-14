using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public Text scoreText;

    private int score;
    
    void Start() {
        score = -1;
        SetText();
    }
    
    public void SetText() {
        score++;
        scoreText.text = "Score: " + score;
    }

}
