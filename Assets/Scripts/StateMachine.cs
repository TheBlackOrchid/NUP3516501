﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StateMachine : MonoBehaviour
{
    public enum States
    {
        Start,
        Menu,
        Tutorial,
        Game,
        Over}

    ;

    public Spawn spawn;
    public ScoreController scoreController;
    //public AdController adController;
    public InputHandler inputHandler;
    public PlayGamesController playGamesController;
    public AnimationController animationController;
    //public IABController iabController;
    public AudioManager audioManager;
    public AudioSource splashAudio;
    public AudioSource bgAudio;
    public Button catcher;
    //public Button noAdsButton;
    public States state;
    public bool needTutorial;

    private Coroutine currCoroutine;
    private WaitForSeconds animationTimeWFS;
    private WaitForSeconds splashScreeWFS;

    void Start()
    {
        animationTimeWFS = new WaitForSeconds(animationController.animationTime);
        splashScreeWFS = new WaitForSeconds(animationController.splashScreenDuration);
        NextState();
        //adController.Init();
//        iabController.Init();
    }

    [ContextMenu("Next State")]
    public void NextState()
    {
        switch (state)
        {
            case States.Start:
                if (currCoroutine == null)
                    currCoroutine = StartCoroutine(StartToMenu());
                break;
            case States.Menu:
                if (currCoroutine == null)
                    currCoroutine = StartCoroutine(MenuToGame());
                break;
            case States.Game:
                if (currCoroutine == null)
                    currCoroutine = StartCoroutine(GameToGameOver());
                break;
            case States.Over:
                if (currCoroutine == null)
                    currCoroutine = StartCoroutine(GameOverToMenu());
                break;
            default:
                if (currCoroutine == null)
                    currCoroutine = StartCoroutine(GameOverToMenu());
                break;
        }
    }

    public void GameOver()
    {
        if (currCoroutine == null)
            currCoroutine = StartCoroutine(GameToGameOver());
    }

    public void ToMenu()
    {
        if (currCoroutine == null)
            currCoroutine = StartCoroutine(GameOverToMenu());
    }

    public void ToTutorial()
    {
        if (currCoroutine == null)
            currCoroutine = StartCoroutine(MenuToTutorial());
    }

    public void ToGame()
    {
        if (currCoroutine == null)
        {
            if (state == States.Menu)
                currCoroutine = StartCoroutine(MenuToGame());
            else if (state == States.Tutorial)
                currCoroutine = StartCoroutine(TutorialToGame());
        }
    }

    public void Continue()
    {
        if (currCoroutine == null)
            currCoroutine = StartCoroutine(GameOverToGame());
    }

    //    public void DisableAds()
    //    {
    //        noAdsButton.interactable = false;
    //        adController.hideBanner();
    //    }

    IEnumerator StartToMenu()
    {
        //use coroutines;
        splashAudio.Play();
        animationController.SplashScreenPlay();
        yield return splashScreeWFS;
        animationController.MenuToggle(true);
        catcher.interactable = true;
        inputHandler.canReadInput = false;
        splashAudio.Stop();
        bgAudio.Play();
        //iabController.QueryInventory();
        yield return animationTimeWFS;
        state = States.Menu;
        needTutorial = playGamesController.best <= 2;
//        if (!iabController.CheckNoAds())
//        {
//            adController.showBanner();
//        }
//        else
//        {
//            noAdsButton.interactable = false;
//        }
        currCoroutine = null;
        // splash scree off, menu on animations
    }

    IEnumerator MenuToGame()
    {
        //use coroutines;
        catcher.interactable = false;
        inputHandler.canReadInput = true;
        animationController.MenuToggle(false);
        yield return animationTimeWFS;
        state = States.Game;
        animationController.GameToggle(true);
        animationController.CameraAnimToggle(true);
        yield return animationTimeWFS;
        spawn.spawnEnabled = true;
        currCoroutine = null;
        // menu off, game on animations
    }

    IEnumerator MenuToTutorial()
    {
        animationController.MenuToggle(false);
        yield return animationTimeWFS;
        state = States.Tutorial;
        animationController.GameToggle(true);
        animationController.CameraAnimToggle(true);
        yield return animationTimeWFS;
        animationController.TutorialToggle(true);
        needTutorial = false;
        currCoroutine = null;
    }

    IEnumerator TutorialToGame()
    {
        //use coroutines;
        catcher.interactable = false;
        inputHandler.canReadInput = true;
        animationController.TutorialToggle(false);
        yield return animationTimeWFS;
        state = States.Game;
        spawn.spawnEnabled = true;
        currCoroutine = null;
        // menu off, game on animations
    }

    IEnumerator GameToGameOver()
    {
        //use coroutines;
        spawn.spawnEnabled = false;
        animationController.GameOverToggle(true);
        audioManager.ToggleGameOver(true);
        inputHandler.canReadInput = false;
        yield return animationTimeWFS;
        state = States.Over;
        catcher.interactable = true;
        currCoroutine = null;
        // game off, game over on animations
    }

    IEnumerator GameOverToGame()
    {
        catcher.interactable = false;
        inputHandler.canReadInput = true;
        animationController.GameOverToggle(false);
        yield return animationTimeWFS;
        state = States.Game;
        audioManager.ToggleGameOver(false);
        animationController.GameToggle(true);
        yield return animationTimeWFS;
        spawn.spawnEnabled = true;
        currCoroutine = null;
    }

    IEnumerator GameOverToMenu()
    {
        //use coroutines;
        catcher.interactable = false;
        inputHandler.canReadInput = true;
        animationController.GameToggle(false);
        animationController.GameOverToggle(false);
        animationController.CameraAnimToggle(false);
        audioManager.ToggleGameOver(false);
        yield return animationTimeWFS;
        animationController.MenuToggle(true);
        catcher.interactable = true;
        inputHandler.canReadInput = false;
        state = States.Menu;
        scoreController.SetScore(0);
        currCoroutine = null;
        // game over off, menu on animations
    }
}
