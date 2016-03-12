using UnityEngine;
using System.Collections;

public class HoleController : MonoBehaviour {

	public const float PosToScale = 1.561754f;

	// public variables
	public Transform target;
	public Transform sideUp;
	public Transform sideDown;
	public float holeSize = 1f; // the thinest distance between hole sides;
	public float wallWidth = 0.6f; // aka x

	// public properties
	public Vector3 targetPosition { get; private set; } // ...with our offset;
	public float sideSeparation { get; private set; } // vertical distance between two sides

	// private variables
	private Vector3 targetOffset; // basically a half of target's height
	private float angle; //angle of side down

	// Use this for initialization
	void Start () 
	{
		targetOffset = new Vector3 (0, target.gameObject.GetComponent<SpriteRenderer> ().bounds.extents.y);
		targetPosition = target.position + targetOffset;
	}

	void Update () 
	{
		angle = Angle(sideDown.position - targetPosition);
		AdjustHoleSize ();
	}

	void AdjustHoleSize() // must be used in Update
	{
		sideSeparation = (holeSize / Mathf.Abs (Mathf.Cos (angle * Mathf.Deg2Rad))) + Mathf.Abs (sideUp.localScale.y / PosToScale); // don't ask why (better draw two right triangles)
	}	

	public static float Angle(Vector3 vector) // parameter vector is a position vector
	{
		return Mathf.Atan(vector.y / vector.x) * Mathf.Rad2Deg; // an atangent of sine over cosine of an angle in radians converted to degrees
	}
}
