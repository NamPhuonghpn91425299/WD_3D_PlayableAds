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
    [SerializeField] private bool enableDebugLog = true;

    public void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        soundDict.Clear();
        if (soundList == null)
        {
            if (enableDebugLog) Debug.LogWarning($"[SoundSO:{name}] soundList is null.");
            return;
        }

        int addedCount = 0;
        for (int i = 0; i < soundList.Count; i++)
        {
            var sound = soundList[i];
            if (sound == null || sound.Clip == null)
            {
                if (enableDebugLog) Debug.LogWarning($"[SoundSO:{name}] Skip null clip at index {i}.");
                continue;
            }

            var key = string.IsNullOrEmpty(sound.clipName) ? sound.Clip.name : sound.clipName;
            if (string.IsNullOrEmpty(key))
            {
                if (enableDebugLog) Debug.LogWarning($"[SoundSO:{name}] Skip empty key at index {i}.");
                continue;
            }

            if (soundDict.ContainsKey(key))
            {
                if (enableDebugLog) Debug.LogWarning($"[SoundSO:{name}] Duplicate key '{key}' at index {i}. Keep first, skip later.");
                continue;
            }

            soundDict.Add(key, sound);
            addedCount++;
        }

        if (enableDebugLog)
        {
            var keys = string.Join(", ", soundDict.Keys.OrderBy(k => k));
            Debug.Log($"[SoundSO:{name}] Init done. Added={addedCount}/{soundList.Count}. Keys=[{keys}]");
        }
    }

    public AudioClip GetAudioClip(string soundName)
    {
        if (soundDict == null || soundDict.Count == 0) Init();
        if (string.IsNullOrEmpty(soundName)) return null;
        if (!soundDict.ContainsKey(soundName))
        {
            if (enableDebugLog)
            {
                var keys = string.Join(", ", soundDict.Keys.OrderBy(k => k));
                Debug.LogWarning($"[SoundSO:{name}] Missing clip key '{soundName}'. Available keys=[{keys}]");
            }
            return null;
        }

        return soundDict.ContainsKey(soundName) ? soundDict[soundName].Clip : null;
    }

    public float GetSoundVolume(string soundName)
    {
        if (soundDict == null || soundDict.Count == 0) Init();
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
