using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundDefine
{
    public SoundType soundType;
    public bool Loop;
    public AudioClip Clip;
    public List<AudioClip> ClipList;

    public void Dispose()
    {
        Clip     = null;
        ClipList = null;
    }
}

public enum SoundType
{
    None,
    Button,
    Toggle,
    MouseSystem,
    BackgroundMusic,
    Effect,
    Slider,
    Hide
}
