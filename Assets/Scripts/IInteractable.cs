using UnityEngine;

// Interface for gameobject that Player can interact with directly
public interface IInteractable
{

   void Interact();
   string GetInteractText();
   Transform GetTransform();

}
