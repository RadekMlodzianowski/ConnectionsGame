using UnityEngine;

public class InteractableObjectCanvas : MonoBehaviour
{
   [SerializeField] private GameObject canvasChildrenObjects;   
   [SerializeField] private Player player;
	[SerializeField] private float verticalOffset = 1.5f;

	//private IInteractable currentTarget;

	[SerializeField] private float showCanvasRange = 7f;
	[SerializeField] private float rotateCanvasRange = 2f;
	[SerializeField] private float rotationAngleDegrees = 45f;

	private Quaternion originalRotation;
	private bool isRotated = false;

	private void Start()
	{
		player = Player.Instance;
		originalRotation = transform.rotation;
	}

	private void Update()
	{
		// Pobierz najbli¿szy interactable w zasiêgu "showCanvasRange"
		IInteractable target = player.GetInteractableGameObject(showCanvasRange);

		if (target != null && !player.HasPickableObject())
		{
			//currentTarget = target;
			Transform t = target.GetTransform();
			transform.position = t.position + Vector3.up * verticalOffset;
			Show();

			// oblicz odleg³oœæ od gracza do obiektu i zastosuj obrót jeœli < rotateRange
			float distance = Vector3.Distance(player.GetTransform().position, t.position);

			// Obrót gdy gracz jest bli¿ej ni¿ "rotateCanvasRange"
			if (distance < rotateCanvasRange)
			{
				if (!isRotated)
				{
					transform.rotation = originalRotation * Quaternion.Euler(0f, 0f, rotationAngleDegrees);
					isRotated = true;
				}
			}
			else
			{
				if (isRotated)
				{
					transform.rotation = originalRotation;
					isRotated = false;
				}
			}
		}
		else
		{
			Hide();
		}
	}
	
	private void Show()
	{
		canvasChildrenObjects.gameObject.SetActive(true);
	}

	private void Hide()
	{
		canvasChildrenObjects.gameObject.SetActive(false);

		// przy ukryciu przywróæ oryginaln¹ rotacjê
		if (isRotated)
		{
			transform.rotation = originalRotation;
			isRotated = false;
		}
	}
}
