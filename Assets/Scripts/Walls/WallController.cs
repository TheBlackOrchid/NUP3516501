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

	[Space(10)]
	public GameObject holePrefab;
	public GameObject fillerPrefab;
	[Space(10)]
	public Transform target;
	[Space(10)]
	public float wallWidth = 0.6f; // actual width of the wall
	public float wallHeight = 15f; // actual height of the wall
	public float wallActiveHeight = 10f; // the height that is available for the holes
	[Space(10)]
	public int holeCount = 2;

	private Transform myTransform;
	private Transform[] holes;
	private Transform[] fillers;
	private GameObject currSegment;
	private HeightRange activeRange;
	private HeightRange[] segments;

	// Use this for initialization
	void Start ()
	{
		myTransform = transform;
		activeRange = new HeightRange (wallHeight / 2, (wallHeight / 2) - wallActiveHeight); // the range representation of activeHeight

		CreateWall ();
		DevideIntoSegments ();
		AdjustHolePosition ();
	}

	void Update()
	{
		// test
		if (Debug.isDebugBuild) { 
			Debug.DrawRay (new Vector3 (myTransform.position.x, segments [0].up), Vector3.down * segments [0].height, Color.red);
			Debug.DrawRay (new Vector3 (myTransform.position.x, segments [1].up), Vector3.down * segments [1].height, Color.green);
			HoleController hC = holes [0].GetComponent<HoleController> ();
			Debug.DrawRay (new Vector3 (myTransform.position.x, hC.holeExtent.up), Vector3.down * hC.holeExtent.height, Color.cyan);
			hC = holes [1].GetComponent<HoleController> ();
			Debug.DrawRay (new Vector3 (myTransform.position.x, hC.holeExtent.up), Vector3.down * hC.holeExtent.height, Color.magenta);
		}
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
}
