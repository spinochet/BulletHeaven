using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public struct Sound
    {
        public AudioClip sound;
        public float volume;
        public float soundCooldown;
        public float soundTimer;
    };

    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }

    [SerializeField] private AudioSource source;
    [SerializeField] private Sound fireball;
    [SerializeField] private Sound laser;
    [SerializeField] private Sound arrow;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        fireball.soundTimer += Time.unscaledDeltaTime;
        laser.soundTimer += Time.unscaledDeltaTime;
        arrow.soundTimer += Time.unscaledDeltaTime;
    }

    public void Play(string name)
    {
        switch (name)
        {
            case "Fireball":
                Play(fireball);
                break;
            case "Laser":
                Play(laser);
                break;
            case "Arrow":
                Play(arrow);
                break;
            default:
                break;
        }
    }

    private void Play(Sound sound)
    {
        if (sound.soundTimer >= sound.soundCooldown)
        {
            source.PlayOneShot(sound.sound, sound.volume);
            sound.soundTimer = 0.0f;
        }
    }
}
