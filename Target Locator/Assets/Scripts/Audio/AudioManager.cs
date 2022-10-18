using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
	#region Variables
	public AudioMixer audioMixer;
	public Sound[] sounds;
	#endregion

	#region Unity Callbacks
	protected override void Awake()
	{
		base.Awake();
	}
	#endregion

	#region Methods
	public AudioSource PlayMusic(string _name)
	{
		Sound sound = GetSound(_name);
		if (sound == null)
			return null;

		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		SetAudioSource(audioSource, sound);
		audioSource.Play();

		return audioSource;
	}

	// Used for non-persistent sounds like sfx and ambiance etc.
	public GameObject PlaySoundAtPoint(string _name, Vector3 _position)
	{
		// Get Sound
		Sound sound = GetSound(_name);
		if (sound == null)
			return null;

		// Create a new game object
		GameObject soundGameObject = new GameObject("Sound");
		soundGameObject.transform.position = _position;

		// Add AudioSource component
		AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
		SetAudioSource(audioSource, sound);
		if (audioSource.loop == false)
			Destroy(soundGameObject, audioSource.clip.length);

		// Play sound
		audioSource.Play();

		return soundGameObject;
	}

	public void PlaySound(string _name)
	{
		Sound sound = GetSound(_name);
		if (sound == null)
			return;

		GameObject soundGameObject = new GameObject("Sound");
		soundGameObject.transform.position = Vector3.zero;
		AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
		SetAudioSource(audioSource, sound);
		if (audioSource.loop == false)
			Destroy(soundGameObject, audioSource.clip.length); // HAS TO BE REMOVED IF POOLING IS ADDED!!!

		audioSource.Play();
	}

	public Sound GetSound(string _name)
	{
		Sound sound = Array.Find(sounds, s => s.name == _name);
		if (sound == null)
		{
			Debug.LogWarning("Sound: " + _name + " not found!");
			return null;
		}

		return sound;
	}

	public void SetAudioSource(AudioSource _audioSource, Sound _sound)
	{
		_audioSource.clip = _sound.clip;
		_audioSource.outputAudioMixerGroup = _sound.mixerGroup;
		_audioSource.priority = _sound.priority;
		_audioSource.volume = _sound.volume;

		if (_sound.pitchRange > 0)
			_audioSource.pitch = RandomizePitch(_sound.pitch, _sound.pitchRange);
		else
			_audioSource.pitch = _sound.pitch;

		_audioSource.spatialBlend = _sound.spatialBlend;
		_audioSource.loop = _sound.loop;
	}

	public float RandomizePitch(float _pitch, float _pitchRange)
	{
		return UnityEngine.Random.Range(_pitch - _pitchRange, _pitch + _pitchRange);
	}

	public string GetRandomString(params string[] _strings)
	{
		int randomIndex = UnityEngine.Random.Range(0, _strings.Length);
		return _strings[randomIndex];
	}
	#endregion
}
