using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
   [SerializeField] private Button playButton; 
   [SerializeField] private Button quitButton;

	private void Awake()
	{
		// u¿ywamy nazwanych metod aby móc je póŸniej usun¹æ
		if (playButton != null) playButton.onClick.AddListener(PlayButtonClicked);
		if (quitButton != null) quitButton.onClick.AddListener(QuitButtonClicked);

		Time.timeScale = 1f; // bo jak z pauzy klikamy w przycisk Main Menu to gra stoi i trzeba rêcznie przywróciæ TimeScale
	}

	private void OnDestroy()
	{
		if (playButton != null) playButton.onClick.RemoveListener(PlayButtonClicked);
		if (quitButton != null) quitButton.onClick.RemoveListener(QuitButtonClicked);
	}

	private void PlayButtonClicked()
	{
		// upewnij siê, ¿e referencja PickableObject w Playerze jest wyczyszczona przed rozpoczêciem gry
		if (Player.Instance != null && Player.Instance.HasPickableObject())
		{
			PickableObject carried = Player.Instance.GetPickableObject();
			if (carried != null)
			{
				// odczep obiekt od gracza i przywróæ podstawowe ustawienia fizyki
				carried.transform.SetParent(null);
				Rigidbody rb = carried.GetComponent<Rigidbody>();
				if (rb != null)
				{
					rb.isKinematic = false;
					rb.useGravity = true;
				}
				Collider col = carried.GetComponent<Collider>();
				if (col != null)
				{
					col.enabled = true;
				}
			}
			// czyœcimy pole w Playerze
			Player.Instance.ClearPickableObject();
		}		

		Loader.Load(Loader.Scene.Level_02);
	}

	private void QuitButtonClicked()
	{
		Application.Quit();
	}
}
