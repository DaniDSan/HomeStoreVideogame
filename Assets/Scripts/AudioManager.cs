using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAS;

    [SerializeField] private AudioSource sfxAS;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxAS.PlayOneShot(clip);
    }
}
