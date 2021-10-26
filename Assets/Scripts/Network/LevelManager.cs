using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class LevelManager : NetworkBehaviour
{
    // Singleton
    public static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

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
        background_1.GetComponent<SpriteRenderer>().sprite = backgrounds[currentBackground];
        background_2.transform.position = new Vector3(0.0f, -1.0f, 20.0f);
        background_2.GetComponent<SpriteRenderer>().sprite = backgrounds[currentBackground];

        cam = Camera.main;
        isScrolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isScrolling)
        {
            transform.position -= Vector3.forward * scrollSpeed * Time.deltaTime;
            
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

    void StartLevel()
    {
        
    }

    public void PauseLevel()
    {
        isScrolling = false;
    }

    public void ResumeLevel()
    {
        isScrolling = true;
    }
}
