using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    protected bool tutorialStarted;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
