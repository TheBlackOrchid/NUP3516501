﻿using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{

    public ScoreController scoreController;
	public AudioSource audioG;

    [Range(0f, 1f)]
    public float[] conditionAlpha = new float[4];
    // colors to show the ball's condition in the ascending order
    public float lifeSpan = 5;
    public int lives = 3;
    public float hitCooldown = 0.5f;
    public float deathDuration = 0.5f;

    private WaitForSeconds lifeSpanWFS;
    private WaitForSeconds hitCooldownWFS;
    private WaitForEndOfFrame wfeof;
    private SpriteRenderer rend;
    private Color currColor;
    private bool canHit = true;
    private int lifeCounter;

    void Awake()
    {
        lifeCounter = lives;
        lifeSpanWFS = new WaitForSeconds(lifeSpan);
        hitCooldownWFS = new WaitForSeconds(hitCooldown);
        wfeof = new WaitForEndOfFrame();
        rend = GetComponent<SpriteRenderer>();
        StartCoroutine(InitialDeath());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Counter"))
        {
			audioG.Play();
            scoreController.ScoreUp(); // lucky one
        }
        else if (collider.gameObject.CompareTag("Killer"))
        {
            scoreController.Wasted();
        }
        Kill(); // but you have to die anyway
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (canHit)
            {
                StartCoroutine(HitCooldown());
            }
        }
    }

    void OnEnable()
    {
        lifeCounter = lives; // reset lives and ready to go
        canHit = true; // reset cooldown trigger
        ChangeColor(lifeCounter); // back to noraml
        StartCoroutine(Live()); // if enabled - begin to live;
    }

    void OnDisable()
    {
        StopAllCoroutines(); // if disabled - stop counting seconds to death
    }

    IEnumerator InitialDeath() // needed to change the color
    {
        yield return new WaitForEndOfFrame();
        Kill();
    }

    IEnumerator Live()
    {
        yield return lifeSpanWFS; // you have only this much
        Kill(); // the only purpose of life is death
    }

    IEnumerator HitCooldown()
    {
        canHit = false;
        lifeCounter--;
        ChangeColor(lifeCounter);
        CheckLives();
        yield return hitCooldownWFS;
        canHit = true;
    }

    void CheckLives()
    {
        if (lifeCounter <= 0)
        {
            scoreController.Wasted();
            Kill(); // you ran out of lives! you have to die;
        }
    }

    void ChangeColor(int index)
    {
        if (index >= 0 && index < conditionAlpha.Length)
        {
            currColor = rend.color;
            currColor.a = conditionAlpha[index];
            rend.color = currColor;
        }
        else
        {
            currColor = rend.color;
            currColor.a = conditionAlpha[0];
            rend.color = currColor;
        }
    }

    public void Kill()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(KillCoroutine());        
    }

    IEnumerator KillCoroutine()
    {
        float elapsedTime = 0;
        currColor = rend.color;
        if (currColor.a == 0)
            elapsedTime = deathDuration + 1;
        while (elapsedTime < deathDuration)
        {
            currColor.a = Mathf.Lerp(currColor.a, 0, elapsedTime / deathDuration);
            rend.color = currColor;
            elapsedTime += Time.deltaTime;
            yield return wfeof;
        }
        gameObject.SetActive(false);
    }
}