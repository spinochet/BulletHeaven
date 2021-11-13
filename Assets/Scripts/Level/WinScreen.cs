using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [Header ("Player HUD")]
    [SerializeField] private HUDController p1Controller;
    [SerializeField] private HUDController p2Controller;

    [Header ("Win Screen Components")]
    [SerializeField] private AudioSource winAudio;
    [SerializeField] private RawImage p1Portrait;
    [SerializeField] private RawImage p2Portrait;
    [SerializeField] private Text p1Score;
    [SerializeField] private Text p2Score;

    [Space (10)]
    [SerializeField] private GameObject p1;
    [SerializeField] private GameObject p2;
    [SerializeField] private GameObject p1Text;
    [SerializeField] private GameObject p2Text;
    [SerializeField] private float time = 2.5f;



    private CanvasGroup canvas;
    public InputAction next;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    public void TriggerWin(string nextLevel)
    {
        // Update p1 stats
        if (!p1Controller.gameObject.activeSelf)
        {
            p1.SetActive(false);
            p1Text.SetActive(false);
        }
        else
        {
            p1Portrait.texture = p1Controller.Portrait;
            p1Score.text = p1Controller.Score;

            Debug.Log("P1 active");
        }

        // Update p2 stats
        if (!p2Controller.gameObject.activeSelf)
        {
            p2.SetActive(false);
            p2Text.SetActive(false);
        }
        else
        {
            p2Portrait.texture = p2Controller.Portrait;
            p2Score.text = p2Controller.Score;

            Debug.Log("P2 active");
        }

        // Lock player and display win screen
        PlayerManager.Instance.LockPlayers();
        StartCoroutine(TriggerwinRoutine(nextLevel));
    }

    IEnumerator TriggerwinRoutine(string nextLevel)
    {
        float timer = 0.0f;

        GameObject[] bgm = GameObject.FindGameObjectsWithTag("BGM");
        AudioSource audio = bgm[0].GetComponent<AudioSource>();

        float maxVolume = audio.volume;

        while (timer <= time)
        {
            timer += Time.deltaTime;
            canvas.alpha = timer / time;

            audio.volume = maxVolume - (maxVolume * (timer / time));
            yield return null;
        }

        canvas.alpha = 1;
        winAudio.Play();

        // Set up next action
        next.Enable();
        next.performed += ctx => NextLevel(nextLevel);
    }

    private void NextLevel(string nextLevel)
    {
        next.Disable();
        PlayerManager.Instance.UnlockPlayers();
        SceneManager.LoadScene(nextLevel);
    }
}
