﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputHandler))]

public class StickController : MonoBehaviour {

    //public variables

    //private variables
    private Rigidbody2D rb;
    private InputHandler iH;
    private Vector2 centerPos;
    private Vector2 cursorPos;
    private Vector3 positionVector;
    private float angle;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        iH = GetComponent<InputHandler>();
        centerPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (iH.onStick)
        {
            cursorPos = iH.cursorPos; //getting mouse or touch position
            positionVector = (cursorPos - centerPos).normalized; // a normalized vector from center of the stick pointed to the cursor position each coordinate of which being cosine and sine respectively
            angle =  Mathf.Atan(positionVector.y / positionVector.x) * Mathf.Rad2Deg; // an atangent of sine over cosine of an angle in radians converted to degrees
            //---test---//
            Debug.DrawRay(centerPos, positionVector);
        }
	}

    void FixedUpdate()
    {
        if (iH.onStick) { rb.MoveRotation(angle); } //rotation
    }
}
