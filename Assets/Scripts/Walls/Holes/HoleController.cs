using UnityEngine;
using System.Collections;

public class HoleController : MonoBehaviour {

	public const float PosToScale = 1.561754f;

	// public variables
	public WallController wallController;
	public Transform sideUp;
	public Transform sideDown;
	public float ballRadius = 0.64f;
	public float holeSize = 1f; // the thinest distance between hole sides;

	// public properties
	public Vector3 targetPosition { get; private set; } // ...with our offset;
	public HeightRange holeExtent { get; private set; } // the vertical boudaries of the hole
	public float wallWidth { get; private set; } // aka x
	public float sideSeparation { get; private set; } // vertical distance between two sides

	// private variables
	private Transform myTransform;
	private Transform target;
	private float targetOffset; // basically a half of target's height
	private float targetHeight; // the height of target's sprite
	private float targetAngle; // the euler z value of the target
	private float sideUpAngle; // angle of side up
	private float sideDownAngle; // angle of side down
	private float lowerOptimalAngle; // two angles within which the hole should correct itself
	private float upperOptimalAngle;

	// previous
	private Vector3 prevSideUpPosition;
	private Vector3 prevSideDownPosition;
	private Vector3 prevTargetPosition;
	private float prevTargetAngle;
	private float prevSideDownAngle;
	private float prevSideUpScaleY;

	void Start()
	{
		myTransform = transform;
		wallWidth = wallController.wallWidth;
		target = wallController.target;
		myTransform.localScale = Vector3.one; // whatever it was, it should be this
		if (myTransform.position.x > 0) { // if it is on right side
			myTransform.Translate (Vector3.right * wallWidth); // translate me one wall width to the right. it's because of pivot
		}
		targetHeight = target.gameObject.GetComponent<SpriteRenderer> ().bounds.extents.y;
		ChangeOffset (target.rotation.eulerAngles.z); // initial correction
	}

	void Update () 
	{
		if (targetPosition != prevTargetPosition
		    || sideUp.position != prevSideUpPosition
		    || sideDown.position != prevSideDownPosition) {
			sideDownAngle = Angle (sideDown.position - targetPosition);
			sideUpAngle = Angle (sideUp.position - targetPosition);

			lowerOptimalAngle = GetOptimalAngle (sideDownAngle);
			upperOptimalAngle = GetOptimalAngle (sideUpAngle);
		}
		AdjustTargetOffset ();
		AdjustHoleSize ();

		UpdatePrevious ();
	}

	void AdjustTargetOffset()
	{
		targetAngle = target.rotation.eulerAngles.z;
		if (targetAngle != prevTargetAngle) {
			bool angleWithin = false;
			if (lowerOptimalAngle < upperOptimalAngle) { // checking if the lower optimal angle is actually lower, not upper
				if (targetAngle > lowerOptimalAngle && targetAngle < upperOptimalAngle) { // checking if the target angle is withing optimals
					angleWithin = true;
				} else {
					angleWithin = false;
				}
			} else {
				if (targetAngle > upperOptimalAngle && targetAngle < lowerOptimalAngle) { // same thing, different order
					angleWithin = true;
				} else {
					angleWithin = false;
				}
			}
			if (angleWithin) {
				ChangeOffset (targetAngle); // correction
			}
		}
	}

	void ChangeOffset(float angle)
	{
		targetOffset = (targetHeight + ballRadius) / Mathf.Abs (Mathf.Cos (angle * Mathf.Deg2Rad)); // draw a triangle
		targetPosition = target.position + Vector3.up * targetOffset;
	}

	float GetOptimalAngle(float angle)
	{
		if (angle >= 0 && angle < 90) {
			return 360 + ((angle - 90) / 2); // trust me
		} else if (angle < 0) {
			return (angle + 90) / 2;
		}
		else {
			return (angle - 90) / 2;
		}
	}

	void AdjustHoleSize() // must be used in Update
	{
		if (sideDownAngle != prevSideDownAngle || sideUp.localScale.y != prevSideUpScaleY) {
			sideSeparation = (holeSize / Mathf.Abs (Mathf.Cos (sideDownAngle * Mathf.Deg2Rad))) + Mathf.Abs (sideUp.localScale.y / PosToScale); // don't ask why (better draw two right triangles)
			holeExtent = new HeightRange(myTransform.position.y + sideSeparation / 2, myTransform.position.y - sideSeparation / 2);
		}
	}

	void UpdatePrevious()
	{
		prevTargetAngle = targetAngle;
		prevTargetPosition = targetPosition;
		prevSideUpPosition = sideUp.position;
		prevSideDownPosition = sideDown.position;
		prevSideDownAngle = sideDownAngle;
		prevSideUpScaleY = sideUp.localScale.y;
	}

	public static float Angle(Vector3 vector) // parameter vector is a position vector
	{
		return Mathf.Atan(vector.y / vector.x) * Mathf.Rad2Deg; // an atangent of sine over cosine of an angle in radians converted to degrees
	}
}
