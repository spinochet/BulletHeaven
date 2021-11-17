using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (camera.WorldToScreenPoint(transform.position).y <= camera.pixelHeight) {
            LevelManager level = FindObjectOfType<LevelManager>();
            if (level) {
                level.SetCheckpoint();
            }

            Destroy(gameObject);
        }
    }
}
