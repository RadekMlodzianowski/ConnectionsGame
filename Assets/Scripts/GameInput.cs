using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameInput : MonoBehaviour
{
   public static GameInput Instance { get; private set; }

	public event EventHandler OnInteractAction;

   InputSystem_Actions inputSystem_actions;

	private void Awake()
	{
		Instance = this;

		inputSystem_actions = new InputSystem_Actions();

		inputSystem_actions.Enable();

		inputSystem_actions.Player.Interact.performed += Interact_performed;

	}

	private void Interact_performed(InputAction.CallbackContext obj)
	{
		OnInteractAction?.Invoke(this, EventArgs.Empty);		
	}

	public Vector2 GetMovementVectorNormalized()
	{
		Vector2 inputVector = inputSystem_actions.Player.Move.ReadValue<Vector2>();
		inputVector = inputVector.normalized;

		return inputVector;
	}


}
