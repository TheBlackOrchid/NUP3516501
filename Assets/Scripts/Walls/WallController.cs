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
	public float wallHeight = 10f; // actual height of the wall
	public float wallActiveHeight = 7f; // the height that is available for the holes
	[Space(10)]
	public int holeCount = 2;

	private Transform myTransform;
	private Transform[] holes;
	private GameObject currSegment;
	private HeightRange activeRange;
	private HeightRange[] segments;

	// Use this for initialization
	void Start ()
	{
		myTransform = transform;
		activeRange = new HeightRange (wallHeight / 2, (wallHeight / 2) - wallActiveHeight);
		CreateWall ();
		DevideIntoSegments ();
		AdjustHolePosition ();
	}

	void CreateWall()
	{
		holes = new Transform[holeCount];

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
			currSegment.transform.parent = myTransform;
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
			holes [i].localPosition = Vector3.up * segments [i].center;
		}
	}
}
