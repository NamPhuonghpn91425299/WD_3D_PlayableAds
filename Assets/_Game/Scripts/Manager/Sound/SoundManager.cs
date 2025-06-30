using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]         AudioMixer  audioMixer;
    [SerializeField] private AudioSource fxMusicSource;
    [SerializeField] private AudioSource specialBgmSource;
    
    [SerializeField] private SoundDefine BGM;

    private void Start()
    {
        var playMusic = 1;
        var playSound  = 1;
        OnSoundChange(playMusic);
        OnSoundFxChange(playSound);
    }

    private void OnSoundFxChange(float currentValue)
    {
        if (audioMixer == null) return;
        currentValue *= 2; // sound fx có âm lượng gấp đôi
        var soundValue    = currentValue == 0 ? -100 : Mathf.Log10(currentValue) * 20;
        var parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.SoundFx);
        var checkSet      = audioMixer.SetFloat(parameterName, soundValue);
#if UNITY_EDITOR
        if (!checkSet) Debug.LogError($"không set được giá trị audio mixer với parameter {parameterName}");
#endif
    }

    private void OnSoundChange(float currentValue)
    {
        if (audioMixer == null) return;
        float maxRangeDesign = 1f;
        currentValue = MathHr.Remap(currentValue, 0, 1, 0, maxRangeDesign);
        var soundValue    = currentValue == 0 ? -100 : Mathf.Log10(currentValue) * 20;
        var parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.Sound);
        var checkSet      = audioMixer.SetFloat(parameterName, soundValue);
#if UNITY_EDITOR
        if (!checkSet) Debug.LogError($"không set được giá trị audio mixer với parameter {parameterName}");
#endif
    }

    private void ChangeVolumeSpecialBgmSound(float currentValue)
    {
        if (audioMixer == null) return;
        var soundValue    = currentValue == 0 ? -100 : Mathf.Log10(currentValue) * 20;
        var parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.SpecialSound);
        var checkSet      = audioMixer.SetFloat(parameterName, soundValue);
#if UNITY_EDITOR
        if (!checkSet) Debug.LogError($"không set được giá trị audio mixer với parameter {parameterName}");
#endif
    }


    private void OnMainSoundChange(float currentValue)
    {
        if (audioMixer == null) return;
        currentValue *= 2;
        var soundValue    = currentValue == 0 ? -100 : Mathf.Log10(currentValue) * 20;
        var parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.MainSound);
        var checkSet      = audioMixer.SetFloat(parameterName, soundValue);
#if UNITY_EDITOR
        if (!checkSet) Debug.LogError($"không set được giá trị audio mixer với parameter {parameterName}");
#endif
    }

    private void OnEnableShowFxInGame(bool enable)
    {
        if (audioMixer == null) return;
        var soundValue    = enable ? Mathf.Log10(1) * 20 : -100;
        var parameterName = Enum.GetName(typeof(SoundMixerGroup), SoundMixerGroup.SoundFxInGame);
        var checkSet      = audioMixer.SetFloat(parameterName, soundValue);
#if UNITY_EDITOR
        if (!checkSet) Debug.LogError($"không set được giá trị audio mixer với parameter {parameterName}");
#endif
    }

    private bool _isFxPauseBySpeed;

    public void SetSpeedAudioGroup(SoundMixerGroup group, float speed) // speed thường để bằng với timescale
    {
        if (group == SoundMixerGroup.SoundFxInGame)
        {
            if (speed == 0 && !_isFxPauseBySpeed)
            {
                _isFxPauseBySpeed = true;
                OnEnableShowFxInGame(false);
            }
            else if (speed > 0 && _isFxPauseBySpeed)
            {
                _isFxPauseBySpeed = false;
                OnEnableShowFxInGame(true);
            }
        }

        var actualValue   = 1 - Mathf.Abs(1 - speed) / 2 * Mathf.Sign(1 - speed); // công thức của mr xương rồng
        var parameterName = Enum.GetName(typeof(SoundMixerGroup), group) + "Pitch";
        var checkSet      = audioMixer.SetFloat(parameterName, actualValue);
#if UNITY_EDITOR
        if (!checkSet) Debug.LogError($"không set được giá trị pitch audio mixer với parameter {parameterName}");
#endif
    }

    /// <summary>
    /// Hàm phát một âm thanh với Mixer là SoundFx
    /// </summary>
    /// <param name="clip"></param>
    public void PlayOneShotFx(AudioClip clip)
    {
        fxMusicSource.PlayOneShot(clip);
    }
    
    /// <summary>
    /// Hàm phát một âm thanh với Mixer là Sound
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
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
        PlayOneShot(clip,volume);
    }
    public static void PlaySound(SoundDefine sound)
    {
        if (sound.Clip == null) return;
        switch (sound.soundType)
        {
            case SoundType.Effect:
                if (Instance.fxMusicSource) Instance.fxMusicSource.PlayOneShot(sound.Clip);
                break;
            default:
                if (Instance.fxMusicSource) Instance.fxMusicSource.PlayOneShot(sound.Clip);
                break;
        }
    }

    private Coroutine _specialSoundStopCountDown;
    
    
    private Coroutine _specialSoundLoop;
    public void PlaySpecialSoundLoop(AudioClip clip, float delay = 0, float bgmRatio = 0)
    {
        if (_specialSoundLoop != null)
        {
            StopCoroutine(_specialSoundLoop);
            return;
        }
        _specialSoundLoop = StartCoroutine(LoopSpecialSound(clip, delay, bgmRatio));
    }
    public IEnumerator LoopSpecialSound(AudioClip clip,float delay = 0 , float bgmRatio = 0)
    {
        //float targetBgmVolume = DataManager.SettingData.Sound * bgmRatio;
        //OnSoundChange(targetBgmVolume);
        //ChangeVolumeSpecialBgmSound(DataManager.SettingData.Sound);
        Instance.specialBgmSource.Stop();
        Instance.specialBgmSource.clip = clip;
        Instance.specialBgmSource.loop = true;
        Instance.specialBgmSource.Play();
        if (delay != 0)
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
    private Tween _specialSoundTween;

    public static void StopSound(SoundDefine sound)
    {
        if (sound.Clip == null) return;
        switch (sound.soundType)
        {
            default:
                Instance?.fxMusicSource.Stop();
                break;
        }
    }

    private static List<AudioClip> _backGroundMusics = new List<AudioClip>();
    private static List<AudioClip> _bgmWaitingList   = new List<AudioClip>();
    private        bool            _isLoopRandomBGM  = false;
    private        bool            _isEndGame        = false;

    public static void PlayRandomBGM(SoundDefine sound)
    {
        if (sound.soundType != SoundType.BackgroundMusic || sound.ClipList == null || sound.ClipList.Count == 0) return;
        Instance._isLoopRandomBGM = false;
        _backGroundMusics = sound.ClipList;
        _bgmWaitingList = new List<AudioClip>(_backGroundMusics);
        Instance._isLoopRandomBGM = true;
    }
    private void SetBoolEndGameUI(bool value)
    {
        _isEndGame = value;
    }
    
}
    
// đặt theo exposed Parameters trong audio mixer
public enum SoundMixerGroup
{
    SoundFx,
    Sound,
    MainSound,
    SoundFxInGame,
    SpecialSound
}