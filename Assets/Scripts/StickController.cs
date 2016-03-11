using UnityEngine;
using UnityEngine.UI; //test

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputHandler))]

public class StickController : MonoBehaviour {

    //public variables
	public Text debugText;
	public LineRenderer debugLine;

    //private variables
    private Rigidbody2D rb;
    private InputHandler iH;
    private Vector2 centerPos;
    private Vector2 cursorPos;
    private Vector3 positionVector;
    private float angle;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        iH = GetComponent<InputHandler>();
        centerPos = transform.position;
		if (Debug.isDebugBuild) 
		{ 
			debugLine.SetPosition (0, centerPos);
		}
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (iH.onStick)
        {
            cursorPos = iH.cursorPos; //getting mouse or touch position
            positionVector = (cursorPos - centerPos).normalized; // a normalized vector from center of the stick pointed to the cursor position 
                                                                 //each coordinate of which being cosine and sine respectively
            angle =  Mathf.Atan(positionVector.y / positionVector.x) * Mathf.Rad2Deg; // an atangent of sine over cosine of an angle in radians converted to degrees
        }
		if (Debug.isDebugBuild) 
		{ 
			//---test---//
			Debug.DrawRay(centerPos, positionVector);
			debugText.text = string.Format ("angle = {0}\nposition vector = {1:0.00},{2:0.00}\nfps = {3:00}\nfsps = {4:00}", 
                                            angle, positionVector.x, positionVector.y, 1 / Time.deltaTime, 1 / Time.fixedDeltaTime);
			debugLine.SetPosition (1, centerPos + (Vector2)positionVector);
		}
	}

    void FixedUpdate()
    {
        if (iH.onStick) 
		{ 
			rb.MoveRotation(angle); 
		} //rotation
    }
}
