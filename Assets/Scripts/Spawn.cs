using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{

    public GameObject ballPrefab;
    //ball prefab
    public ScoreController scoreController;
    public bool spawnEnabled = true;
    public int poolSize = 2;
    public float spawnRate = 0.5f;
    public float ballSpeed;

    private GameObject ballPool;
    private GameObject currBall;
    private Transform myTransform;
    private Rigidbody2D currBallRb;
    private BallController currBallController;
    private BallController[] balls;
    private int currBallIndex;
    private bool canSpawn = true;

    // Use this for initialization
    void Start()
    {
        balls = new BallController[poolSize];
        CreatePool("BallPool");
        myTransform = transform;
        currBallIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnEnabled && canSpawn)
        {
            StartCoroutine(SpawnCoroutine());
        } // running the coroutine
    }

    IEnumerator SpawnCoroutine()
    {
        // initialization
        canSpawn = false;
        currBall = ballPool.transform.GetChild(currBallIndex).gameObject;

        if (!currBall.activeInHierarchy)
        {
            currBallRb = currBall.GetComponent<Rigidbody2D>();
            currBall.SetActive(true);

            // transformation
            currBall.transform.position = myTransform.position;
            currBall.transform.rotation = myTransform.rotation;
            currBallRb.angularVelocity = 0;
            currBallRb.velocity = Vector2.down * ballSpeed;

            // cooldown
            yield return new WaitForSeconds(spawnRate);
        }

        // ready to go
        if (currBallIndex < poolSize - 1)
        {
            currBallIndex++;
        }
        else
        {
            currBallIndex = 0;
        } // if ran out of balls - take the first
        canSpawn = true;

    }

    public void KillAll()
    {
        foreach (var b in balls)
        {
            b.Kill();
        }
    }

    void CreatePool(string name)
    {
        ballPool = new GameObject(); // creating ball pool
        ballPool.name = name; // naming it
        ballPool.transform.position = Vector3.up * 10; // setting its position

        balls = new BallController[poolSize];

        for (int i = 0; i < poolSize; i++) // populating pool
        {
            currBall = (GameObject)Instantiate(ballPrefab); // new ball from prefab
            currBall.transform.SetParent(ballPool.transform); // parenting to pool
            currBall.transform.localPosition = Vector3.zero; // teleporting ball to pool's position
            currBallController = currBall.GetComponent<BallController>();
            currBallController.scoreController = scoreController;
            balls[i] = currBallController;
            // used to kill itself here. now does it after a while
        }
        currBall = null; // must be null for further actions (in case)
    }
}
