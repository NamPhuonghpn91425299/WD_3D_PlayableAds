using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
	[SerializeField]
	private AudioMixer audioMixer;

	[SerializeField]
	private AudioSource fxMusicSource;

	[SerializeField]
	private AudioSource specialBgmSource;

	[SerializeField]
	private SoundDefine BGM;

	private bool _isFxPauseBySpeed;

	private Coroutine _specialSoundStopCountDown;

	private Coroutine _specialSoundLoop;

	private Tween _specialSoundTween;

	private static List<AudioClip> _backGroundMusics = new List<AudioClip>();

	private static List<AudioClip> _bgmWaitingList = new List<AudioClip>();

	private bool _isLoopRandomBGM = false;

	private bool _isEndGame = false;

	private void Start()
	{
		int playMusic = 1;
		int playSound = 1;
		OnSoundChange(playMusic);
		OnSoundFxChange(playSound);
	}

	private void OnSoundFxChange(float currentValue)
	{
		if (!(audioMixer == null))
		{
			currentValue *= 2f;
			float soundValue = ((currentValue == 0f) ? (-100f) : (Mathf.Log10(currentValue) * 20f));
			string parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.SoundFx);
			bool checkSet = audioMixer.SetFloat(parameterName, soundValue);
		}
	}

	private void OnSoundChange(float currentValue)
	{
		if (!(audioMixer == null))
		{
			float maxRangeDesign = 1f;
			currentValue = MathHr.Remap(currentValue, 0f, 1f, 0f, maxRangeDesign);
			float soundValue = ((currentValue == 0f) ? (-100f) : (Mathf.Log10(currentValue) * 20f));
			string parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.Sound);
			bool checkSet = audioMixer.SetFloat(parameterName, soundValue);
		}
	}

	private void ChangeVolumeSpecialBgmSound(float currentValue)
	{
		if (!(audioMixer == null))
		{
			float soundValue = ((currentValue == 0f) ? (-100f) : (Mathf.Log10(currentValue) * 20f));
			string parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.SpecialSound);
			bool checkSet = audioMixer.SetFloat(parameterName, soundValue);
		}
	}

	private void OnMainSoundChange(float currentValue)
	{
		if (!(audioMixer == null))
		{
			currentValue *= 2f;
			float soundValue = ((currentValue == 0f) ? (-100f) : (Mathf.Log10(currentValue) * 20f));
			string parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.MainSound);
			bool checkSet = audioMixer.SetFloat(parameterName, soundValue);
		}
	}

	private void OnEnableShowFxInGame(bool enable)
	{
		if (!(audioMixer == null))
		{
			float soundValue = (enable ? (Mathf.Log10(1f) * 20f) : (-100f));
			string parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.SoundFxInGame);
			bool checkSet = audioMixer.SetFloat(parameterName, soundValue);
		}
	}

	public void SetSpeedAudioGroup(SoundMixerGroup group, float speed)
	{
		if (group == SoundMixerGroup.SoundFxInGame)
		{
			if (speed == 0f && !_isFxPauseBySpeed)
			{
				_isFxPauseBySpeed = true;
				OnEnableShowFxInGame(false);
			}
			else if (speed > 0f && _isFxPauseBySpeed)
			{
				_isFxPauseBySpeed = false;
				OnEnableShowFxInGame(true);
			}
		}
		float actualValue = 1f - Mathf.Abs(1f - speed) / 2f * Mathf.Sign(1f - speed);
		string parameterName = Enum.GetName(typeof(SoundMixerGroup), group) + "Pitch";
		bool checkSet = audioMixer.SetFloat(parameterName, actualValue);
	}

	public void PlayOneShotFx(AudioClip clip)
	{
		fxMusicSource.PlayOneShot(clip);
	}

	public void PlayOneShot(AudioClip clip, float volume = 1f)
	{
		fxMusicSource.PlayOneShot(clip, volume);
	}

	public void PlayOneShotDelayed(AudioClip clip, float delay, float volume = 1f)
	{
		StartCoroutine(PlayDelayed(clip, delay, volume));
	}

	private IEnumerator PlayDelayed(AudioClip clip, float delay, float volume = 1f)
	{
		yield return new WaitForSeconds(delay);
		PlayOneShot(clip, volume);
	}

	public static void PlaySound(SoundDefine sound)
	{
		if (sound.Clip == null)
		{
			return;
		}
		SoundType soundType = sound.soundType;
		SoundType soundType2 = soundType;
		if (soundType2 == SoundType.Effect)
		{
			if ((bool)Singleton<SoundManager>.Instance.fxMusicSource)
			{
				Singleton<SoundManager>.Instance.fxMusicSource.PlayOneShot(sound.Clip);
			}
		}
		else if ((bool)Singleton<SoundManager>.Instance.fxMusicSource)
		{
			Singleton<SoundManager>.Instance.fxMusicSource.PlayOneShot(sound.Clip);
		}
	}

	public void PlaySpecialSoundLoop(AudioClip clip, float delay = 0f, float bgmRatio = 0f)
	{
		if (_specialSoundLoop != null)
		{
			StopCoroutine(_specialSoundLoop);
		}
		else
		{
			_specialSoundLoop = StartCoroutine(LoopSpecialSound(clip, delay, bgmRatio));
		}
	}

	public IEnumerator LoopSpecialSound(AudioClip clip, float delay = 0f, float bgmRatio = 0f)
	{
		Singleton<SoundManager>.Instance.specialBgmSource.Stop();
		Singleton<SoundManager>.Instance.specialBgmSource.clip = clip;
		Singleton<SoundManager>.Instance.specialBgmSource.loop = true;
		Singleton<SoundManager>.Instance.specialBgmSource.Play();
		if (delay != 0f)
		{
			yield return new WaitForSeconds(delay);
			StopSpecialSoundLoop();
		}
	}

	public void StopSpecialSoundLoop()
	{
		if (_specialSoundLoop != null)
		{
			StopCoroutine(_specialSoundLoop);
		}
		specialBgmSource.Stop();
	}

	public static void StopSound(SoundDefine sound)
	{
		if (!(sound.Clip == null))
		{
			SoundType soundType = sound.soundType;
			SoundType soundType2 = soundType;
			Singleton<SoundManager>.Instance?.fxMusicSource.Stop();
		}
	}

	public static void PlayRandomBGM(SoundDefine sound)
	{
		if (sound.soundType == SoundType.BackgroundMusic && sound.ClipList != null && sound.ClipList.Count != 0)
		{
			Singleton<SoundManager>.Instance._isLoopRandomBGM = false;
			_backGroundMusics = sound.ClipList;
			_bgmWaitingList = new List<AudioClip>(_backGroundMusics);
			Singleton<SoundManager>.Instance._isLoopRandomBGM = true;
		}
	}

	private void SetBoolEndGameUI(bool value)
	{
		_isEndGame = value;
	}
}
