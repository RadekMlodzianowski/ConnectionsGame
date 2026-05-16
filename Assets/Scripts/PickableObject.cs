using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Script for all of gameobjects that can be picked up and hold
public class PickableObject : MonoBehaviour, IInteractable
{
	
	Rigidbody rigidbidy;
	Collider pickableCollider;
	
	

	private void Start()
	{
		rigidbidy = GetComponent<Rigidbody>();
		pickableCollider = GetComponent<Collider>();
	}


	public void Interact()
	{
				
		if (!Player.Instance.HasPickableObject()) // Player is not carrying anything.
		{
			PickUp();
		}
		else // Player is carrying something.
		{
			if (Player.Instance.HasPickableObject())
			{
				Drop();
			}
		}
	}
	

	private void PickUp()
	{
		// disable physics
		rigidbidy.isKinematic = true;
		rigidbidy.useGravity = false;

		pickableCollider.enabled = false;

		this.transform.SetParent(Player.Instance.GetPickableObjectHoldPointTransform());
		this.transform.localPosition = Vector3.zero;
		this.transform.localRotation = Quaternion.identity;

		Player.Instance.SetPickableObject(this);
		
	}

	private void Drop()
	{
		IPickableObjectParent droppable;		
		
		// If there is no droppable gameobjects near, put the carried gameobject on the ground.
		if (GetDroppableObject() == null)
		{
			this.transform.SetParent(null);
		}
		else // If there is droppable gameobject nearby put the carried gameobject on it (for ex. on a platform).
		{
			droppable = GetDroppableObject();			
			this.transform.SetParent(droppable.GetPickableObjectHoldPointTransform());
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;

			droppable.SetPickableObject(this);
			
		}
		

		// enable physics
		rigidbidy.isKinematic = false;
		rigidbidy.useGravity = true;

		pickableCollider.enabled = true;

		Player.Instance.SetPickableObject(null);		

		// small forward toss
		// rigidbidy.AddForce(transform.forward * tossStrength, ForceMode.Impulse);
	}

	// Get the nearest droppable object.
	public IPickableObjectParent GetDroppableObject()
	{
		List<IPickableObjectParent> droppableList = new List<IPickableObjectParent>();
		float interactRange = 2f;
		LayerMask detectionMask = LayerMask.GetMask("Droppable");
		Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange, detectionMask);
		foreach (Collider collider in colliderArray)
		{
			if (collider.TryGetComponent(out IPickableObjectParent droppable))
			{
				droppableList.Add(droppable);
			}
		}

		IPickableObjectParent closestDroppable = null;
		foreach (IPickableObjectParent droppable in droppableList)
		{
			if (closestDroppable == null)
			{
				closestDroppable = droppable;
			}
			else
			{
				if (Vector3.Distance(transform.position, droppable.GetTransform().position) <
					Vector3.Distance(transform.position, closestDroppable.GetTransform().position))
				{
					// Closer
					closestDroppable = droppable;
				}
			}
		}		
		return closestDroppable;
	}


	public string GetInteractText()
	{
		return "Pickup";
	}

	public Transform GetTransform()
	{
		return transform;
	}
}
