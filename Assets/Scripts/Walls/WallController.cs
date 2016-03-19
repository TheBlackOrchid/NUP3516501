using UnityEngine;
using System.Collections;

public struct HeightRange {
	public float up; // y coordinate of the upper range bound
	public float down; // y coordinate of the lower range bound
	public float center; // the center of the range
	public float height; // sort of magnitude

	public HeightRange(float _up, float _down){
		up = _up;
		down = _down;
		center = (up + down) / 2;
		height = up - down;
	}
}

public class WallController : MonoBehaviour {

	//[Space(10)]
	[Header("Prefabs")]
	[Tooltip("The hole that will be created")]
	public GameObject holePrefab;
	[Tooltip("The filler that will be created")]
	public GameObject fillerPrefab;

	//[Space(10)]
	[Header("Target")]
	[Tooltip("The target at which all holes are facing")]
	public Transform target;

	//[Space(10)]
	[Header("Wall parameters")]
	[Tooltip("The width of the wall. Also how thick it appears on screen")]
	public float wallWidth = 0.6f; // actual width of the wall
	[Tooltip("The full height of the wall")]
	public float wallHeight = 15f; // actual height of the wall
	[Tooltip("The height that is available for the holes")]
	public float wallActiveHeight = 10f; // the height that is available for the holes

	//[Space(10)]
	[Header("Holes parameters")]
	[Tooltip("The number of the beast... ghrhm... I mean holes")]
	public int holeCount = 2;
	[Tooltip("The initial size of the holes")]
	public float holeSize = 3;

	private Transform myTransform;
	private Transform[] holes;
	private Transform[] fillers;
	private GameObject currSegment;
	private HeightRange activeRange;
	private HeightRange[] segments;
	private HeightRange[] activeSegments;
	private float horScreenEdge;
	private int xDir;

	// Use this for initialization
	void Start ()
	{
		myTransform = transform;
		xDir = (int)Mathf.Sign (myTransform.position.x);
		activeRange = new HeightRange (wallHeight / 2, (wallHeight / 2) - wallActiveHeight); // the range representation of activeHeight

		StickToSide ();
		CreateWall ();
		DevideIntoSegments ();
		AdjustHolePosition ();
		GetActiveHolesSegments ();
	}

	void StickToSide ()
	{
		horScreenEdge = Camera.main.ScreenToWorldPoint (Vector3.right * Screen.width).x;
		myTransform.position = Vector3.right * (horScreenEdge - wallWidth) * xDir;
	}

	void CreateWall()
	{
		holes = new Transform[holeCount];
		fillers = new Transform[holeCount + 1]; // there are always one more filler than the holes

		for (int i = 0; i < holeCount; i++)  // creating holes
		{
			currSegment = (GameObject)Instantiate (holePrefab, myTransform.position, myTransform.rotation);
			currSegment.GetComponent<HoleController> ().wallController = this;
			currSegment.transform.parent = myTransform;
			holes [i] = currSegment.transform;
		}
		for (int i = 0; i <= holeCount; i++)  // creating fillers
		{
			currSegment = (GameObject)Instantiate (fillerPrefab, myTransform.position, myTransform.rotation);
			FillerController fC = currSegment.GetComponent<FillerController> ();
			fC.wallController = this;

			if (i > 0 && i < holeCount) { // if it's not the first or the last
				fC.position = Position.Center; // then set its position (enum) to center
				fC.upperHole = holes [i - 1]; // set the upper
				fC.lowerHole = holes [i]; // and lower holes
			} else if (i == holeCount) { // if it's the last 
				fC.position = Position.Down; // then set its position (enum) to down
				fC.upperHole = holes [i - 1]; // and set only the upper hole
			} else { // here the opposite thing
				fC.position = Position.Up;
				fC.lowerHole = holes [0];
			}

			currSegment.transform.parent = myTransform;
			fillers [i] = currSegment.transform;
		}
	}

	void DevideIntoSegments()
	{
		segments = new HeightRange[holeCount];

		float segmentHeight = wallActiveHeight / holeCount;
		float currUpper = activeRange.up;
		float currLower = currUpper - segmentHeight;
		for (int i = 0; i < holeCount; i++) 
		{
			segments[i] = new HeightRange (currUpper, currLower);
			currUpper = currLower;
			currLower = currUpper - segmentHeight;
		}
	}

	void AdjustHolePosition()
	{
		for (int i = 0; i < holeCount; i++) 
		{
			holes [i].localPosition = Vector3.up * segments [i].center; // positioning the holes in center of their segments
		}
	}

	void GetActiveHolesSegments()
	{
		activeSegments = new HeightRange[segments.Length];
		for (int i = 0; i < segments.Length; i++)
		{
			HoleController hC = holes [i].GetComponent<HoleController> ();
			float upper = hC.GetHoleHeighAtPosition (segments [i].up - (hC.holeSize * 0.8f));
			float lower = hC.GetHoleHeighAtPosition (segments [i].down + (hC.holeSize * 0.8f));
			activeSegments[i] = new HeightRange(segments[i].up - upper / 2, segments[i].down + lower / 2);
		}
	}

	[ContextMenu("Stick to up")]
	void MoveUp()
	{
		for (int i = 0; i < holeCount; i++) {
			holes [i].position = new Vector3 (holes [i].position.x, activeSegments [i].up, holes [i].position.z);
		}
	}

	[ContextMenu("Stick to down")]
	void MoveDown()
	{
		for (int i = 0; i < holeCount; i++) {
			holes [i].position = new Vector3 (holes [i].position.x, activeSegments [i].down, holes [i].position.z);
		}
	}
}
