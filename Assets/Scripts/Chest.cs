using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
   [SerializeField] GameObject chestLid;
	[SerializeField] private string interactText;
	private Animator animator;
	private bool isChestOpen;

	private void Awake()
	{
		animator = chestLid.GetComponent<Animator>();
	}

	public void Interact()
	{
		ToggleChest();
	}


	public void ToggleChest()
	{
		isChestOpen = !isChestOpen;
		animator.SetBool("IsChestOpen", isChestOpen);
		
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
