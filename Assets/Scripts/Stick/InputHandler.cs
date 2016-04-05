using UnityEngine;

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

    public void CatchTap() 
    {
        if (stateMachine.state == StateMachine.States.Menu || stateMachine.state == StateMachine.States.Over)
        {
            stateMachine.NextState();
        }
        //else { Debug.Log("Wrong state"); }
    }

    void OnMouseDown()
    {
        if (canReadInput) {
            onStick = true;
        }
    }

    void OnMouseDrag()
    {
        if (canReadInput) {
#if UNITY_EDITOR
            cursorPos = cam.ScreenToWorldPoint(Input.mousePosition); //mouse position in world coordinates
#elif UNITY_ANDROID
		cursorPos = cam.ScreenToWorldPoint (Input.GetTouch(0).position); //where you've touched
#endif
        }
    }

    void OnMouseUp()
    {
        cursorPos = (Vector2)transform.position + Vector2.right;
        onStick = false;
    }
}