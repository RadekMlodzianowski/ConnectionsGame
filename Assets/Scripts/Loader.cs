using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// skrypt do ³adowania scen (jest statyczny - nie przypisany do obiektu i nie dziedziczy po Monobehaviour)
// patrz skrypt PortalPlaneSceneLoader
public static class Loader
{
	public enum Scene // enum ¿eby nie u¿ywaæ stringów
	{		
		MainMenuScene,
		Level_01,
		Level_02,
		Level_03,
		Level_04,
		Level_05,
		LoadingScene
	}

	private static Scene targetScene;

	// zwyk³e ³adowanie, bez loading screena
	public static void Load(Scene targetScene) 
	{
		Loader.targetScene = targetScene;
		SceneManager.LoadScene(targetScene.ToString());		
	}
	

	// £aduje scenê "LoadingScene" i po delay sekundach przechodzi do targetScene
	public static void LoadWithLoadingScreen(Scene targetScene, float delaySeconds)
	{
		Loader.targetScene = targetScene;

		// stwórz pomocniczy GameObject, który przetrwa miêdzy scenami i uruchomi coroutine
		GameObject runner = new GameObject("LoaderRunner");
		Object.DontDestroyOnLoad(runner);

		// dodajemy komponent LoadRunner do obiektu runner
		LoaderRunner comp = runner.AddComponent<LoaderRunner>();
		comp.StartLoading("LoadingScene", targetScene.ToString(), delaySeconds);
	}

	
	// MonoBehaviour pomocniczy do uruchamiania coroutine z opóŸnieniem (klasa wewnêtrzna), courutine nie mo¿na odpaliæ z poziomu klasy statycznej
	private class LoaderRunner : MonoBehaviour
	{
		public void StartLoading(string loadingSceneName, string finalSceneName, float delaySeconds)
		{
			StartCoroutine(LoadingCoroutine(loadingSceneName, finalSceneName, delaySeconds));
		}

		private IEnumerator LoadingCoroutine(string loadingSceneName, string finalSceneName, float delaySeconds)
		{
			// najpierw przejdŸ do sceny Loading
			SceneManager.LoadScene(loadingSceneName);

			// odczekaj okreœlony czas (np. 1s)
			yield return new WaitForSeconds(delaySeconds);

			// za³aduj docelow¹ scenê
			SceneManager.LoadScene(finalSceneName);

			// posprz¹taj runner
			Destroy(gameObject);
		}
	}
}
