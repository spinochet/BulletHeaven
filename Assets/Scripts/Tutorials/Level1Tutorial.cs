using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Tutorial : TutorialScript
{
    [SerializeField] private List<GameObject> objects;
    private GameObject[] players;
    private LevelManager levelManager;

    private bool canShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Character");
        levelManager = transform.parent.GetComponent<LevelManager>();

        // foreach (GameObject player in players)
        // {
        //     player.GetComponent<Pawn>().enabled = false;
        // }

        // if (!LevelManager.Instance.PlayTutorial)
        // {
        //     Destroy(gameObject);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorialStarted && Camera.main.WorldToScreenPoint(transform.position).y <= 0)
        {
            tutorialStarted = true;
            levelManager.ToggleScrolling(false);
        }
        else if (transform.position.z <= -16.0f)
        {
            levelManager.ToggleScrolling(true);
            Destroy(gameObject);
        }
        if (tutorialStarted && timer < timing)
        {
            Debug.Log("Counting");
            timer += Time.deltaTime;
        }
    }

    private bool TutorialFinished()
    {
        foreach (GameObject tutorialObject in objects)
        {
            if (tutorialObject != null) return false;
        }

        return false;
    }
}
