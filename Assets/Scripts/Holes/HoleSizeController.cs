using UnityEngine;
using System.Collections;

public class HoleSizeController : MonoBehaviour {

	public enum Direction { Up,Down }
	public Direction direction;

	private HoleController hC;
	private Transform myTransform;
	private int dirInt;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		hC = myTransform.parent.GetComponent<HoleController> ();

		switch (direction)
		{
		case Direction.Up:
			dirInt = 1;
			break;
		case Direction.Down:
			dirInt = -1;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.position = myTransform.parent.position + Vector3.up * (hC.sideSeparation / 2) * dirInt;
	}
}
