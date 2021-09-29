using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner3D : MonoBehaviour
{
    public GameObject _enemyPrefab;

    void Start()
    {


    }

    void Update()
    {

        if (transform.position.z <= 0) {
            transform.position = Vector3.zero;
            for (int i = 0; i < numShifts; ++i) {
                Vector3 _dir = Vector3.zero;
                switch (dir) {
                    case Direction.FORWARD:
                        _dir = Vector3.forward * dist;
                        break;
                    case Direction.RIGHT:
                        _dir = Vector3.right * dist;
                        break;
                    case Direction.DIAG:
                        _dir = (Vector3.right * Mathf.Sign(dist) + Vector3.forward).normalized * Mathf.Abs(dist);
                        break;
                }

                Vector3 sp = _spawnPoint + _dir * i;
                Instantiate(_enemyPrefab, sp, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    public List<Vector3> _spawnPoints;

    public Vector3 _spawnPoint;
    public int numShifts;
    public float dist;
    public enum Direction { FORWARD, RIGHT, DIAG };

    public Direction dir;

    void OnDrawGizmosSelected()
    {
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

        for (int i = 0; i < numShifts; ++i) {
            Vector3 _dir = Vector3.zero;
            switch (dir) {
                case Direction.FORWARD:
                    _dir = Vector3.forward * dist;
                    break;
                case Direction.RIGHT:
                    _dir = Vector3.right * dist;
                    break;
                case Direction.DIAG:
                    _dir = (Vector3.right * Mathf.Sign(dist) + Vector3.forward).normalized * Mathf.Abs(dist);
                    break;
            }

            Gizmos.DrawSphere(_spawnPoint + transform.position + _dir * i, 0.5f);
        }
        
    }


}
