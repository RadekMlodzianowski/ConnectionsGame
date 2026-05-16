using UnityEngine;

public class Chest : MonoBehaviour
{
   [SerializeField] GameObject chestLid;
   private Animator animator;
	private bool isChestOpen;

	private void Awake()
	{
		animator = chestLid.GetComponent<Animator>();
	}

	public void ToggleChest()
	{
		isChestOpen = !isChestOpen;
		animator.SetBool("IsChestOpen", isChestOpen);

	}


}
