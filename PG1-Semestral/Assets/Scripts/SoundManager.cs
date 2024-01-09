using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Static instance, aby mohli být zvuky předávány odkudkoliv bez předávání reference
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource soundAS;
    [SerializeField] private AudioSource ambienceAS;

    [Header("Audio clips")]
    [SerializeField] private AudioClip bounceAC;
    [SerializeField] private AudioClip goalAC;
    [SerializeField] private AudioClip countdown321AC;
    [SerializeField] private AudioClip countdownStartAC;

    private Dictionary<string, AudioClip> sounds; // Zvuky a jejich názvy

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        sounds = new Dictionary<string, AudioClip> {
            {"Bounce", bounceAC},
            {"Goal", goalAC},
            {"321", countdown321AC},
            {"Start", countdownStartAC},
        };
    }

    public void PlaySound(string soundName)
    {
        if (!this.isActiveAndEnabled) return;
        soundAS.PlayOneShot(sounds[soundName]);
    }
    
}
