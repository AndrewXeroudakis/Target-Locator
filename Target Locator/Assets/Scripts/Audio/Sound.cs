using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
	#region Variables
	public string name;
	public AudioClip clip;
	public AudioMixerGroup mixerGroup;
	[Range(0, 256)]
	public int priority;
	[Range(0f, 1f)]
	public float volume = 1;
	[Range(-3f, 3f)]
	public float pitch = 1;
	[Range(0f, 1f)]
	public float pitchRange = 0.05f;
	[Range(0f, 1f)]
	public float spatialBlend;
	public bool loop;
	#endregion
}