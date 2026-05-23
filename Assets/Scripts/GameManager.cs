using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

	public event EventHandler OnGamePaused;
	public event EventHandler OnGameUnpaused;

	[SerializeField] private bool isGamePaused = false;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.Log("There is more than one GameManager instance!");
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
	}

	private void GameInputOnPauseAction(object sender, EventArgs e)
	{
		TogglePauseGame();
	}

	public void TogglePauseGame()
	{
		isGamePaused = !isGamePaused;

		if (isGamePaused)
		{
			Time.timeScale = 0f;
			OnGamePaused?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			Time.timeScale = 1f;
			OnGameUnpaused?.Invoke(this, EventArgs.Empty);
		}
	}
}
