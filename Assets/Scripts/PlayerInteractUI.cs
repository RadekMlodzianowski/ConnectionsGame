using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
   [SerializeField] private GameObject containerGameObject;
   //[SerializeField] private Player player; // not needed since we are using singleton
   [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

   private Player player;

	private void Start()
	{
      player = Player.Instance;   
	}

	private void Update()
	{
      if (player.GetInteractableGameObject() != null && !player.HasPickableObject())
      {
         Show(player.GetInteractableGameObject());
      }
      else
      {
         Hide();
      }
	}

	private void Show(IInteractable interactable)
   {
      containerGameObject.SetActive(true);
      interactTextMeshProUGUI.text = interactable.GetInteractText();
   }

   private void Hide()
   {
      containerGameObject.SetActive(false);
   }

}
