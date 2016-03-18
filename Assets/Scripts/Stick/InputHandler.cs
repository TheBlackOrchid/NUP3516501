using UnityEngine;
using UnityEngine.UI; //test

public class InputHandler : MonoBehaviour {

    // public variables
    public Text debugText; //test

    // public properties
    public Vector2 cursorPos { get; private set; }
	public bool onStick { get; private set; }

    // private variables
    private Camera cam;

    // Use this for initialization
    void Start ()
	{
        cam = Camera.main;
		cursorPos = (Vector2)transform.position + Vector2.right;
        onStick = false;
	}

	void OnMouseDown()
	{
		onStick = true;
	}

	void OnMouseDrag()
	{
		#if UNITY_EDITOR
		cursorPos = cam.ScreenToWorldPoint (Input.mousePosition); //mouse position in world coordinates
		#elif UNITY_ANDROID
		cursorPos = cam.ScreenToWorldPoint (Input.GetTouch(0).position); //where you've touched
		#endif
	}

	void OnMouseUp()
	{
		onStick = false;
	}
}