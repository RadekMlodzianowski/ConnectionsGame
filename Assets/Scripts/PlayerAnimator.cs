using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
   private const string IS_WALKING = "IsWalking";
	private const string IS_HOLDING = "IsHolding";

   [SerializeField] Player player;
   private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();

	}

	private void Update()
	{
		animator.SetBool(IS_WALKING, player.IsWalking());
		animator.SetBool(IS_HOLDING, player.HasPickableObject());
	}


}
