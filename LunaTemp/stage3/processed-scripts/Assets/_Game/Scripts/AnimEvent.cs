using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    public AudioSource MainAudioSource;
    public AudioClip[] MotionAudioClips;
    public void RaiseVoice(int voiceIndex)
    {

        if (MotionAudioClips != null)
        {
            MainAudioSource.clip = MotionAudioClips[voiceIndex - 1];
            MainAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"No audio clip found for voice index: {voiceIndex}");
        }
    }
}
