using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    public StateMachine stateMachine;
    
    // public properties
    public Vector2 cursorPos { get; private set; }

    public bool onStick { get; private set; }

    public bool canReadInput { get; set; }

    // private variables
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        cursorPos = (Vector2)transform.position + Vector2.right;
        onStick = false;
    }

    public void CatchTap(Button self)
    {
        switch (stateMachine.state)
        {
            case StateMachine.States.Menu:
                if (!stateMachine.needTutorial)
                {
                    stateMachine.ToGame();
                    self.interactable = false;
                }
                else
                {
                    stateMachine.ToTutorial();
                }
                break;
            case StateMachine.States.Tutorial:
                stateMachine.ToGame();
                self.interactable = false;
                break;
            case StateMachine.States.Over:
                stateMachine.ToMenu();
                self.interactable = false;
                break;
            case StateMachine.States.Start:
                break;
            case StateMachine.States.Game:
                break;
            default: 
                stateMachine.ToMenu();
                self.interactable = false;
                break;
        }
    }

    void OnMouseDown()
    {
        if (canReadInput)
        {
            onStick = true;
        }
    }

    void OnMouseDrag()
    {
        if (canReadInput)
        {
#if UNITY_EDITOR
            cursorPos = cam.ScreenToWorldPoint(Input.mousePosition); //mouse position in world coordinates
#elif UNITY_ANDROID
		cursorPos = cam.ScreenToWorldPoint (Input.GetTouch(0).position); //where you've touched
#endif
        }
        else
        {
            onStick = false;
        }
    }

    void OnMouseUp()
    {
        cursorPos = (Vector2)transform.position + Vector2.right;
        onStick = false;
    }
}