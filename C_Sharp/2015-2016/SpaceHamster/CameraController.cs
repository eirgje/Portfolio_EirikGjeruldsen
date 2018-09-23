using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public float smoothness = 1.5f;

	public Transform player;
	private Vector3 relCamPos;
	private float relCamPosMag;
	private Vector3 newPos;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		relCamPos = transform.position - player.position;
		relCamPosMag = relCamPos.magnitude - 0.5f;
	}

	void FixedUpdate()
	{
		Vector3 standardPos = player.position + relCamPos;
		Vector3 abovePos = player.position + Vector3.up * relCamPosMag;

		Vector3[] checkPoints = new Vector3[5];
		checkPoints[0] = standardPos;
		checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
		checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
		checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
		checkPoints[4] = abovePos;

		for (int i = 0; i < checkPoints.Length; i++)
		{
			if(ViewingPosCheck(checkPoints[i]))
			{
				break;
			}
		}

		transform.position = Vector3.Lerp(transform.position, newPos, smoothness * Time.deltaTime);
		SmoothLookAt();
	}

	bool ViewingPosCheck(Vector3 checkPos)
	{
		RaycastHit hit;

		if (Physics.Raycast(checkPos, player.position - checkPos, out hit, relCamPosMag))
		{
			if (hit.transform != player)
			{
				return false;
			}
		}

		newPos = checkPos;
		return true;
	}

	void SmoothLookAt()
	{
		Vector3 relPlayerPos = player.position - transform.position;
		Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPos, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smoothness * Time.deltaTime);
	}
}

