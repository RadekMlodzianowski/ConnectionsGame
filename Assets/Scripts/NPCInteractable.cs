using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
   [SerializeField] private Transform spawnableObject;
   [SerializeField] private string interactText;

   private Vector3 spawnOffset = new Vector3(0f, 3f, 0f);


   public void Interact()
   {
      SpawnObject();
   }


   // Spawning heart (problem: you can spawn many hearts mashing the button)
   public void SpawnObject()
   {
      Transform spawnedObject = Instantiate(spawnableObject, transform.position + spawnOffset, Quaternion.identity);      
      Destroy(spawnedObject.gameObject, 2f);
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
