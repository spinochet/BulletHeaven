using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header ("Level Stuff")]
    [SerializeField] private string nextLevel;
    [SerializeField] private GameObject spawn1;
    [SerializeField] private GameObject spawn2;

    [Header ("Autoscroll")]
    [SerializeField] private float scrollSpeed = 2.5f;
    [SerializeField] private GameObject backgroundPrefab = null;
    [SerializeField] private Sprite backgroundSprite = null;
    private GameObject background_1 = null;
    private GameObject background_2 = null;

    private bool isScrolling = true;
    public bool IsScrolling { get { return isScrolling; } }

    private Camera cam = null;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn backgrounds
        Quaternion spawnRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
        background_1 = GameObject.Instantiate(backgroundPrefab, new Vector3(0.0f, -1.0f, 0.0f), spawnRotation);
        background_1.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
        background_2 = GameObject.Instantiate(backgroundPrefab, new Vector3(0.0f, -1.0f, 20.0f), spawnRotation);
        background_2.GetComponent<SpriteRenderer>().sprite = backgroundSprite;

        // Spawn players
        PlayerManager.Instance.SpawnPawns(new Vector3(-2.0f, 0.0f, -2.0f), new Vector3(2.0f, 0.0f, -2.0f));

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (isScrolling)
        {
            transform.position -= Vector3.forward * scrollSpeed * Time.deltaTime;

            // Scroll background 1
            if (background_1)
            {
                background_1.transform.position -= Vector3.forward * scrollSpeed * Time.deltaTime;
                if (cam.WorldToScreenPoint(background_1.transform.position).y <= 0.0f)
                    background_1.transform.position = new Vector3(0.0f, -1.0f, 34.6f);
            }

            // Scroll background 2
            if (background_2)
            {
                background_2.transform.position -= Vector3.forward * scrollSpeed * Time.deltaTime;
                if (cam.WorldToScreenPoint(background_2.transform.position).y <= 0.0f)
                    background_2.transform.position = new Vector3(0.0f, -1.0f, 34.6f);
            }
        }
    }

    // Toggle scrolling the level on and off
    public void ToggleScrolling(bool on)
    {
        isScrolling = on;
    }

    // Load next level
    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
