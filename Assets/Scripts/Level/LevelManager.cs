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
    private float zCheckpoint = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn backgrounds
        Quaternion spawnRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
        background_1 = GameObject.Instantiate(backgroundPrefab, new Vector3(0.0f, -1.0f, 20.2f), spawnRotation);
        background_1.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
        background_2 = GameObject.Instantiate(backgroundPrefab, new Vector3(0.0f, -1.0f, 45.0f), spawnRotation);
        background_2.GetComponent<SpriteRenderer>().sprite = backgroundSprite;

        // Spawn players
        PlayerManager.Instance.SpawnPawns(new Vector3(-2.0f, 0.0f, -2.0f), new Vector3(2.0f, 0.0f, -2.0f));

        cam = Camera.main;
        zCheckpoint = transform.position.z;
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
                    background_1.transform.position = background_2.transform.position + new Vector3(0.0f, -1.0f, 25.0f);
            }

            // Scroll background 2
            if (background_2)
            {
                background_2.transform.position -= Vector3.forward * scrollSpeed * Time.deltaTime;
                if (cam.WorldToScreenPoint(background_2.transform.position).y <= 0.0f)
                    background_2.transform.position = background_1.transform.position + new Vector3(0.0f, -1.0f, 25.0f);
            }
        }
    }

    // Toggle scrolling the level on and off
    public void ToggleScrolling(bool on)
    {
        isScrolling = on;
    }

    // Save checkpoint
    public void SetCheckpoint()
    {
        zCheckpoint = transform.position.z;

        WaveSpawner3D[] waves = FindObjectsOfType(typeof(WaveSpawner3D)) as WaveSpawner3D[];
        foreach (WaveSpawner3D wave in waves)
        {
            if (wave.IsSpawned)
                Destroy(wave.gameObject);
        }
    }

    // Save checkpoint
    public void RestoreCheckpoint()
    {
        transform.position = new Vector3(0.0f, 0.0f, zCheckpoint);
        isScrolling = true;

        Pawn[] pawns = FindObjectsOfType(typeof(Pawn)) as Pawn[];
        foreach (Pawn pawn in pawns)
        {
            Destroy(pawn.gameObject);
        }

        Bullet[] bullets = FindObjectsOfType(typeof(Bullet)) as Bullet[];
        foreach (Bullet bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }

        SideLaser[] lasers = FindObjectsOfType(typeof(SideLaser)) as SideLaser[];
        foreach (SideLaser laser in lasers)
        {
            Destroy(laser.gameObject);
        }

        SwordAttack[] swords = FindObjectsOfType(typeof(SwordAttack)) as SwordAttack[];
        foreach (SwordAttack sword in swords)
        {
            Destroy(sword.gameObject);
        }
        
        WaveSpawner3D[] waves = FindObjectsOfType(typeof(WaveSpawner3D)) as WaveSpawner3D[];
        foreach (WaveSpawner3D wave in waves)
        {
            wave.Reset();
        }



        // Spawn players
        PlayerManager.Instance.SpawnPawns(new Vector3(-2.0f, 0.0f, -2.0f), new Vector3(2.0f, 0.0f, -2.0f));
    }

    // Load next level
    public void NextLevel()
    {
        GameObject win = GameObject.Find("Win Screen");
        if (win)
        {
            win.GetComponent<WinScreen>().TriggerWin(nextLevel);
        }
        // SceneManager.LoadScene(nextLevel);
    }

    // ------------
    // EDITOR STUFF
    // ------------

    void OnDrawGizmos()
    {
        Vector3 p1 = new Vector3(-7.2f, 0f, 500.0f);
        Vector3 p2 = new Vector3(-7.2f, 0f, -5.4f);
        Vector3 p3 = new Vector3(7.2f, 0f, -5.4f);
        Vector3 p4 = new Vector3(7.2f, 0f, 500.0f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p3, p4);
    }
}
