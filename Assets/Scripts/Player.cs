using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPickableObjectParent
{
    public static Player Instance { get; private set; }	

	[SerializeField] private float moveSpeed = 10f;
	[SerializeField] private float interactionRange = 2f;
   [SerializeField] private GameInput gameInput;
	[SerializeField] CharacterController characterController;
	[SerializeField] private ParticleSystem walkingParticleSystem;

	[SerializeField] private bool isWalking;
	
	[SerializeField] private bool isGrounded;

	[SerializeField] private Transform pickupHoldPoint;

	public PickableObject pickableObject;

	[SerializeField] private float gravityValue = -9.81f;
	private Vector3 playerVelocity;

	// flaga informuj�ca, �e jeste�my na ekranie �adowania
	private bool isInLoadingScene = false;
	// zapami�tujemy stan CharacterController przed wej�ciem w LoadingScene, aby poprawnie go przywr�ci�
	private bool characterControllerWasEnabledBeforeLoading = false;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.Log("There is more than one Player instance! Default Player instance destroyed"); 
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;		
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{	
		// Je�li to ekran �adowania � zatrzymaj fizyk�/upadanie gracza i zapami�taj stan kontrolera
		// Dodatkowo: je�li gracz trzyma PickableObject -> odczepiamy je i czy�cimy referencj�,
		// �eby obiekt nie pozosta� jako child obiektu oznaczonego jako DontDestroyOnLoad i nie "spada�" mi�dzy scenami.
		if (scene.name == "LoadingScene" || scene.name == "MainMenuScene")
		{
			isInLoadingScene = true;
			playerVelocity = Vector3.zero;
			if (characterController != null)
			{
				characterControllerWasEnabledBeforeLoading = characterController.enabled;
				characterController.enabled = false;
			}

			// Je�li gracz trzyma pickable object - odczepiamy go i zabezpieczamy physics tak, aby nie spada�
			if (HasPickableObject())
			{
				PickableObject carried = GetPickableObject();
				if (carried != null)
				{
					// odczep od Playera
					carried.transform.SetParent(null);

					// umie�� chwilowo w obiekcie sceny, �eby obiekt nie pozosta� wewn�trz DontDestroyOnLoad hierarchii
					GameObject tempHolder = GameObject.Find("TempPickableHolder");
					if (tempHolder == null)
					{
						tempHolder = new GameObject("TempPickableHolder");
						// NOTE: nie wywo�ujemy DontDestroyOnLoad na tempHolder � niech zostanie zniszczony razem ze scen� �adowania
					}
					carried.transform.SetParent(tempHolder.transform, worldPositionStays: true);

					// zabezpiecz physics podczas ekranu �adowania
					Rigidbody rb = carried.GetComponent<Rigidbody>();
					if (rb != null)
					{
						rb.isKinematic = true;
						rb.useGravity = false;
						rb.linearVelocity = Vector3.zero;
						rb.angularVelocity = Vector3.zero;
					}
					Collider col = carried.GetComponent<Collider>();
					if (col != null)
					{
						col.enabled = false;
					}
				}

				// Czy�cimy referencj� w Playerze � obiekt ma zosta� obs�u�ony przez scen� �adowania / zostanie zniszczony przy za�adowaniu nowej sceny
				ClearPickableObject();
			}

			return;
		}

		// opuszczamy LoadingScene -> wy��czamy flag�
		isInLoadingScene = false;

		// Dla wszystkich innych scen pr�bujemy ustawi� pozycj� na SpawnPoint
		GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");
		if (spawnPoint != null)
		{
			// Wy��cz kontroler przed teleportacj�, �eby unikn�� niepozadanych przesuniec
			bool wasEnabled = characterController == null ? false : characterController.enabled;
			if (characterController != null && characterController.enabled)
			{
				characterController.enabled = false;
			}

			transform.position = spawnPoint.transform.position;

			// Ustaw rotacj� gracza na rotacj� spawnPoint, ale wymu� Y = 90�
			Quaternion spawnRotation = spawnPoint.transform.rotation;
			Vector3 spawnEuler = spawnRotation.eulerAngles;
			spawnEuler.y = 90f;
			transform.rotation = Quaternion.Euler(spawnEuler);

			// zerujemy pr�dko�� aby grawitacja z LoadingScene nie "przeskoczy�a"
			playerVelocity = Vector3.zero;

			// przywr�� CharacterController je�li by� w��czony wcze�niej albo by� w��czony przed LoadingScene
			if (characterController != null)
			{
				if (characterControllerWasEnabledBeforeLoading || wasEnabled)
				{
					characterController.enabled = true;
				}
				// po przywr�ceniu nie musimy ju� zachowywa� starego stanu
				characterControllerWasEnabledBeforeLoading = false;
			}
		}
		else
		{
			// Brak SpawnPoint � upewnij si�, �e CharacterController nie pozostanie wy��czony (np. po LoadingScene)
			if (characterController != null)
			{
				if (characterControllerWasEnabledBeforeLoading && !characterController.enabled)
				{
					characterController.enabled = true;
				}
				characterControllerWasEnabledBeforeLoading = false;
			}
		}
	}

	private void Start()
	{
		gameInput.OnInteractAction += GameInput_OnInteractAction;
	}

	private void GameInput_OnInteractAction(object sender, System.EventArgs e)
	{
		CheckForInteractions();		
	}

	private void Update()
	{
		// na ekranie �adowania nie wykonujemy logiki ruchu (zapobiega "spadaniu" i wywo�aniom Move na nieaktywnym kontrolerze)
		if (isInLoadingScene) return;

		HandleMovement();
		
	}

	public bool IsWalking()
	{
		return isWalking;
	}	

	private void HandleMovement()
	{
		float rotateSpeed = 15f;

		Vector2 inputVector = gameInput.GetMovementVectorNormalized();
		Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);

		// isWalking = moveDir != Vector3.zero; (zamiast ujmowanie tego w "if...:" ni�ej

		if (move != Vector3.zero)
		{
			isWalking = true;
			// Apply rotation
			transform.forward = Vector3.Slerp(transform.forward, move, Time.deltaTime * rotateSpeed);
			if (walkingParticleSystem != null) walkingParticleSystem.gameObject.SetActive(true);	
		}
		else
		{
			isWalking = false;
			if (walkingParticleSystem != null) walkingParticleSystem.gameObject.SetActive(false);
		}

		//Apply gravity
		playerVelocity.y += gravityValue * Time.deltaTime;

		Vector3 finalMove = move * moveSpeed + Vector3.up * playerVelocity.y;

		if (characterController != null)
		{
			// Preferuj CharacterController, ale tylko gdy aktywny
			if (characterController.enabled)
			{
				characterController.Move(finalMove * Time.deltaTime);
			}
			else
			{
				// Controller istnieje, ale jest wy��czony (np. podczas LoadingScene) �
				// najlepiej NIE wykonywa� ruchu (unika b��d�w i nieoczekiwanych przesuni��).
				// Je�li chcesz teleportowa� obiekt mimo to, odkomentuj lini� poni�ej:
				// transform.position += finalMove * Time.deltaTime;
			}
		}
		else
		{
			// Brak CharacterController � fallback do transform (brak kolizji)
			transform.position += finalMove * Time.deltaTime;
		}
	}


	// Interact with the closes interactable gameobject
	private void CheckForInteractions()
	{
		IInteractable interactable;
		
		
		if (!HasPickableObject()) // if the player does not carry anything let him get interactable object from Physics function
		{
			interactable = GetInteractableGameObject(interactionRange);
		}
		else // if the player is carrying something sign it as an interactable object without Physics function
		{
			interactable = GetPickableObject();			
		}
			
		
		if (interactable != null)
		{
			interactable.Interact();
		}		
				
	}

	// Search for closest interactable gameobject
	public IInteractable GetInteractableGameObject(float interactRange)
	{
		List<IInteractable> interactableList = new List<IInteractable>();
		// float interactRange = 2f;
		Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
		foreach (Collider collider in colliderArray)
		{
			if (collider.TryGetComponent(out IInteractable interactable))
			{
				interactableList.Add(interactable);
			}
		}		

		IInteractable closestInteractable = null;
		foreach (IInteractable interactable in interactableList)
		{
			if (closestInteractable == null)
			{
				closestInteractable = interactable;
			}
			else
			{
				if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
					Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
				{
					// Closer
					closestInteractable = interactable;
				}
			}
		}
		return closestInteractable;
	}


	public Transform GetPickableObjectHoldPointTransform()
	{
		return pickupHoldPoint;
	}

	public void SetPickableObject(PickableObject pickableObject)
	{
		this.pickableObject = pickableObject;
	}

	public PickableObject GetPickableObject()
	{
		return pickableObject;
	}

	public void ClearPickableObject()
	{
		pickableObject = null;
	}

	public bool HasPickableObject()
	{
		return pickableObject != null;
	}

	public Transform GetTransform()
	{
		return transform;
	}

}
