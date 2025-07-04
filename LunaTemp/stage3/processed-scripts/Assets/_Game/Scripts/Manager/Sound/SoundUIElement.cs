using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class SoundUIElement : MonoBehaviour, IPointerClickHandler, ISubmitHandler
{
    public SoundDefine Sound;
    [SerializeField] private bool PlayOnEnable = true;
    [SerializeField] private bool StopOnDisable = false;
    [SerializeField] bool playWithInteractable = false;
    [SerializeField] bool isPlayRandomBackGroundMusic = true;

    private Selectable _button;
    private Selectable button
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Selectable>();
            return _button;
        }
    }

    protected virtual void OnEnable()
    {
        if (PlayOnEnable)
        {
            switch (Sound.soundType)
            {
                case SoundType.Effect:
                    SoundManager.PlaySound(Sound);
                    break;
                case SoundType.BackgroundMusic:
                    PlayBGM();
                    break;
            }
        }
    }

    protected virtual void OnDisable()
    {
        switch (Sound.soundType)
        {
            case SoundType.Hide:
                SoundManager.PlaySound(Sound);
                break;
        }

        if (StopOnDisable)
        {
            SoundManager.StopSound(Sound);
        }
    }

    protected virtual void OnDestroy()
    {
        Sound.Dispose();
        _button = null;
    }

    // showroom sửa lại nhé
    public IEnumerator PlayBGM() // xóa
    {
        yield return null;
        if (Sound.soundType == SoundType.BackgroundMusic)
        {
            if (isPlayRandomBackGroundMusic)
                SoundManager.PlayRandomBGM(Sound);
            else
                SoundManager.PlaySound(Sound);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button == null || button.enabled == false || !button.interactable && !playWithInteractable) return;
        switch (Sound.soundType)
        {
            case SoundType.Button:
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    if (Sound.Clip != null)
                    {
                        SoundManager.PlaySound(Sound);
                    }
                }
                break;
            case SoundType.Toggle:
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    if (Sound.Clip != null)
                    {
                        SoundManager.PlaySound(Sound);
                    }
                }
                break;
            case SoundType.MouseSystem:
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    SoundManager.PlaySound(Sound);
                }
                break;
        }

    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (button == null || !button.interactable && !playWithInteractable) return;
        switch (Sound.soundType)
        {
            case SoundType.Button:
                if (button != null && button.IsActive() && button.IsInteractable())
                {
                    SoundManager.PlaySound(Sound);
                }
                break;
            case SoundType.Toggle:
                
                SoundManager.PlaySound(Sound);
                break;
        }
    }
}
