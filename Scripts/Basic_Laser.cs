using UnityEngine;
using System.Collections;

public class Basic_Laser : MonoBehaviour {

	public LayerMask LayerMask;

	public float LaserScale;    
	private float laserLength;
	public float MaxLaserLength;


	public Transform laserImpact;
	public Transform laserMuzzle;

	LineRenderer lineRenderer;
	RaycastHit hitPoint;
	

	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}


	void FixedUpdate(){
		Raycast();	
	}

	

	void Raycast(){

		hitPoint = new RaycastHit();
		Ray ray = new Ray(transform.position, transform.forward);


		float propMult = MaxLaserLength * (LaserScale / 10f);

		if (laserMuzzle)
			laserMuzzle.position = transform.position;
            
            lineRenderer.SetPosition(0, ray.origin);

		if (Physics.Raycast(ray, out hitPoint, MaxLaserLength, LayerMask))
		{
            laserImpact.GetComponentInChildren<ParticleSystem>().Play();

			laserLength = Vector3.Distance(transform.position, hitPoint.point);
			lineRenderer.SetPosition(1, hitPoint.point);

			propMult = laserLength * (LaserScale / 10f);

		   //hitPoint.rigidbody.AddForceAtPosition(transform.forward * 300, hitPoint.point, ForceMode.Force);


			if (laserImpact)
				laserImpact.position = hitPoint.point - transform.forward * 0.1f;
		}
		else
		{
			
		laserLength = MaxLaserLength;
        lineRenderer.SetPosition(1, ray.origin);

		if (laserImpact)
            laserImpact.GetComponentInChildren<ParticleSystem>().Pause();
			laserImpact.position = hitPoint.point + transform.forward * laserLength;
		}



		lineRenderer.material.SetTextureScale("_MainTex", new Vector2(propMult, 1f));
		lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * -3, 0));

	}

}
