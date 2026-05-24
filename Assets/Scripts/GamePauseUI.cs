using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
   [SerializeField] private Button resumeButton; 
   [SerializeField] private Button mainMenuButton;

	private void Awake()
	{
		// u¿ywamy nazwanych metod zamiast lambd, ¿eby móc je potem usun¹æ
		if (resumeButton != null) resumeButton.onClick.AddListener(ResumeButtonClicked);
		if (mainMenuButton != null) mainMenuButton.onClick.AddListener(MainMenuButtonClicked);		

	}

	private void Start()
	{
		if (GameManager.Instance != null)
		{
			GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
			GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
		}

		Hide();
	}

	private void OnDestroy()
	{
		// odsubskrybuj eventy i usuñ listenery — zapobiega wywo³ywaniu metod na zniszczonym obiekcie
		if (GameManager.Instance != null)
		{
			GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
			GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
		}

		if (resumeButton != null) resumeButton.onClick.RemoveListener(ResumeButtonClicked);
		if (mainMenuButton != null) mainMenuButton.onClick.RemoveListener(MainMenuButtonClicked);
	}

	private void ResumeButtonClicked()
	{
		GameManager.Instance?.TogglePauseGame();
	}

	private void MainMenuButtonClicked()
	{
		Loader.Load(Loader.Scene.MainMenuScene);
	}

	private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
	{
		Hide();
	}

	private void GameManager_OnGamePaused(object sender, System.EventArgs e)
	{
		Show();		
	}

	private void Show()
	{
		gameObject.SetActive(true);
		resumeButton?.Select(); // zaznaczamy pierwszy przycisk, potrzebne do obs³ugi kontrolera (prawdopodobnie nie dzia³a)
		
	}

	private void Hide()
	{
		gameObject.SetActive(false);
	}
}
