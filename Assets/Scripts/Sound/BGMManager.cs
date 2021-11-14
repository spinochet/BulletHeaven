using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> BGMTracks;
    private int index = 0;

    public bool debug = false;
    public InputAction play1;
    public InputAction play2;
    public InputAction play3;
    public InputAction play4;
    public InputAction play5;

    // Start is called before the first frame update
    void Start()
    {
        BGMTracks[index].mute = false;

        if (debug)
        {
            play1.Enable();
            play2.Enable();
            play3.Enable();
            play4.Enable();
            play5.Enable();

            play1.performed += ctx => SwitchTrack(0);
            play2.performed += ctx => SwitchTrack(1);
            play3.performed += ctx => SwitchTrack(2);
            play4.performed += ctx => SwitchTrack(3);
            play5.performed += ctx => SwitchTrack(4);
        }
    }

    // Switch audio tracks
    void SwitchTrack(int i)
    {
        // Check bounds
        i = i < BGMTracks.Count ? i : 0;

        // Switch tracks
        BGMTracks[index].mute = true;
        BGMTracks[i].mute = false;
        index = i;
    }
}
