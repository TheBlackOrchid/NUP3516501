using UnityEngine;

public class GameManager : MonoBehaviour
{

    public WallController[] walls = new WallController[2];
    public Spawn ballSpawn;
    public float ballSpeedStep = 1f;
    public float ballSpawnRateStep = 0.1f;
    public float minBallSpawnRate = 0.1f;
    public bool closeHoles;

    private float defaultBallSpeed;
    private float defaultBallSpawnRate;

    void Start()
    {
        defaultBallSpeed = ballSpawn.ballSpeed;
        defaultBallSpawnRate = ballSpawn.spawnRate;
    }

    public void MoveHoles()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].MoveHolesToRandom();
        }
    }

    public void CloseHoles()
    {
        if (closeHoles)
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].CloseRandomHole();
            }
        }
    }

    public void IncreaseDifficulty()
    {
        ballSpawn.ballSpeed += ballSpeedStep;
        if (ballSpawn.spawnRate > minBallSpawnRate)
        {
            ballSpawn.spawnRate -= ballSpawnRateStep;
        }
    }

    public void ResetDifficulty()
    {
        ballSpawn.ballSpeed = defaultBallSpeed;
        ballSpawn.spawnRate = defaultBallSpawnRate;
    }
}
