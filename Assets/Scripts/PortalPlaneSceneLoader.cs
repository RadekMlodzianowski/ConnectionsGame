using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalPlaneSceneLoader : MonoBehaviour
{
	[SerializeField] private GameObject endGameCanvas; // przypisz w Inspectorze Canvas z przyciskiem powrotu do MainMenu
	[SerializeField] private float loadingDelaySeconds = 1f;

	private bool endGameShown = false;

	private void Start()
	{
		// upewnij siê, ¿e canvas endgame jest niewidoczny na starcie sceny
		if (endGameCanvas != null)
			endGameCanvas.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		// dla CharacterController rekomendujemy trigger zamiast OnCollisionEnter
		if (!other.gameObject.CompareTag("Player")) return;

		Debug.Log("OnTriggerEnter: Player entered portal trigger");

		// jeœli endgame ju¿ pokazany, ignorujemy kolejne wejœcia
		if (endGameShown) return;

		string currentSceneName = SceneManager.GetActiveScene().name;

		// Spróbuj sparsowaæ nazwê sceny do Loader.Scene (z nazwy sceny zrobiæ enum)
		if (Enum.TryParse<Loader.Scene>(currentSceneName, out Loader.Scene currentScene))
		{
			Array values = Enum.GetValues(typeof(Loader.Scene));
			int totalScenes = values.Length;

			// znajdŸ indeks bie¿¹cej sceny w tablicy wartoœci enuma (bez za³o¿eñ o ci¹g³oœci wartoœci)
			int currentIndex = Array.IndexOf(values, currentScene);
			int nextIndex = currentIndex + 1;

			// jeœli nie ma nastêpnej sceny -> traktujemy to jako koniec gry
			if (currentIndex < 0 || nextIndex >= totalScenes)
			{
				ShowEndGame();
				return;
			}

			Loader.Scene nextScene = (Loader.Scene)values.GetValue(nextIndex);

			// Je¿eli nastêpn¹ scen¹ jest LoadingScene (LoadingScene ma byæ traktowana jako separator/ostatnia) -> endgame
			if (nextScene == Loader.Scene.LoadingScene)
			{
				ShowEndGame();
				return;
			}

			// W przeciwnym razie ³adujemy kolejn¹ scenê z ekranem ³adowania
			Loader.LoadWithLoadingScreen(nextScene, loadingDelaySeconds);
		}
		else
		{
			Debug.LogWarning($"PortalPlaneSceneLoader: nie mo¿na sparsowaæ bie¿¹cej sceny '{currentSceneName}' do Loader.Scene.");
		}
	}

	private void ShowEndGame()
	{
		if (endGameCanvas != null)
		{
			endGameCanvas.SetActive(true);
			endGameShown = true;
			Time.timeScale = 0f; // zatrzymaj rozgrywkê, aby gracz móg³ korzystaæ z EndGame UI
		}
		else
		{
			// fallback: brak EndGame UI -> wróæ do MainMenu
			Debug.LogWarning("PortalPlaneSceneLoader: EndGameCanvas nie przypisany, ³adujê MainMenuScene jako fallback.");
			Loader.Load(Loader.Scene.MainMenuScene);
		}
	}
}
