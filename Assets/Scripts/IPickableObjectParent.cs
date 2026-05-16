using UnityEngine;


// Interface for gameobjects that other gameobject can be put on top off (include Player carrying things).
public interface IPickableObjectParent
{
	public Transform GetTransform();

	public Transform GetPickableObjectHoldPointTransform();

	public void SetPickableObject(PickableObject pickableObject);

	public PickableObject GetPickableObject();

	public void ClearPickableObject();

	public bool HasPickableObject();	
}
