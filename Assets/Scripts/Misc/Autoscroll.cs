using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Autoscroll : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.forward * speed * Time.deltaTime;
       // if (transform.position.z <= -137.5f)
       //{
       //     speed = 0.0f;
       //     timer += Time.deltaTime;

       //     if (timer > 7.5f)
       //     {
       //         SceneManager.LoadScene("MainMenu");
       //     }
       // }
    }
}
