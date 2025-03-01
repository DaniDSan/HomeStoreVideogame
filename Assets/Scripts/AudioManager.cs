using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundEffect {
    public AudioClip clip;
    [Range(0.1f, 3f)]
    public float pitch;
    [Range(0f, 0.5f)]
    public float randomizeRange;
    public bool isRandomize;
    [Range(0f, 1f)]
    public float soundVolume;

    public SoundEffect(AudioClip clip, float pitch, float randomizeRange, bool isRandomize, float soundVolume) {
        this.clip = clip;
        this.pitch = pitch;
        this.randomizeRange = randomizeRange;
        this.isRandomize = isRandomize;
        this.soundVolume = soundVolume;
    }

    public float GetRandomPitch() {
        return pitch + Random.Range(-randomizeRange, randomizeRange);
    }
}

[System.Serializable]
public struct PlacementSoundsEffects {
    public SoundEffect rotateSFX;
    public SoundEffect placeSFX;
    public SoundEffect sellSFX;
}


public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioSource musicAS;

    [SerializeField] private AudioSource sfxAS;

    public PlacementSoundsEffects placementSoundsEffects = new PlacementSoundsEffects();

    public static AudioManager instance;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void PlaySFX(SoundEffect soundEffect) {
        sfxAS.volume = soundEffect.soundVolume;
        sfxAS.pitch = soundEffect.GetRandomPitch();
        sfxAS.PlayOneShot(soundEffect.clip);
    }
}
