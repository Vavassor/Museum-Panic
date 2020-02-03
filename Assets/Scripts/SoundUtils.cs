using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SoundUtils
{
    public static void PlaySoundNonrepeating(AudioSource audioSource, List<AudioClip> audioClips)
    {
        int n = Random.Range(1, audioClips.Count);
        audioSource.clip = audioClips[n];
        audioSource.PlayOneShot(audioSource.clip);
        audioClips[n] = audioClips[0];
        audioClips[0] = audioSource.clip;
    }
}