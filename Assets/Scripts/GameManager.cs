using UnityEngine;

public class GameManager : MonoBehaviour
{

    public WallController[] walls = new WallController[2];
    public Spawn ballSpawn;
    public float ballSpeedStep = 1f;
    public float ballSpawnRateStep = 0.1f;
    public float minBallSpawnRate = 0.1f;
    public float holeCloseChanceStep = 0.01f;
    public bool closeHoles;

    private float defaultBallSpeed;
    private float defaultBallSpawnRate;
    private float defaultHoleCloseChance;

    private int hasClosedHolesIndex;
    private bool hasClosedHoles;

    void Start()
    {
        defaultBallSpeed = ballSpawn.ballSpeed;
        defaultBallSpawnRate = ballSpawn.spawnRate;
        defaultHoleCloseChance = walls[0].holeCloseChance;
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
            hasClosedHoles = CheckClosed();
            if (hasClosedHoles)
            {
                walls[hasClosedHolesIndex].OpenHole();
            }
            for (int i = 0; i < walls.Length; i++)
            {
                if (i == hasClosedHolesIndex) continue;

                walls[i].CloseRandomHole();
            }
        }
    }

    bool CheckClosed()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i].closed)
            {
                hasClosedHolesIndex = i;
                return true;
            }
        }
        return false;
    }

    public void IncreaseDifficulty()
    {
        ballSpawn.ballSpeed += ballSpeedStep;
        if (ballSpawn.spawnRate > minBallSpawnRate)
        {
            ballSpawn.spawnRate -= ballSpawnRateStep;
        }
        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i].holeCloseChance < 1)
            {
                walls[i].holeCloseChance += holeCloseChanceStep;
            }
        }

    }

    public void ResetDifficulty()
    {
        ballSpawn.ballSpeed = defaultBallSpeed;
        ballSpawn.spawnRate = defaultBallSpawnRate;
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].holeCloseChance = defaultHoleCloseChance;
        }
    }
}
