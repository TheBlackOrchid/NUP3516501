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
    public InputHandler inputHandler;
    public PlayGamesController playGamesController;
    public AnimationController animationController;
    public Button catcher;
    public States state;


    private WaitForSeconds animationTimeWFS;
    private WaitForSeconds splashScreeWFS;

    void Start()
    {
        animationTimeWFS = new WaitForSeconds(animationController.animationTime);
        splashScreeWFS = new WaitForSeconds(animationController.splashScreenDuration);
        NextState();
    }

    [ContextMenu("Next State")]
    public void NextState()
    {
        switch (state)
        {
            case States.Start:
                StartCoroutine(StartToMenu());
                break;
            case States.Menu:
                StartCoroutine(MenuToGame());
                break;
            case States.Game:
                StartCoroutine(GameToGameOver());
                break;
            case States.Over:
                StartCoroutine(GameOverToMenu());
                break;
            default:
                StartCoroutine(GameOverToMenu());
                break;
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameToGameOver());
    }

    public void ToMenu()
    {
        StartCoroutine(GameOverToMenu());
    }

    public void ToGame()
    {
        StartCoroutine(MenuToGame());
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
        // game over off, menu on animations
    }
}
