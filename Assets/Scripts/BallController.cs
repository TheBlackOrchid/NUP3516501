using UnityEngine;

public class BallController : MonoBehaviour {

    public ScoreController scoreController;
    public int liveCounter = 3;
    public bool available { get; set; }


    void Start() {
        available = true;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Counter")) {
            scoreController.SetText();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            liveCounter--;
            CheckLives();
        }
    }


    void CheckLives() {
        if (liveCounter <= 0) {
            available = true;
        }
    }

}