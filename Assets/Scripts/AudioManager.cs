using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sprite onGfx;
    public Sprite offGfx;

    public AudioMixerSnapshot defaultSnapshot;
    public AudioMixerSnapshot musicOffSnapshot;
    public AudioMixerSnapshot gameOverSnapshot;

    public float transitionTime = 0.1f;

    private bool musicState = true;

    void Start()
    {
        defaultSnapshot.TransitionTo(0f);
    }

    public void ToggleAudio()
    {
        if (musicState)
        {
            musicOffSnapshot.TransitionTo(transitionTime);
            musicState = false;

        }
        else
        {
            defaultSnapshot.TransitionTo(transitionTime);
            musicState = true;
        }
    }

    public void ToggleGameOver(bool toggle)
    {
        if (musicState)
        {
            if (toggle)
                gameOverSnapshot.TransitionTo(transitionTime);
            else
                defaultSnapshot.TransitionTo(transitionTime);
        }
    }

    public void ChangeImageColor(SoundButtonsHolder buttons)
    {
        if (!musicState) // if on, then turn off
        {
            foreach (GameObject b in buttons.soundButtons)
            {
                b.GetComponent<Image>().sprite = offGfx;
            }
        }
        else // else turn on
        {
            foreach (GameObject b in buttons.soundButtons)
            {
                b.GetComponent<Image>().sprite = onGfx;
            }
        }
    }
}
