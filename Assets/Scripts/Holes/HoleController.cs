using UnityEngine;
using System.Collections;

public class HoleController : MonoBehaviour {

	// public variables
	public Transform target;
	public Transform sideDown;
	public float holeSize = 1f; // the thinest distance between hole sides;

	// public properties
	public Vector3 targetPosition { get; private set; } // ...with our offset;
	public float sideSeparation { get; private set; } // vertical distance between two sides

	// private variables
	private Transform myTransform;
	private Transform[] sides;
	private Transform[] fillers;
	private Vector3 targetOffset; // basically a half of target's height


	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
		targetOffset = new Vector3 (0, target.gameObject.GetComponent<SpriteRenderer> ().bounds.extents.y);
		targetPosition = target.position + targetOffset;
	}

	void Update () 
	{
		AdjustHoleSize ();
	}

	void AdjustHoleSize() // must be used in Update
	{
		sideSeparation = holeSize / Mathf.Abs (Mathf.Cos (sideDown.localRotation.eulerAngles.z * Mathf.Deg2Rad)); // don't ask why
	}
}
