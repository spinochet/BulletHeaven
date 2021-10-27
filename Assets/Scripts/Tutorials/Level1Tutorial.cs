using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Tutorial : TutorialScript
{
    [SerializeField] private List<GameObject> objects;
    private GameObject[] players;

    private bool canShoot = false;

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Character");

        foreach (GameObject player in players)
        {
            player.GetComponent<Pawn>().enabled = false;
        }

        if (!LevelManager.Instance.PlayTutorial)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.WorldToScreenPoint(transform.position).y <= Camera.main.pixelHeight && !tutorialStarted)
        {
            tutorialStarted = true;
        }
        else if (Camera.main.WorldToScreenPoint(transform.position).y <= 0 && !canShoot)
        {
            canShoot = true;
            LevelManager.Instance.PauseLevel();

            foreach (GameObject player in players)
            {
                player.GetComponent<Pawn>().enabled = true;
            }
        }
        else if (transform.position.z <= -16.0f)
        {
            LevelManager.Instance.EndTutorial();
            Destroy(gameObject);
        }
        if (tutorialStarted && canShoot && timer < 5.0f)
        {
            Debug.Log("Here");
            timer += Time.deltaTime;
        }
        else if (timer > 5.0f)
        {
            LevelManager.Instance.ResumeLevel();
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
