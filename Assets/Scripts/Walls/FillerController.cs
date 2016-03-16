using UnityEngine;
using System.Collections;

public enum Position { Up, Center, Down}

public class FillerController : MonoBehaviour {
	
	public WallController wallController;
	public Transform upperHole; // can be null
	public Transform lowerHole; // can be null
	public Position position; // enum
	public float offset = 0.001f; // to avoid holes between sprites

	private Transform myTransform;
	private HoleController uHHC; // upper hole HoleController
	private HoleController lHHC; // lower hole HoleController
	private HeightRange ownSegment; // height range
	private float wallWidth;
	private float wallVerticalExtent; // the half of the wall height
	private int xDir; // to determine if the filler is on the right

	// previous
	private HeightRange prevOwnSegment;
	private float prevWallWidth;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		wallVerticalExtent = wallController.wallHeight / 2;

		if (upperHole != null) {
			uHHC = upperHole.GetComponent<HoleController> ();
		}
		if (lowerHole != null) {
			lHHC = lowerHole.GetComponent<HoleController> ();
		}

		xDir = (int)-Mathf.Sign (myTransform.position.x);

		wallWidth = wallController.wallWidth;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBoundaries ();
		AdjustSize ();
		UpdatePrevious ();
	}

	void UpdateBoundaries () // need optimizations
	{
		switch (position) { // building height ranges for all positions
		case Position.Up:
			ownSegment = new HeightRange (myTransform.parent.position.y + wallVerticalExtent, lHHC.holeExtent.up - offset);
			break;
		case Position.Center:
			ownSegment = new HeightRange (uHHC.holeExtent.down + offset, lHHC.holeExtent.up - offset);
			break;
		case Position.Down:
			ownSegment = new HeightRange (uHHC.holeExtent.down + offset, myTransform.parent.position.y - wallVerticalExtent);
			break;
		}
	}

	void AdjustSize()
	{
		if (ownSegment.up != prevOwnSegment.up || ownSegment.down != prevOwnSegment.down || wallWidth != prevWallWidth) { // if something has changed
			myTransform.localPosition = Vector3.up * ownSegment.center; // then put yourself in center of you segment
			myTransform.localScale = new Vector3 (wallWidth * HoleController.PosToScale * xDir, ownSegment.height * HoleController.PosToScale, 1); // and scale appropriately
		}
	}

	void UpdatePrevious () // update previous
	{
		prevWallWidth = wallWidth;
		prevOwnSegment = ownSegment;
	}
}
