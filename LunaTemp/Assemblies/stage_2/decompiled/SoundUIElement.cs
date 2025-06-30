using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundUIElement : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
{
	public SoundDefine Sound;

	[SerializeField]
	private bool PlayOnEnable = true;

	[SerializeField]
	private bool StopOnDisable = false;

	[SerializeField]
	private bool playWithInteractable = false;

	[SerializeField]
	private bool isPlayRandomBackGroundMusic = true;

	private Selectable _button;

	private Selectable button
	{
		get
		{
			if (_button == null)
			{
				_button = GetComponent<Selectable>();
			}
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
		SoundType soundType = Sound.soundType;
		SoundType soundType2 = soundType;
		if (soundType2 == SoundType.Hide)
		{
			SoundManager.PlaySound(Sound);
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

	public IEnumerator PlayBGM()
	{
		yield return null;
		if (Sound.soundType == SoundType.BackgroundMusic)
		{
			if (isPlayRandomBackGroundMusic)
			{
				SoundManager.PlayRandomBGM(Sound);
			}
			else
			{
				SoundManager.PlaySound(Sound);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (button == null || !button.enabled || (!button.interactable && !playWithInteractable))
		{
			return;
		}
		switch (Sound.soundType)
		{
		case SoundType.Button:
			if (eventData.button == PointerEventData.InputButton.Left && Sound.Clip != null)
			{
				SoundManager.PlaySound(Sound);
			}
			break;
		case SoundType.Toggle:
			if (eventData.button == PointerEventData.InputButton.Left && Sound.Clip != null)
			{
				SoundManager.PlaySound(Sound);
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
		if (button == null || (!button.interactable && !playWithInteractable))
		{
			return;
		}
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
