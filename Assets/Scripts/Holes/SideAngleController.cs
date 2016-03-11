using UnityEngine;
using System.Collections;

public class SideAngleController : MonoBehaviour {

	private HoleController hC;
	private Transform myTransform;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		hC = myTransform.parent.GetComponent<HoleController> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		myTransform.LookAt (hC.targetPosition);
		myTransform.Rotate (Vector3.up, -90);
	}
}
