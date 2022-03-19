using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : MonoBehaviour
{
	Rigidbody body;
	public float liftCoefficient;

	void Start()
	{

		Transform trs = transform;
		while (trs != null && trs.GetComponent<Rigidbody>() == null)
			trs = trs.parent;

		if (trs != null)
			body = trs.GetComponent<Rigidbody>();
	}

	void Update()
	{

		if (body != null)
		{

			float lift = liftCoefficient * body.velocity.sqrMagnitude;
			body.AddForceAtPosition(lift * -transform.up, transform.position);
		}
	}
}
