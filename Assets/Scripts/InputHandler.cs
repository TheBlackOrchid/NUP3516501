using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour {

    //public variables
    public Text text;

	//public properties
	public bool onStick { get; set; }

	//private variables

	// Use this for initialization
	void Start ()
	{
		onStick = false;
	}
		


	void Update ()
	{
#if UNITY_EDITOR
#elif UNITY_ANDROID
		HandleTouch();
#endif
        //Debug.Log ("onStick = " + onStick);
        text.text = onStick.ToString();
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



# elif UNITY_ANDROID //touch input handles here

    void HandleTouch() //function called in update
	{
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((t.position)), Vector2.zero);

                if (hit.collider != null)
                {
                    onStick = true;
                }
            }
            else if (t.phase == TouchPhase.Ended || 
                t.phase == TouchPhase.Canceled)
            {
                onStick = false;
            }
        }
	}

#endif

}
