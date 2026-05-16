using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour, IPickableObjectParent
{
    public static Player Instance { get; private set; }

	

	[SerializeField] private float moveSpeed = 10f;
   [SerializeField] private GameInput gameInput;
	[SerializeField] CharacterController characterController;

	[SerializeField] private bool isWalking;
	[SerializeField] private bool isGrounded;

	[SerializeField] private Transform pickupHoldPoint;

	public PickableObject pickableObject;

	private float gravityValue = -9.81f;
	private Vector3 playerVelocity;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("There is more than one Player instance!"); // Destroy(this); ??
		}

		Instance = this;
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
		HandleMovement();
	}

	public bool IsWalking()
	{
		return isWalking;
	}

	private void HandleMovement()
	{
		float rotateSpeed = 10f;

		Vector2 inputVector = gameInput.GetMovementVectorNormalized();
		Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);

		// isWalking = moveDir != Vector3.zero; (zamiast ujmowanie tego w "if...:" ni¿ej

		if (move != Vector3.zero)
		{
			isWalking = true;
			transform.forward = Vector3.Slerp(transform.forward, move, Time.deltaTime * rotateSpeed);
		}
		else
		{
			isWalking = false;
		}

		//Apply gravity
		playerVelocity.y += gravityValue * Time.deltaTime;

		Vector3 finalMove = move * moveSpeed + Vector3.up * playerVelocity.y;
		characterController.Move(finalMove * Time.deltaTime);

	}


	// Interact with the closes interactable gameobject
	private void CheckForInteractions()
	{
		IInteractable interactable;
		
		
		if (!HasPickableObject()) // if the player does not carry anything let him get interactable object from Physics function
		{
			interactable = GetInteractableGameObject();
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
	public IInteractable GetInteractableGameObject()
	{
		List<IInteractable> interactableList = new List<IInteractable>();
		float interactRange = 4f;
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
