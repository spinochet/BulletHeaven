using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner3D : MonoBehaviour
{

    struct DirData {
        public Vector3 dir;
        public bool diag;
        public Vector3 dir2;
    }

    public enum Direction { FORWARD, RIGHT, DIAG, V_ANGLE, X };

    [System.Serializable]
    struct WaveData {
        public GameObject _enemyPrefabs;

        public Vector3 _spawnPoint;
        public int numShifts;
        public float dist;
        public float rowDist;
        public int numRows;

        public Direction dir;
        public bool stagger;
    }

    [SerializeField] private List<WaveData> waves;
    public bool pauseLevel;
    [SerializeField] private bool isCheckpoint = false;

    private List<GameObject> enemies;

    Camera cam;
    private bool spawned = false;
    private bool paused = false;
    private bool checkpoint = false;
    public bool IsSpawned { get { return spawned; } }

    void Start()
    {
        cam = Camera.main;
    }

    private DirData GetDir(WaveData wave, int val = 0) {
        Vector3 _dir = Vector3.zero;
        Vector3 _dir2 = Vector3.zero;
        bool diag = false;
        switch (wave.dir) {
            case Direction.FORWARD:
                _dir = Vector3.forward * wave.dist;
                break;
            case Direction.RIGHT:
                _dir = Vector3.right * wave.dist;
                break;
            case Direction.DIAG:
                diag = true;
                _dir = (Vector3.right * Mathf.Sign(wave.dist) + Vector3.forward).normalized * Mathf.Abs(wave.dist);
                break;
            case Direction.V_ANGLE:
                if (val < 0) {
                    _dir = (Vector3.left * Mathf.Sign(val) - Vector3.forward).normalized * wave.dist;
                } else {
                    _dir = (Vector3.right * Mathf.Sign(val) + Vector3.forward).normalized * wave.dist;
                }
                break;
            case Direction.X:
                if (val < 0) {
                    _dir = (Vector3.left * Mathf.Sign(val) - Vector3.forward).normalized * wave.dist;
                    _dir2 = (Vector3.left * Mathf.Sign(val) - Vector3.forward).normalized * -wave.dist;
                } else if (val > 0) {
                    _dir = (Vector3.right * Mathf.Sign(val) + Vector3.forward).normalized * wave.dist;
                    _dir2 = (Vector3.right * Mathf.Sign(val) + Vector3.forward).normalized * -wave.dist;
                }
                break;
        }

        DirData data;
        data.dir = _dir;
        data.dir2 = _dir2;
        data.diag = diag;

        return data;
    }

    void Update()
    {
        if (cam.WorldToScreenPoint(transform.position).y <= cam.pixelHeight && !spawned)
        {
            enemies = new List<GameObject>();

            foreach (WaveData wave in waves)
                SpawnWave(wave);

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

    public void Reset()
    {
        spawned = false;
        paused = false;
        checkpoint = false;
    }

    void SpawnEnemy(WaveData wave, Vector3 position, Quaternion rotation)
    {
        GameObject e = Instantiate(wave._enemyPrefabs, position, rotation);
        e.GetComponent<EnemyController>().SetLevelManager(transform.parent.GetComponent<LevelManager>());
        e.GetComponent<EnemyController>().SetDelay(Random.Range(0.0f, 2.0f));
        enemies.Add(e);
    }

    void SpawnWave(WaveData wave)
    {
        spawned = true; // Comment this line for bug

        if (wave.dir == Direction.V_ANGLE)
        {
            // I would advise making the V angle have an 
            // odd number of enemies
            int target_offset = wave.numShifts / 2;
            int e_offset = -target_offset;

            for (; e_offset <= target_offset; ++e_offset)
            {
                DirData data = GetDir(wave, e_offset);

                Vector3 sp = Vector3.zero;
                sp = (wave._spawnPoint + data.dir * e_offset);

                SpawnEnemy(wave, sp + transform.position, Quaternion.Euler(0.0f,180f,0.0f));
            }

        }
        else if (wave.dir == Direction.X)
        {
            int target_offset = wave.numShifts / 2;
            int e_offset = -target_offset;

            for (; e_offset <= target_offset; ++e_offset) {
                DirData data = GetDir(wave, e_offset);

                Vector3 sp1 = Vector3.zero;
                sp1 = (wave._spawnPoint + data.dir * e_offset);

                SpawnEnemy(wave, sp1 + transform.position, Quaternion.Euler(0.0f,180f,0.0f));

                if (e_offset != 0) {
                    Vector3 sp2 = Vector3.zero;
                    sp2 = (wave._spawnPoint + data.dir2 * e_offset);

                    SpawnEnemy(wave, sp2 + transform.position, Quaternion.Euler(0.0f,180f,0.0f));
                }
            }
        }
        else 
        {
            for (int j = 0; j < wave.numRows; ++j)
            {
                int staggerOffset = 0;
                int staggerFactor = 0;
                if (j % 2 != 0 && wave.stagger)
                {
                    staggerFactor = 1;
                    staggerOffset = -1;
                }

                for (int i = 0; i < wave.numShifts + staggerOffset; ++i)
                {
                    DirData data = GetDir(wave);

                    Vector3 sp = Vector3.zero;
                    if (data.diag)
                    {
                        sp = (wave._spawnPoint + data.dir * i) + (Vector3.forward * wave.rowDist * j) + (data.dir * 0.5f * staggerFactor);
                    }
                    else
                    {
                        sp = (wave._spawnPoint + data.dir * i) + (Vector3.forward * wave.rowDist * j) + (Vector3.right * wave.dist * 0.5f * staggerFactor);
                    }

                    SpawnEnemy(wave, sp + transform.position, Quaternion.Euler(0.0f,180f,0.0f));
                }
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (WaveData wave in waves)
        {
            if (wave.dir == Direction.V_ANGLE) {
                // I would advise making the V angle have an 
                // odd number of enemies
                int target_offset = wave.numShifts / 2;
                int e_offset = -target_offset;

                for (; e_offset <= target_offset; ++e_offset) {
                    DirData data = GetDir(wave, e_offset);
                    Vector3 sphereSpawn = Vector3.zero;
                    sphereSpawn = (wave._spawnPoint + transform.position + data.dir * e_offset);
                    Gizmos.DrawSphere(sphereSpawn, 0.5f);
                }
            } else if (wave.dir == Direction.X) {
                int target_offset = wave.numShifts / 2;
                int e_offset = -target_offset;
                for (; e_offset <= target_offset; ++e_offset)
                {
                    DirData data = GetDir(wave, e_offset);

                    Vector3 sphereSpawn1 = Vector3.zero;
                    Vector3 sphereSpawn2 = Vector3.zero;

                    sphereSpawn1 = (wave._spawnPoint + transform.position + data.dir * e_offset);
                    Gizmos.DrawSphere(sphereSpawn1, 0.5f);
                    if (e_offset != 0)
                    {
                        sphereSpawn2 = (wave._spawnPoint + transform.position + data.dir2 * e_offset);
                        Gizmos.DrawSphere(sphereSpawn2, 0.5f);
                    }
                }
            } else {

                for (int j = 0; j < wave.numRows; ++j)
                {
                    int staggerOffset = 0;
                    int staggerFactor = 0;
                    if (j % 2 != 0 && wave.stagger)
                    {
                        staggerFactor = 1;
                        staggerOffset = -1;
                    }
                    for (int i = 0; i < wave.numShifts + staggerOffset; ++i)
                    {
                        DirData data = GetDir(wave);
                        Vector3 sphereSpawn = Vector3.zero;
                        if (data.diag)
                        {
                            sphereSpawn = (wave._spawnPoint + transform.position + data.dir * i) + (Vector3.forward * wave.rowDist * j) + (data.dir * 0.5f * staggerFactor);
                        }
                        else
                        {
                            sphereSpawn = (wave._spawnPoint + transform.position + data.dir * i) + (Vector3.forward * wave.rowDist * j) + (Vector3.right * wave.dist * 0.5f * staggerFactor);
                        }
                        
                        Gizmos.DrawSphere(sphereSpawn, 0.5f);
                    }
                }
            }
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
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
}
