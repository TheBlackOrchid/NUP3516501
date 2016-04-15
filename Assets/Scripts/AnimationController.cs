using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    public Animator splashScreenLayout;
	public Animator tutorialLayout;
    public Animator menuLayout;
    public Animator gameLayout;
    public Animator gameOverLayout;
    public Animator cameraAnim;

    public float animationTime = 0.5f;
    public float splashScreenDuration = 4.5f;

    public void SplashScreenPlay()
    {
        splashScreenLayout.SetTrigger("play");
    }

	public void TutorialToggle (bool toggle)
	{
		tutorialLayout.SetBool("fade", toggle);
	}

    public void MenuToggle(bool toggle)
    {
        menuLayout.SetBool("slideUp", toggle);
    }

    public void GameToggle(bool toggle)
    {
        gameLayout.SetBool("slideDown", toggle);
    }

    public void GameOverToggle(bool toggle)
    {
        gameOverLayout.SetBool("slideUp", toggle);
    }

    public void CameraAnimToggle(bool toggle)
    {
        cameraAnim.SetBool("slideDown", toggle);
    }
}
