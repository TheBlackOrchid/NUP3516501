using UnityEngine;
using UnityEngine.UI; //test

public class InputHandler : MonoBehaviour {

    //public variables
    public Text debugText; //test

    //public properties
    public Vector2 cursorPos { get; private set; }
	public bool onStick { get; private set; }

    //private variables
    private Camera cam;
#if UNITY_EDITOR
#elif UNITY_ANDROID
    private Touch currTouch;
#endif

    // Use this for initialization
    void Start ()
	{
        cam = Camera.main;
        onStick = false;
	}

	void Update ()
	{
#if UNITY_EDITOR
        cursorPos = cam.ScreenToWorldPoint(Input.mousePosition); //mouse position in world coordinates
#elif UNITY_ANDROID
		HandleTouch();
#endif
        //test
        debugText.text = onStick.ToString();
	}
		
#if UNITY_EDITOR //mouse input handles here

	void LateUpdate()
	{
		if (Input.GetMouseButtonUp(0)) //when you released the button
		{
			onStick = false;
		}
	}

	void OnMouseOver() // when mouse is over this gameObject
	{
		if (Input.GetMouseButtonDown(0)) //when you clicked down the button
		{   
            onStick = true;
		}
	}

#elif UNITY_ANDROID //touch input handles here

    void HandleTouch() //function must be called in update
	{
        if (Input.touchCount > 0) //if there is at least one touch
        {
            currTouch = Input.GetTouch(0);    
            cursorPos = cam.ScreenToWorldPoint(currTouch.position); //where you've touched

            if (currTouch.phase == TouchPhase.Began) //if touch hes just started
            {
                RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint((currTouch.position)), Vector2.zero); //raycasting from touch position

                if (hit.collider.CompareTag("Stick") && hit.collider != null) //checking tags
                {
                    onStick = true; //you've touched the stick
                }
            }
            else if (currTouch.phase == TouchPhase.Ended ||  //revert to false if touch has ended
                currTouch.phase == TouchPhase.Canceled)
            {
                onStick = false;
            }
        }
	}

#endif

}
