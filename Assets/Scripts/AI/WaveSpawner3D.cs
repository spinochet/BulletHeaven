using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner3D : MonoBehaviour
{
    public GameObject _enemyPrefab;

    public List<Vector3> _spawnPoints;

    public Vector3 _spawnPoint;
    public int numShifts;
    public float dist;
    public float rowDist;
    public enum Direction { FORWARD, RIGHT, DIAG, V_ANGLE };
    public int numRows = 1;

    public bool stagger;

    public Direction dir;

    void Start()
    {


    }

    void Update()
    {

        if (transform.position.z <= 0) {
            transform.position = Vector3.zero;

            if (dir == Direction.V_ANGLE) {

                // I would advise making the V angle have an 
                // odd number of enemies
                int target_offset = numShifts / 2;
                int e_offset = -target_offset;

                for (; e_offset <= target_offset; ++e_offset) {
                    Vector3 _dir = Vector3.zero;
                    if (e_offset < 0) {
                        _dir = (Vector3.left * Mathf.Sign(e_offset) - Vector3.forward).normalized * dist;
                    } else {
                        _dir = (Vector3.right * Mathf.Sign(e_offset) + Vector3.forward).normalized * dist;
                    }
                    

                    Vector3 sp = Vector3.zero;

                    sp = (_spawnPoint + _dir * e_offset);
                    Instantiate(_enemyPrefab, sp, Quaternion.Euler(0.0f,180f,0.0f));
                }

            } else {
                for (int j = 0; j < numRows; ++j) {
                    int staggerOffset = 0;
                    int staggerFactor = 0;
                    bool diag = false;
                    if (j % 2 != 0 && stagger) {
                        staggerFactor = 1;
                        staggerOffset = -1;
                    }

                    for (int i = 0; i < numShifts + staggerOffset; ++i) {
                        Vector3 _dir = Vector3.zero;

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
                        }

                        Vector3 sp = Vector3.zero;
                        if (diag) {
                            sp = (_spawnPoint + _dir * i) + (Vector3.forward * rowDist * j) + (_dir * 0.5f * staggerFactor);
                        } else {
                            sp = (_spawnPoint + _dir * i) + (Vector3.forward * rowDist * j) + (Vector3.right * dist * 0.5f * staggerFactor);
                        }

                        Instantiate(_enemyPrefab, sp, Quaternion.Euler(0.0f,180f,0.0f));
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
                Vector3 _dir = Vector3.zero;
                if (e_offset < 0) {
                    _dir = (Vector3.left * Mathf.Sign(e_offset) - Vector3.forward).normalized * dist;
                } else {
                    _dir = (Vector3.right * Mathf.Sign(e_offset) + Vector3.forward).normalized * dist;
                }
                

                Vector3 sphereSpawn = Vector3.zero;

                sphereSpawn = (_spawnPoint + transform.position + _dir * e_offset);
                Gizmos.DrawSphere(sphereSpawn, 0.5f);
            }
        } else {

            for (int j = 0; j < numRows; ++j) {
                int staggerOffset = 0;
                int staggerFactor = 0;
                bool diag = false;
                if (j % 2 != 0 && stagger) {
                    staggerFactor = 1;
                    staggerOffset = -1;
                }
                for (int i = 0; i < numShifts + staggerOffset; ++i) {
                    Vector3 _dir = Vector3.zero;

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
                    }
                    Vector3 sphereSpawn = Vector3.zero;
                    if (diag) {
                        sphereSpawn = (_spawnPoint + transform.position + _dir * i) + (Vector3.forward * rowDist * j) + (_dir * 0.5f * staggerFactor);
                    } else {
                        sphereSpawn = (_spawnPoint + transform.position + _dir * i) + (Vector3.forward * rowDist * j) + (Vector3.right * dist * 0.5f * staggerFactor);
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
