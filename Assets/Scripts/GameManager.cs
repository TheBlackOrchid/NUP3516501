using UnityEngine;

public class GameManager : MonoBehaviour
{

    public WallController[] walls = new WallController[2];
    public Spawn ballSpawn;
    public float ballSpeedStep = 1f;

    private float defaultBallSpeed;

    void Start()
    {
        defaultBallSpeed = ballSpawn.ballSpeed;
    }

    public void MoveHoles()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].MoveHolesToRandom();
        }
    }

    public void IncreaseBallSpeed()
    {
        ballSpawn.ballSpeed += ballSpeedStep;
    }

    public void ResetBallSpeed()
    {
        ballSpawn.ballSpeed = defaultBallSpeed;
    }
}
