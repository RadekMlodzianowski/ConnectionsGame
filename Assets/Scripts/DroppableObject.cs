using Unity.VisualScripting;
using UnityEngine;

// Script for gameobjects that you can put pickable gameobjects on top off (for ex. platforms).
public class DroppableObject : MonoBehaviour, IPickableObjectParent
{

	[SerializeField] private Transform pickupHoldPoint;
	[SerializeField] private PickableObject pickableObject;
	[SerializeField] private GameObject linkedGateway;
	
	public string platformColor;
	public bool isPickDropMatched = false;

	private void Update()
	{
		if (Player.Instance.pickableObject == this.pickableObject)
		{
			ClearPickableObject();
		}

		

		if (HasPickableObject())
		{
			if (platformColor == pickableObject.pickedObjectColor)
			{
				isPickDropMatched = true;
				if (linkedGateway != null)
				{
					linkedGateway.gameObject.SetActive(false);
				}
			}
			else
			{
				isPickDropMatched = false;
				if (linkedGateway != null)
				{
					linkedGateway.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			isPickDropMatched = false;
			if (linkedGateway != null)
			{
				linkedGateway.gameObject.SetActive(true);
			}
		}
	}

	// from interface:
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
