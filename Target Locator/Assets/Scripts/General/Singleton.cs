using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
	#region Variables
	public static T Instance { get; private set; }
	#endregion

	#region Unity Event Functions
	protected virtual void Awake()
	{
		if (Instance == null)
		{
			Instance = this as T;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	#endregion
}
