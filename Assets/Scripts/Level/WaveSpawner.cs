using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum Direction { LINE, V, X };

    [System.Serializable]
    struct WaveData {
        [Header ("Enemy Data")]
        public Direction dir;
        public GameObject enemyPrefab;
        public Color color;

        [Header ("Wave Spawning")]
        public Vector3 spawnPoint;
        public int numEnemies;
        public float dist;
        public float angle;

        [Header ("Stacking")]
        public bool stack;
        public float numStacks;
        public float x_offset;
        public float z_offset;
    }

    // Wave data
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private bool pauseLevel = false;
    [SerializeField] private bool isCheckpoint = false;

    private List<GameObject> enemies;
    Camera cam;

    // Flags
    private bool spawned = false;
    private bool paused = false;
    private bool checkpoint = false;
    public bool IsSpawned { get { return spawned; } }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.WorldToScreenPoint(transform.position).y <= cam.pixelHeight && !spawned)
        {
            // Keep track of enemies
            enemies = new List<GameObject>();

            // Spawn all waves
            foreach (WaveData wave in waves)
                SpawnWave(wave);

            // Set flags
            spawned = true;
            if (!pauseLevel)
                checkpoint = true;
        }
        else if (!checkpoint && spawned && cam.WorldToScreenPoint(transform.position).y <= 0.0f)
        {
            if (!paused)
            {
                paused = true;
                transform.parent.GetComponent<LevelManager>().ToggleScrolling(false);
            }
            else if (CheckEnemies())
            {
                transform.parent.GetComponent<LevelManager>().ToggleScrolling(true);
                if (isCheckpoint)
                    transform.parent.GetComponent<LevelManager>().SetCheckpoint();

                checkpoint = true;
            }
        }
    }

    bool CheckEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) return false;
        }

        return true;
    }

    public void Reset()
    {
        spawned = false;
        paused = false;
        checkpoint = false;
    }

    // --------------
    // SPAWNING STUFF
    // --------------

    // Spawn enemy given the parameters
    void SpawnEnemy(WaveData wave, Vector3 position, Quaternion rotation)
    {
        GameObject e = Instantiate(wave.enemyPrefab, position, rotation);
        e.GetComponent<EnemyController>().SetLevelManager(transform.parent.GetComponent<LevelManager>());
        e.GetComponent<EnemyController>().SetDelay(Random.Range(0.0f, 2.0f));
        enemies.Add(e);
    }

    // Spawn given wave
    void SpawnWave(WaveData wave)
    {
        List<Vector3> spawnPoints = new List<Vector3>();
        Vector3 dir_1 = Vector3.zero;
        Vector3 dir_2 = Vector3.zero;
        float angle = wave.angle;

        switch (wave.dir)
        {
            case Direction.LINE:
                SpawnHelper(ref spawnPoints, wave.spawnPoint, Quaternion.Euler(0.0f, angle + 90.0f, 0.0f) * Vector3.forward, wave.numEnemies, wave.dist);
                if (wave.stack && wave.numStacks > 1)
                {
                    for (int i = 0; i < wave.numStacks; ++i)
                    {
                        Vector3 forward = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;
                        Vector3 right = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.right;
                        Vector3 spawnPoint = wave.spawnPoint + (right * wave.x_offset * (i % 2)) + (forward * wave.z_offset * i);
                        SpawnHelper(ref spawnPoints, spawnPoint, Quaternion.Euler(0.0f, angle + 90.0f, 0.0f) * Vector3.forward, wave.numEnemies, wave.dist);
                    }
                }
                break;
            case Direction.V:
                if (angle == 0) angle = 45.0f;
                dir_1 = Quaternion.Euler(new Vector3(0.0f, angle, 0.0f)) * Vector3.forward;
                dir_2 = Quaternion.Euler(new Vector3(0.0f, -angle, 0.0f)) * Vector3.forward;

                if (wave.numEnemies % 2 == 1)
                {
                    SpawnHelper(ref spawnPoints, wave.spawnPoint, dir_1, (wave.numEnemies / 2) + 1, wave.dist);
                    SpawnHelper(ref spawnPoints, wave.spawnPoint + (dir_2 * wave.dist), dir_2, wave.numEnemies / 2, wave.dist);
                }
                else
                {
                    Vector3 displacement = Vector3.right * (wave.dist / 2);
                    SpawnHelper(ref spawnPoints, wave.spawnPoint + displacement, dir_1, wave.numEnemies / 2, wave.dist);
                    SpawnHelper(ref spawnPoints, wave.spawnPoint - displacement, dir_2, wave.numEnemies / 2, wave.dist);
                }
                break;
            case Direction.X:
                if (angle == 0) angle = 45.0f;
                dir_1 = Quaternion.Euler(new Vector3(0.0f, angle, 0.0f)) * Vector3.forward;
                dir_2 = Quaternion.Euler(new Vector3(0.0f, -angle, 0.0f)) * Vector3.forward;

                SpawnHelper(ref spawnPoints, wave.spawnPoint, dir_1, wave.numEnemies + 1, wave.dist);
                SpawnHelper(ref spawnPoints, wave.spawnPoint + (dir_2 * wave.dist), dir_2, wave.numEnemies, wave.dist);
                SpawnHelper(ref spawnPoints, wave.spawnPoint - (dir_1 * wave.dist), -dir_1, wave.numEnemies, wave.dist);
                SpawnHelper(ref spawnPoints, wave.spawnPoint - (dir_2 * wave.dist), -dir_2, wave.numEnemies, wave.dist);
                break;
        }

        if (Application.isPlaying)
            foreach (Vector3 position in spawnPoints)
                SpawnEnemy(wave, position, Quaternion.Euler(0.0f,180f,0.0f));
        else if (Application.isEditor)
            foreach (Vector3 position in spawnPoints)
                Gizmos.DrawSphere(position, 0.5f);
    }

    void SpawnHelper(ref List<Vector3> spawnPoints, Vector3 spawnPoint, Vector3 dir, int numEnemies, float dist)
    {
        for (int i = 0; i < numEnemies; ++i)
            spawnPoints.Add(transform.position + spawnPoint + (dir * dist * i));
    }


    // ------------
    // EDITOR STUFF
    // ------------

    void OnDrawGizmos()
    {
        if (Application.isEditor)
            if (waves != null)
                foreach (WaveData wave in waves)
                {
                    Gizmos.color = wave.color;
                    if (wave.enemyPrefab)
                        SpawnWave(wave);
                }
    }

    void OnDrawGizmosSelected()
    {
        float x = transform.position.x;
        float z = transform.position.z;
        Vector3 p1 = new Vector3(-7.2f + x, 0f, 10.8f + z);
        Vector3 p2 = new Vector3(-7.2f + x, 0f, 0f + z);
        Vector3 p3 = new Vector3(7.2f + x, 0f, 0f + z);
        Vector3 p4 = new Vector3(7.2f + x, 0f, 10.8f + z);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p4, p1);
    }
}
