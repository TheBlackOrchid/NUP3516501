using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public StateMachine stateMachine;
    
    // public properties
    public Vector2 cursorPos { get; private set; }

    public bool onStick { get; private set; }

    // private variables
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        cursorPos = (Vector2)transform.position + Vector2.right;
        onStick = false;
    }

    void Update()
    {
        if (stateMachine.state == StateMachine.States.Menu || stateMachine.state == StateMachine.States.Over)
        {
			#if UNITY_EDITOR
			if (Input.GetMouseButtonDown(0))
            {
                stateMachine.NextState();
            }
			#elif UNITY_ANDROID
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				stateMachine.NextState();				
			}
			#endif
        }
    }

    void OnMouseDown()
    {
        onStick = true;
    }

    void OnMouseDrag()
    {
        #if UNITY_EDITOR
        cursorPos = cam.ScreenToWorldPoint(Input.mousePosition); //mouse position in world coordinates
        #elif UNITY_ANDROID
		cursorPos = cam.ScreenToWorldPoint (Input.GetTouch(0).position); //where you've touched
        #endif
    }

    void OnMouseUp()
    {
        cursorPos = (Vector2)transform.position + Vector2.right;
        onStick = false;
    }
}