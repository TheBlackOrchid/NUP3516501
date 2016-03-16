using UnityEngine;
using System.Collections;

public class SideController : MonoBehaviour {

	private HoleController hC;
	private Transform myTransform;
	private Vector3 ScaleVector;

	private float x; // side up x position
	private float y; // see my drawings

	private float angle; // angle of a slope

	private int xDirInt; // xDir in use
	private int xDirIntReverse; // -original
	private int xDirIntOriginal; // initial xDir

	private int yDirInt; // same thing as with xDir
	private int yDirIntReverse;
	private int yDirIntOriginal;

	private float prevSideSeparation; // for optimisation purposes
	private float prevWallWidht;
	private float prevAngle; // for optimisation purposes
	private int xPrevDir; // for optimisation purposes

	// Use this for initialization
	void Start () {
		myTransform = transform;
		hC = myTransform.parent.GetComponent<HoleController> ();

		xDirIntOriginal = (int)Mathf.Sign(myTransform.localScale.x);
		xDirIntReverse = -xDirIntOriginal;

		yDirIntReverse = (int)Mathf.Sign(myTransform.localScale.y);
		yDirIntOriginal = -yDirIntReverse;
	}
	
	// Update is called once per frame
	void Update () {
		AdjustSize ();
		AdjustPosition ();
		UpdatePrevious ();
	}

	void AdjustSize () // controlls object scale to create the slope that is needed
	{
		angle = HoleController.Angle(myTransform.position - hC.targetPosition); // general way to build position vectors
		if (angle != prevAngle || hC.wallWidth != prevWallWidht) { // is this angle not the same as the last time? if it is, then do nothing
			y = hC.wallWidth * Mathf.Tan(angle * Mathf.Deg2Rad); // such trigonometry... to understand draw a right triangle

			if (angle > 0) // angle is grater than zero when the side is under the target
			{
				xDirInt = xDirIntReverse; // flipping dirs
				yDirInt = yDirIntReverse;
			} 
			else // ...and lesss than zero when it is below
			{
				xDirInt = xDirIntOriginal; // flipping dirs
				yDirInt = yDirIntOriginal;
			}

			ScaleVector = new Vector3(hC.wallWidth * HoleController.PosToScale * xDirInt, y * HoleController.PosToScale * yDirInt, 1); //constructing a scale vector
			myTransform.localScale = ScaleVector; // applying the scale
		}
	}

	void AdjustPosition() 
	{
		if (xDirInt != xPrevDir || hC.sideSeparation != prevSideSeparation || hC.wallWidth != prevWallWidht) { // same thing as above
			if (xDirInt < 0) // in other words: if it is side down, then set its position x value to be -wall width
			{ 
				x = hC.wallWidth * xDirInt; // this is needed because of pivot being on the side of a sprite
				// we use xDir because when the side is under the target we need to flip it as well
			} 
			else
			{ // if it's side up - do nothing
				x = 0;
			}
			myTransform.localPosition = new Vector3 (x, hC.sideSeparation / 2 * yDirIntOriginal); // applying the transformations
		}
	}

	void UpdatePrevious()
	{
		prevAngle = angle;
		xPrevDir = xDirInt;
		prevSideSeparation = hC.sideSeparation;
		prevWallWidht = hC.wallWidth;
	}
}
