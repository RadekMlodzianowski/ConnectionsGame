using UnityEngine;

public class LeverInteractable : MonoBehaviour, IInteractable
{
	[SerializeField] Chest chest;

	private Animator animator;
	private bool isLeverPulled;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}


	public void Interact()
	{
		chest.ToggleChest();
		ToggleLever();		
	}

	private void ToggleLever()
	{
		isLeverPulled = !isLeverPulled;
		animator.SetBool("IsLeverPulled", isLeverPulled);
	}

	public string GetInteractText()
	{
		return "Pull Lever";
	}

	public Transform GetTransform()
	{
		return transform;
	}
		
}
