using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private string nextLevel;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= 0.0f)
        {
            transform.parent.GetComponent<LevelManager>().NextLevel();
        }
    }
}
