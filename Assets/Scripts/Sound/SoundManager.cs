using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public struct Sound
    {
        public List<AudioClip> sound;
        public float volume;
        public float soundCooldown;

        [HideInInspector] public float soundLastPlayed;
        [HideInInspector] public float timeLastPlayed;
    };

    [System.Serializable]
    public struct SoundHack
    {
        public string name;
        public Sound sound;
        public bool test;
    }

    // SoundManager Singleton
    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }

    public bool debugSound;

    // SFX Soundss
    [SerializeField] private AudioSource source;
    public List<SoundHack> soundList;

    private Dictionary<string, Sound> sounds;
    private float sfxVolume = 1.0f;
    private float timer = 0.0f;

    // BGM
    private float bgmVolume = 1.0f;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            DestroyImmediate(this);

        // Load in sounds
        sounds = new Dictionary<string, Sound>();
        foreach (SoundHack sound in soundList)
        {
            sounds.Add(sound.name, sound.sound);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (debugSound)
        {
            foreach (SoundHack soundHack in soundList)
            {
                if (soundHack.test)
                {
                    Sound sound = sounds[soundHack.name];

                    // Update sound
                    sound.volume = soundHack.sound.volume;
                    sound.soundCooldown = soundHack.sound.soundCooldown;

                    if ((timer - sound.timeLastPlayed) >= sound.soundCooldown)
                    {
                        int soundToPlay = Random.Range(0, sound.sound.Count);

                        sound.soundLastPlayed = soundToPlay;
                        sound.timeLastPlayed = timer;
                        sounds[soundHack.name] = sound;

                        source.PlayOneShot(sound.sound[soundToPlay], sound.volume);
                    }
                }
            }
        }
    }

    // Play SFX sounds
    public void Play(string name)
    {
        if (sounds.ContainsKey(name))
        {
            Sound sound = sounds[name];

            if ((timer - sound.timeLastPlayed) >= sound.soundCooldown)
            {
                int soundToPlay = Random.Range(0, sound.sound.Count);

                sound.soundLastPlayed = soundToPlay;
                sound.timeLastPlayed = timer;
                sounds[name] = sound;
                source.PlayOneShot(sound.sound[soundToPlay], sound.volume);
            }
        }
    }
}
