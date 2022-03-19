using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Axel
{
	Front,
	Rear
}
[Serializable]
public struct Wheel
{
	public GameObject model;
	public WheelCollider collider;
	public Axel axel;
}

public class Controller : MonoBehaviour
{


	public List<AudioSource> CarSound;
	private int currentGear;
	private float[] GearRatio = { 90.0f, 120.0f, 150.0f, 180.0f, 200.0f };
	[SerializeField] public float maxSpeed ;
	[SerializeField] private float Steer = 1f;
	[SerializeField] private float MaxSteerAngle = 15f;

	[SerializeField] private List<Wheel> wheels;

	private float inputX, inputY;

	int lane;
	
	
	public float currentSpeed;
	public Transform centerOfMass;
	private const float ROTATION_MIN = -20f;
	private const float ROTATION_MAX = 20f;
	public GameObject[] Target;
	bool projectileTriggered;
	public bool leftCheck;
	public bool rightCheck;
	public GameObject GameOver;
	public bool rearLight;
	public GameObject[] rearLights;
	Rigidbody _rb;


	public ParticleSystem GearSwitch1;
	public ParticleSystem GearSwitch2;
	public ParticleSystem GearSwitch3;
	public ParticleSystem GearSwitch4;



	void Start()
	{
		projectileTriggered = false;
		_rb = GetComponent<Rigidbody>();
		_rb.centerOfMass = centerOfMass.localPosition;
		leftCheck = false;
		rightCheck = false;
		
		for (int i = 1; i <= 5; ++i)
		{
			CarSound.Add(GameObject.Find(string.Format("CarSound ({0})", i)).GetComponent<AudioSource>());
			CarSound[i - 1].Play();
		}
		foreach (var rearLight in rearLights)
		{
			rearLight.SetActive(false);
		}

	}

	// Update is called once per frame
	public void FixedUpdate()
	{
		Move();
		
		Turn();
		Wheels();
		
		LookForward();
		Sensors();
		Direction();
		Gear();
		carSounds();
       
		if (currentSpeed < 64)
		{
			foreach (var wheel in wheels)
			{
				wheel.collider.motorTorque = 1 * maxSpeed * 500 *  Time.deltaTime;
				wheel.collider.brakeTorque = 0;
			}
		}

		currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;

		
	}
	public void Update()
    {
		Brake();
		Way();
	}


	private void Move()
	{
        if (!projectileTriggered)
        {
			if (currentSpeed < maxSpeed)
			{
				foreach (var wheel in wheels)
				{

					wheel.collider.motorTorque = maxSpeed * 250 * Time.deltaTime;
				}

			}
			else if (currentSpeed > maxSpeed)
			{
				foreach (var wheel in wheels)
				{

					wheel.collider.motorTorque = 0;
				}

			}
		}

        else
        {
			foreach (var wheel in wheels)
			{

				wheel.collider.brakeTorque = 500000f;
			}
		}
	}
	private void Brake()
    {
		if (currentSpeed > 64)
		{

			if (Input.touchCount>0)
			{
				foreach (var wheel in wheels)
				{
					FindObjectOfType<AudioManager>().Play("brake");
					wheel.collider.brakeTorque = 4000f;

				}
				foreach (var rearLight in rearLights)
				{
					rearLight.SetActive(true);
				}

			}

			else
			{
				if (Input.touchCount==0)
				{

					foreach (var rearLight in rearLights)
					{
						rearLight.SetActive(false);
					}
					foreach (var wheel in wheels)
					{

						wheel.collider.brakeTorque = 0f;
						wheel.collider.motorTorque = 1 * maxSpeed * 500 * Time.deltaTime;
						FindObjectOfType<AudioManager>().Stop("brake");

					}
				}
			}

			}
	}

	private void Gear()
	{
		float f = Mathf.Abs(currentSpeed / maxSpeed);

		
		float upGearlimit = (1 / (float)5) * (currentGear + 1);
		float downGearlitim = (1 / (float)5) * currentGear;

		if (currentGear > 0 && f < downGearlitim)
		{
			currentGear--;
		}
		if (f > upGearlimit && (currentGear < (4)))
		{
			currentGear++;
			FindObjectOfType<AudioManager>().Play("ExhaustBlow");
			GearSwitch1.Play();
			GearSwitch2.Play();
			GearSwitch3.Play();
			GearSwitch4.Play();
			
		}


	}

	void Turn()
	{
		foreach (var wheel in wheels)
		{
			if (wheel.axel == Axel.Front)
			{
				var _steerAngle = inputX * Steer * MaxSteerAngle;
				wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, _steerAngle, .1f);
			}
		}
	}



	private void Way()
	{
		inputX = Input.acceleration.x*30 ;
		
	}




	private void LookForward()
	{
		if (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
		{
			if (centerOfMass.position.x < -4.46f)
				lane = 0;
			if (centerOfMass.position.x > -4.46f && centerOfMass.position.x < 0)
				lane = 1;
			if (centerOfMass.position.x > 0 && centerOfMass.position.x < 4.5f)
				lane = 2;
			if (centerOfMass.position.x > 4.5f)
				lane = 3;

			foreach (var wheel in wheels)
			{
				if (wheel.axel == Axel.Front)
				{

					Vector3 relativeVector = transform.InverseTransformPoint(Target[lane].transform.position);
					float newSteer = (relativeVector.x / relativeVector.magnitude) * 40;

					wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, newSteer, .7f);


				}
			}
		}

	}

	private void Wheels()
	{
		foreach (var wheel in wheels)
		{
			Quaternion _rot;
			Vector3 _pos;

			wheel.collider.GetWorldPose(out _pos, out _rot);
			wheel.model.transform.position = _pos;
			wheel.model.transform.rotation = _rot;

		}
	}
	private void Direction()
	{
		Vector3 currentRotation = this.transform.eulerAngles;
		currentRotation.y = currentRotation.y % 360;
		if (currentRotation.y > 180)
			currentRotation.y -= 360f;
		currentRotation.y = Mathf.Clamp(currentRotation.y, ROTATION_MIN, ROTATION_MAX);
		this.transform.rotation = Quaternion.Euler(currentRotation);


	}



	public void Sensors()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0.5f), transform.right, out hit, 3))
		{

			if (hit.collider.tag == "EnemyCar1" && !rightCheck && currentSpeed >= 60)
			{
				Debug.DrawRay(transform.position + new Vector3(0, 0.3f, 0.5f), transform.right * hit.distance, Color.red);

				rightCheck = true;

			}


		}
		else
		{
			rightCheck = false;
		}

		if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0.5f), -transform.right, out hit, 3))
		{


			if (hit.collider.tag == "EnemyCar1" && !leftCheck && currentSpeed >= 60)
			{
				Debug.DrawRay(transform.position + new Vector3(0, 0.3f, 0.5f), -transform.right * hit.distance, Color.red);
				
				leftCheck = true;
			}


		}
		else
		{

			leftCheck = false;
		}
	}
	void carSounds()
	{
		float v = Mathf.Abs(currentSpeed / GearRatio[currentGear]);

		

				//Set CarSound[0]
				if (currentGear < 1)
				{
					CarSound[0].volume = 0.5f;
					CarSound[1].volume = 0.0f;
					CarSound[2].volume = 0.0f;
					CarSound[3].volume = 0.0f;
					CarSound[4].volume = 0.0f;
				}
				else if (currentGear < 2)
				{
					CarSound[0].volume = 1;
					float PitchMath = v/2f+0.3f;
					CarSound[0].pitch = PitchMath; 

					CarSound[1].volume = 0.0f;
					CarSound[2].volume = 0.0f;
					CarSound[3].volume = 0.0f;
					CarSound[4].volume = 0.0f;
				}
				else if (currentGear < 3)
				{
					CarSound[1].volume = 1;
					float PitchMath = v - 0.1f;
					CarSound[1].pitch = PitchMath;

					CarSound[0].volume = 0.0f;
					CarSound[2].volume = 0.0f;
					CarSound[3].volume = 0.0f;
					CarSound[4].volume = 0.0f;
				}
				else if (currentGear < 4)
				{
					CarSound[2].volume = 1;
					float PitchMath = v - 0.1f;
					CarSound[2].pitch = PitchMath;

					CarSound[1].volume = 0.0f;
					CarSound[0].volume = 0.0f;
					CarSound[3].volume = 0.0f;
					CarSound[4].volume = 0.0f;
				}
				else if (currentGear < 5)
				{
					CarSound[3].volume = 1;
					float PitchMath = v - 0.1f;
					CarSound[3].pitch = PitchMath;

					CarSound[1].volume = 0.0f;
					CarSound[0].volume = 0.0f;
					CarSound[2].volume = 0.0f;
					CarSound[4].volume = 0.0f;
				}

				else
				{
					CarSound[4].volume = 1;
					float PitchMath = v - 0.1f;
					CarSound[4].pitch = PitchMath;

					CarSound[1].volume = 0.0f;
					CarSound[0].volume = 0.0f;
					CarSound[2].volume = 0.0f;
					CarSound[3].volume = 0.0f;
				}
	}

	
	

	private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "EnemyCar1")
			{
			_rb.isKinematic = true;
			projectileTriggered = true;						
			GameOver.SetActive(true);
			FindObjectOfType<AudioManager>().Play("Crash");			
			
			}

		}
} 
