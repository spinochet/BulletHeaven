using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Autoscroll : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;

    private float timer;

    public void StartScroll()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 1.0f / 60.0f;
    }

    public void PauseScroll()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.forward * speed * Time.deltaTime;
    }
}
