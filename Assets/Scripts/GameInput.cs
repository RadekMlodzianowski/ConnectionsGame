using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameInput : MonoBehaviour
{
   public static GameInput Instance { get; private set; }

	public event EventHandler OnInteractAction;
	public event EventHandler OnPauseAction;

   InputSystem_Actions inputSystem_actions;

	private void Awake()
	{
				
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(this);
		
		inputSystem_actions = new InputSystem_Actions();

		inputSystem_actions.Enable();

		inputSystem_actions.Player.Interact.performed += Interact_performed;
		inputSystem_actions.Player.Pause.performed += Pause_performed;
		
	}
	/*
	private void OnDestroy() // potrzebne gdy w w pause menu powracamy do main menu i wciskamy play ¿eby zagraæ od pocz¹tku
	{
		inputSystem_actions.Player.Interact.performed -= Interact_performed;
		inputSystem_actions.Player.Pause.performed -= Pause_performed;
	}
	*/

	private void Pause_performed(InputAction.CallbackContext obj)
	{
		OnPauseAction?.Invoke(this, EventArgs.Empty);
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
