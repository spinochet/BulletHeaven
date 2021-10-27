using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTutorial : TutorialScript
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.WorldToScreenPoint(transform.position).y <= Camera.main.pixelHeight && !tutorialStarted)
        {
            tutorialStarted = true;
            Debug.Log("Here");
        }
    }
}
