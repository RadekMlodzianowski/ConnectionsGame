using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private enum Mode
	{
		LookAt,
		LookAtInverted,
		CameraForward,
		CameraForwardInverted,
	}

	[SerializeField] private Mode mode;


	private void LateUpdate()
	{
		switch (mode)
		{
			case Mode.LookAt:
				transform.LookAt(Camera.main.transform);
				break;
			case Mode.LookAtInverted:
				// look from the opposite position (because the bars are inverted)
				Vector3 dirFromCamera = transform.position - Camera.main.transform.position; // Vector from camera to this object
				transform.LookAt(transform.position + dirFromCamera); // look at opposite position
				break;
			case Mode.CameraForward:
				transform.forward = Camera.main.transform.forward;
				break;
			case Mode.CameraForwardInverted:
				transform.forward = -Camera.main.transform.forward;
				break;

		}


	}
}

