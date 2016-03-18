using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

    public ScoreController scoreController;
	public Color[] conditionColors = new Color[4]; // colors to show the ball's condition in the ascending order
	public float lifeSpan = 5;
    public int lives = 3;

	private WaitForSeconds lifeSpanWFS;
	private SpriteRenderer rend;
	private int lifeCounter;

	void Awake()
	{
		lifeCounter = lives;
		lifeSpanWFS = new WaitForSeconds (lifeSpan);
		rend = GetComponent<SpriteRenderer> ();
	}

    void OnTriggerEnter2D(Collider2D collider) 
	{
		if (collider.gameObject.CompareTag ("Counter")) {
			scoreController.ScoreUp (); // lucky one
			Kill (); // but you have to die
		} else if (collider.gameObject.CompareTag ("Killer")) {
			scoreController.Wasted ();
			Kill (); // i'm sorry...
		}
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Wall")) {
            lifeCounter--;
			ChangeColor (lifeCounter);
            CheckLives();
        }
    }

	void OnEnable() 
	{
		lifeCounter = lives; // reset lives and ready to go
		ChangeColor (lifeCounter); // back to noraml
		StartCoroutine (Live ()); // if enabled - begin to live;
	}

	void OnDisable() 
	{
		StopAllCoroutines (); // if disabled - stop counting seconds to death
	}

	IEnumerator Live()
	{
		yield return lifeSpanWFS; // you have only this much
		Kill (); // the only purpose of life is death
	}

    void CheckLives() {
		if (lifeCounter <= 0) { 
			Kill (); // you ran out of lives! you have to die;
        }
    }

	void ChangeColor(int index)
	{
		rend.color = conditionColors [index];
	}

	void Kill()
	{
		gameObject.SetActive (false); // not very interesting death
	}

}