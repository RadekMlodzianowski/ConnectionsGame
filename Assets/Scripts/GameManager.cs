using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }




	

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.Log("There is more than one GameManager instance!");
			Destroy(this);
			return;
		}

		Instance = this;
		// DontDestroyOnLoad(this); if the GameManager persist across scenes
	}

}
