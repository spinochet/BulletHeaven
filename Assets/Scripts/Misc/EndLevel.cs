using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= 0.0f)
            SceneManager.LoadScene("MainMenu");
    }
}
