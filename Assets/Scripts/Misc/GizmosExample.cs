using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GizmosExample : MonoBehaviour
{
    public bool show;
    public Transform target1;
    public Transform target2;

    void OnDrawGizmos()
    {
        if (show)
        {
            // Display the explosion radius when selected
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1.0f);
            Gizmos.DrawLine(transform.position, target1.position);
            Gizmos.DrawLine(target1.position, target2.position);
        }
    }
}
