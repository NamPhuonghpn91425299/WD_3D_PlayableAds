using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/SoundSO", order = 10)]
public class SoundSO : ScriptableObject
{
    [SerializeField] public List<SoundData> soundList;
    public Dictionary<string, SoundData> soundDict = new Dictionary<string, SoundData>();
    public void OnEnable()
    {
        //soundDict = soundList.ToDictionary(x => x.Clip.name, x => x);
        foreach (SoundData sound in soundList)
        {
            if (string.IsNullOrEmpty(sound.clipName)) soundDict.Add(sound.Clip.name, sound);
            else soundDict.Add(sound.clipName, sound);
        }
    }
    
    public AudioClip GetAudioClip(string soundName)
    {
        return soundDict.ContainsKey(soundName) ? soundDict[soundName].Clip : null;
    }

    public float GetSoundVolume(string soundName)
    {
        return soundDict.ContainsKey(soundName) ? soundDict[soundName].Volume : 1f;
    }
}

[Serializable]
public class SoundData
{
    public string clipName;
    public AudioClip Clip;
    public float Volume = 1f;
}
