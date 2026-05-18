using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// Script for portals. It mainly checks if all the conditions are met for portal to open.
public class Portal : MonoBehaviour
{
   [SerializeField] private GameObject portalPlane;
	// [SerializeField] private float portalSpawnDelay = 1f;
	// private float timer = 0f;

   public DroppableObject[] droppableObjectsArray;
	public bool areAllPickDropMatched = false;
	
	
	private void Update()
	{
		CheckForPickDropMatches();
		/*
		timer += Time.deltaTime;
		if (timer >= portalSpawnDelay)
		{
			TogglePortalPlane();
			timer = 0f;
		}
		*/
		TogglePortalPlane();
	}

	private void CheckForPickDropMatches()
	{
		// if even one of droppableObjects booleans is false then the foreach loop never reaches true for all of them
		foreach (DroppableObject droppableObject in droppableObjectsArray)
		{
			if (!droppableObject.isPickDropMatched)
			{
				areAllPickDropMatched = false;
				return;
			}			
		}
		areAllPickDropMatched = true;
	}

	private void TogglePortalPlane()
	{
		if (areAllPickDropMatched)
		{
			portalPlane.gameObject.SetActive(true);
		}
		else
		{
			portalPlane.gameObject.SetActive(false);
		}
	}

	

}
