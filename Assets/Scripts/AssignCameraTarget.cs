using Unity.Cinemachine;
using UnityEngine;

public class AssignCameraTarget : MonoBehaviour
{
	private void Start()
	{
		CinemachineCamera vcam =
			 GetComponent<CinemachineCamera>();

		if (Player.Instance != null)
		{
			vcam.Follow = Player.Instance.transform;
			vcam.LookAt = Player.Instance.transform;
		}
	}
}
