using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

    public GameObject ball;
    public float spawnRate = 0.5f;
    public float ballSpeed;

    private Rigidbody2D ballRb;
    private bool canSpawn = true;

	// Use this for initialization
	void Start () {
        ballRb = ball.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canSpawn) { StartCoroutine(SpawnCoroutine()); }
    }

    IEnumerator SpawnCoroutine()
    {
        canSpawn = false;
        ball.transform.position = transform.position;
        ball.transform.rotation = transform.rotation;
        ballRb.angularVelocity = 0;
        ballRb.velocity = Vector2.down * ballSpeed;
        Debug.Log("spawned");
        yield return new WaitForSeconds(spawnRate);
        canSpawn = true;
    }

}
