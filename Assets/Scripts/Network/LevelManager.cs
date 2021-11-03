using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class LevelManager : NetworkBehaviour
{
    // Singleton
    public static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    // Level
    [Header ("Level Stuff")]
    public GameObject spawnPointP1;
    public GameObject spawnPointP2;
    [SerializeField] private Pause pauseController;
    [SerializeField] private string nextLevel;

    // Backgrounds
    [Header ("Scrollable Backgrounds")]
    [SerializeField] private float scrollSpeed = 2.5f;
    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private GameObject background_1;
    [SerializeField] private GameObject background_2;
    private int currentBackground = 0;

    private Camera cam;
    private bool isScrolling = false;
    public bool IsScrolling { get { return isScrolling; } }

    private bool isPaused = false;
    public bool IsPaused { get { return isPaused; } }

    [Header ("Tutorial Stuff")]
    [SerializeField] private GameObject tutorial;
    [SerializeField] private bool playTutorial = false;
    public bool PlayTutorial { get { return playTutorial; } }

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            DestroyImmediate(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        background_1.transform.position = new Vector3(0.0f, -1.0f, 0.0f);
        //background_1.GetComponent<SpriteRenderer>().sprite = backgrounds[currentBackground];
        background_2.transform.position = new Vector3(0.0f, -1.0f, 20.0f);
        //background_2.GetComponent<SpriteRenderer>().sprite = backgrounds[currentBackground];

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (isScrolling)
        {
            if (!playTutorial)
                transform.position -= Vector3.forward * scrollSpeed * Time.deltaTime;
            else
                tutorial.transform.position -= Vector3.forward * scrollSpeed * Time.deltaTime;
            
            // Scroll background 1
            background_1.transform.position -= Vector3.forward * scrollSpeed * Time.deltaTime;
            if (cam.WorldToScreenPoint(background_1.transform.position).y <= 0.0f)
                background_1.transform.position = new Vector3(0.0f, -1.0f, 34.6f);

            // Scroll background 2
            background_2.transform.position -= Vector3.forward * scrollSpeed * Time.deltaTime;
            if (cam.WorldToScreenPoint(background_2.transform.position).y <= 0.0f)
                background_2.transform.position = new Vector3(0.0f, -1.0f, 34.6f);
        }
    }

    // --------------
    // TUTORIAL STUFF
    // --------------

    public void EndTutorial()
    {
        Debug.Log("End");
        playTutorial = false;
    }

    // [ClientRpc]
    // public void EndTutorialRpc()
    // {
    //     playTutorial = false;
    // }

    // -----------
    // LEVEL STUFF
    // -----------

    public void StartLevel()
    {
        isScrolling = true;
        // StartLevelRpc();
    }

    // [ClientRpc]
    // public void StartLevelRpc()
    // {
    //     isScrolling = true;
    // }

    public void NextLevel()
    {
        PlayerNetworkManager.Instance.LoadArcadeLevel(nextLevel);
    }

    public GameObject SpawnEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
    {
        GameObject e = Instantiate(enemyPrefab, position, rotation);
        NetworkServer.Spawn(e);

        return e;
    }

    public void StopScrolling()
    {
        isScrolling = false;
        // PauseLevelRpc();
    }

    [Command(requiresAuthority = false)]
    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseController.gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.0f : 1.0f;

        TogglePauseRpc();
    }

    [ClientRpc]
    public void TogglePauseRpc()
    {
        isPaused = !isPaused;
        pauseController.gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.0f : 1.0f;
    }

    public void ResumeLevel()
    {
        isScrolling = true;
        isPaused = false;
        Time.timeScale = 1.0f;

        ResumeLevelRpc();
    }

    [ClientRpc]
    public void ResumeLevelRpc()
    {
        isScrolling = true;
        isPaused = false;
        Time.timeScale = 1.0f;
    }
}
