using Unity.VisualScripting;
using UnityEngine;

// Script for moving elements of the level caused by interactions
public class LevelElementsSwitchUpdated : MonoBehaviour, IInteractable
{
	[SerializeField] private string interactText;

	public GameObject[] movingElements;

	[SerializeField] private float elementMoveSpeed = 5f;
	[SerializeField] private float moveDistance = 10f;

	private Vector3 targetPosition1;
	private Vector3 targetPosition2;

	// [SerializeField] private bool hasInteracted = false;
	[SerializeField] private bool isMovingDown = false;
	[SerializeField] private bool isMovingUp = false;

	// toggle: false = default (element0 -> down, element1 -> up)
	// true = reversed (element0 -> up, element1 -> down)
	private bool toggled = false;

	
	private void Update()
	{
		// bezpieczne wywo³ania z kontrol¹ d³ugoœci tablicy
		if (movingElements != null && movingElements.Length > 0 && movingElements[0] != null)
			MoveElementDown(movingElements[0]);

		if (movingElements != null && movingElements.Length > 1 && movingElements[1] != null)
			MoveElementUp(movingElements[1]);
	}

	public void Interact()
	{
		// zignoruj interakcjê jeœli elementy s¹ ju¿ w ruchu
		if (isMovingDown || isMovingUp) return;
		if (movingElements == null) return;

		// ustaw cele zale¿nie od aktualnego stanu toggle (toggled)
		if (movingElements.Length > 0 && movingElements[0] != null)
		{
			// jeœli toggled == false: element0 idzie w dó³, jeœli true: idzie w górê
			targetPosition1 = movingElements[0].transform.position + (toggled ? Vector3.up * moveDistance : -Vector3.up * moveDistance);
			isMovingDown = true; // u¿ywamy tej flagi jako "element0 jest w ruchu"
		}

		if (movingElements.Length > 1 && movingElements[1] != null)
		{
			// jeœli toggled == false: element1 idzie w górê, jeœli true: idzie w dó³
			targetPosition2 = movingElements[1].transform.position + (toggled ? -Vector3.up * moveDistance : Vector3.up * moveDistance);
			isMovingUp = true; // u¿ywamy tej flagi jako "element1 jest w ruchu"
		}

		// hasInteracted = isMovingDown || isMovingUp;

		// odwróæ stan tak, aby kolejna Interact() wykona³a ruch w przeciwnych kierunkach
		toggled = !toggled;
	}

	public void MoveElementDown(GameObject movingElement)
	{
		if (!isMovingDown || movingElement == null) return;

		movingElement.transform.position = Vector3.MoveTowards(movingElement.transform.position, targetPosition1, elementMoveSpeed * Time.deltaTime);
		if (movingElement.transform.position == targetPosition1)
		{
			isMovingDown = false;			
		}
	}

	public void MoveElementUp(GameObject movingElement)
	{
		if (!isMovingUp || movingElement == null) return;

		movingElement.transform.position = Vector3.MoveTowards(movingElement.transform.position, targetPosition2, elementMoveSpeed * Time.deltaTime);
		if (movingElement.transform.position == targetPosition2)
		{
			isMovingUp = false;			
		}
	}


	public string GetInteractText()
	{
		return interactText;
	}

	public Transform GetTransform()
	{
		return transform;
	}
}
