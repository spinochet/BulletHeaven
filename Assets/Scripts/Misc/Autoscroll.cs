using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Autoscroll : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private GameObject background1;
    [SerializeField] private GameObject background2;

    private float timer;

    void Start()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 1.0f / 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.forward * speed * Time.deltaTime;
       
       background1.transform.position -= Vector3.forward * speed * Time.deltaTime;
       if (background1.transform.position.z <= -10.4f) background1.transform.position = new Vector3(0.0f, 0.0f, 40.0f);

       background2.transform.position -= Vector3.forward * speed * Time.deltaTime;
       if (background2.transform.position.z <= -10.4f) background2.transform.position = new Vector3(0.0f, 0.0f, 40.0f);
    }
}
