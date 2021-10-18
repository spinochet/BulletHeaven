using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class WaveSpawner3D : NetworkBehaviour
{

    struct DirData {
        public Vector3 dir;
        public bool diag;
        public Vector3 dir2;
    }

    public GameObject _enemyPrefab;

    public List<Vector3> _spawnPoints;

    public Vector3 _spawnPoint;
    public int numShifts;
    public float dist;
    public float rowDist;
    public enum Direction { FORWARD, RIGHT, DIAG, V_ANGLE, X };
    public int numRows = 1;

    public bool stagger;

    public Direction dir;

    void Start()
    {


    }

    private DirData GetDir(int val = 0) {
        Vector3 _dir = Vector3.zero;
        Vector3 _dir2 = Vector3.zero;
        bool diag = false;
        switch (dir) {
            case Direction.FORWARD:
                _dir = Vector3.forward * dist;
                break;
            case Direction.RIGHT:
                _dir = Vector3.right * dist;
                break;
            case Direction.DIAG:
                diag = true;
                _dir = (Vector3.right * Mathf.Sign(dist) + Vector3.forward).normalized * Mathf.Abs(dist);
                break;
            case Direction.V_ANGLE:
                if (val < 0) {
                    _dir = (Vector3.left * Mathf.Sign(val) - Vector3.forward).normalized * dist;
                } else {
                    _dir = (Vector3.right * Mathf.Sign(val) + Vector3.forward).normalized * dist;
                }
                break;
            case Direction.X:
                if (val < 0) {
                    _dir = (Vector3.left * Mathf.Sign(val) - Vector3.forward).normalized * dist;
                    _dir2 = (Vector3.left * Mathf.Sign(val) - Vector3.forward).normalized * -dist;
                } else if (val > 0) {
                    _dir = (Vector3.right * Mathf.Sign(val) + Vector3.forward).normalized * dist;
                    _dir2 = (Vector3.right * Mathf.Sign(val) + Vector3.forward).normalized * -dist;
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

        if (transform.position.z <= 0) {
            if (isServer)
            {
                transform.position = Vector3.zero;

                if (dir == Direction.V_ANGLE) {

                    // I would advise making the V angle have an 
                    // odd number of enemies
                    int target_offset = numShifts / 2;
                    int e_offset = -target_offset;

                    for (; e_offset <= target_offset; ++e_offset) {
                        DirData data = GetDir(e_offset);

                        Vector3 sp = Vector3.zero;
                        sp = (_spawnPoint + data.dir * e_offset);

                        GameObject e = Instantiate(_enemyPrefab, sp, Quaternion.Euler(0.0f,180f,0.0f));
                        NetworkServer.Spawn(e);
                    }

                } else if (dir == Direction.X) {
                    int target_offset = numShifts / 2;
                    int e_offset = -target_offset;

                    for (; e_offset <= target_offset; ++e_offset) {
                        DirData data = GetDir(e_offset);

                        Vector3 sp1 = Vector3.zero;
                        sp1 = (_spawnPoint + data.dir * e_offset);
                        GameObject e = Instantiate(_enemyPrefab, sp1, Quaternion.Euler(0.0f,180f,0.0f));
                        NetworkServer.Spawn(e);

                        if (e_offset != 0) {
                            Vector3 sp2 = Vector3.zero;
                            sp2 = (_spawnPoint + data.dir2 * e_offset);
                            GameObject e2 = Instantiate(_enemyPrefab, sp2, Quaternion.Euler(0.0f,180f,0.0f));
                            NetworkServer.Spawn(e2);
                        }
                    }


                } else {
                    for (int j = 0; j < numRows; ++j) {
                        int staggerOffset = 0;
                        int staggerFactor = 0;
                        if (j % 2 != 0 && stagger) {
                            staggerFactor = 1;
                            staggerOffset = -1;
                        }

                        for (int i = 0; i < numShifts + staggerOffset; ++i) {
                            DirData data = GetDir();

                            Vector3 sp = Vector3.zero;
                            if (data.diag) {
                                sp = (_spawnPoint + data.dir * i) + (Vector3.forward * rowDist * j) + (data.dir * 0.5f * staggerFactor);
                            } else {
                                sp = (_spawnPoint + data.dir * i) + (Vector3.forward * rowDist * j) + (Vector3.right * dist * 0.5f * staggerFactor);
                            }
                            GameObject e = Instantiate(_enemyPrefab, sp, Quaternion.Euler(0.0f,180f,0.0f));
                            NetworkServer.Spawn(e);
                        }
                    }
                }
            }
            
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;


        if (dir == Direction.V_ANGLE) {
            // I would advise making the V angle have an 
            // odd number of enemies
            int target_offset = numShifts / 2;
            int e_offset = -target_offset;

            for (; e_offset <= target_offset; ++e_offset) {
                DirData data = GetDir(e_offset);
                Vector3 sphereSpawn = Vector3.zero;
                sphereSpawn = (_spawnPoint + transform.position + data.dir * e_offset);
                Gizmos.DrawSphere(sphereSpawn, 0.5f);
            }
        } else if (dir == Direction.X) {
            int target_offset = numShifts / 2;
            int e_offset = -target_offset;
            for (; e_offset <= target_offset; ++e_offset) {
                DirData data = GetDir(e_offset);

                Vector3 sphereSpawn1 = Vector3.zero;
                Vector3 sphereSpawn2 = Vector3.zero;

                sphereSpawn1 = (_spawnPoint + transform.position + data.dir * e_offset);
                Gizmos.DrawSphere(sphereSpawn1, 0.5f);
                if (e_offset != 0) {
                    sphereSpawn2 = (_spawnPoint + transform.position + data.dir2 * e_offset);
                    Gizmos.DrawSphere(sphereSpawn2, 0.5f);
                }
            }
        } else {

            for (int j = 0; j < numRows; ++j) {
                int staggerOffset = 0;
                int staggerFactor = 0;
                if (j % 2 != 0 && stagger) {
                    staggerFactor = 1;
                    staggerOffset = -1;
                }
                for (int i = 0; i < numShifts + staggerOffset; ++i) {
                    DirData data = GetDir();
                    Vector3 sphereSpawn = Vector3.zero;
                    if (data.diag) {
                        sphereSpawn = (_spawnPoint + transform.position + data.dir * i) + (Vector3.forward * rowDist * j) + (data.dir * 0.5f * staggerFactor);
                    } else {
                        sphereSpawn = (_spawnPoint + transform.position + data.dir * i) + (Vector3.forward * rowDist * j) + (Vector3.right * dist * 0.5f * staggerFactor);
                    }
                    
                    Gizmos.DrawSphere(sphereSpawn, 0.5f);
                }
            }
        }
    }
    void OnDrawGizmosSelected(){
        float x = transform.position.x;
        float z = transform.position.z;
        Vector3 p1 = new Vector3(-7.2f + x, 0f, 5.4f + z);
        Vector3 p2 = new Vector3(-7.2f + x, 0f, -5.4f + z);
        Vector3 p3 = new Vector3(7.2f + x, 0f, -5.4f + z);
        Vector3 p4 = new Vector3(7.2f + x, 0f, 5.4f + z);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);

    }
}
