using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Autoscroll _autoScrollScript;

    void Start()
    {
        _autoScrollScript = GetComponent<Autoscroll>();
    }

    void StartLevel()
    {
        _autoScrollScript.StartScroll();
    }

    void PauseLevel()
    {
        _autoScrollScript.PauseScroll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
