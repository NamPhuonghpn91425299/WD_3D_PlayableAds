using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SoundDefine
{
	public SoundType soundType;

	public bool Loop;

	public AudioClip Clip;

	public List<AudioClip> ClipList;

	public void Dispose()
	{
		Clip = null;
		ClipList = null;
	}
}
