using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StateMachine : MonoBehaviour
{
    public enum States
    {
        Start,
        Menu,
        Game,
        Over}

    ;

    public Spawn spawn;
    public ScoreController scoreController;
    public AdController adController;
    public InputHandler inputHandler;
    public PlayGamesController playGamesController;
    public AnimationController animationController;
    public Button catcher;
    public States state;

    private Coroutine currCoroutine;
    private WaitForSeconds animationTimeWFS;
    private WaitForSeconds splashScreeWFS;

    void Start()
    {
        animationTimeWFS = new WaitForSeconds(animationController.animationTime);
        splashScreeWFS = new WaitForSeconds(animationController.splashScreenDuration);
        adController.Init();
        NextState();
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

    public void ToGame()
    {
        if (currCoroutine == null)
            currCoroutine = StartCoroutine(MenuToGame());
    }

    IEnumerator StartToMenu()
    {
        //use coroutines;
        animationController.SplashScreenPlay();
        yield return splashScreeWFS;
        animationController.MenuToggle(true);
        catcher.interactable = true;
        inputHandler.canReadInput = false;
        yield return animationTimeWFS;
        state = States.Menu;
        adController.showBanner();
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

    IEnumerator GameToGameOver()
    {
        //use coroutines;
        spawn.spawnEnabled = false;
        animationController.GameOverToggle(true);
        yield return animationTimeWFS;
        state = States.Over;
        catcher.interactable = true;
        inputHandler.canReadInput = false;
        currCoroutine = null;
        // game off, game over on animations
    }

    IEnumerator GameOverToMenu()
    {
        //use coroutines;
        catcher.interactable = false;
        inputHandler.canReadInput = true;
        animationController.GameToggle(false);
        animationController.GameOverToggle(false);
        animationController.CameraAnimToggle(false);
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
