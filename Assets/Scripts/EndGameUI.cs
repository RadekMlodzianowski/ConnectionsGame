using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
   [SerializeField] private Button mainMenuButton;

	private void Awake()
	{
		// u¿ywamy nazwanych metod zamiast lambd, ¿eby móc je potem usun¹æ		
		if (mainMenuButton != null) mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
	}

	private void OnDestroy()
	{
		if (mainMenuButton != null) mainMenuButton.onClick.RemoveListener(MainMenuButtonClicked);
	}

	private void MainMenuButtonClicked()
	{
		Loader.Load(Loader.Scene.MainMenuScene);
	}

}
