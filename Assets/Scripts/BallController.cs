using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

    public ScoreController scoreController;
	public float lifeSpan = 5;
    public int lives = 3;

	private int lifeCounter;
	private WaitForSeconds lifeSpanWFS;

    void Start() {
		lifeCounter = lives;
		lifeSpanWFS = new WaitForSeconds (lifeSpan);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Counter")) {
            scoreController.SetText();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            lifeCounter--;
            CheckLives();
        }
    }

	void OnEnable() {
		//Debug.Log (name + " onEnable");
		StartCoroutine (Live ());
	}

	void OnDisable() {
		StopAllCoroutines ();
		lifeCounter = lives;
	}

	IEnumerator Live()
	{
		yield return lifeSpanWFS;
		gameObject.SetActive (false);
	}

    void CheckLives() {
		if (lifeCounter <= 0) {
			this.gameObject.SetActive (false);
        }
    }

}