using UnityEngine;
using System.Collections;

public class TowerCollisionDetector : MonoBehaviour {

	public bool badSpot = false;
	private int tCount = 0;

	void OnTriggerEnter(Collider other)
	{
		if (other.collider.tag == "Interface Collider")
			++tCount;
		if (tCount > 0)
			badSpot = true;
	}

	void OnTriggerExit(Collider other)
	{
		if (other.collider.tag == "Interface Collider")
			--tCount;

		if (tCount <= 0)
			badSpot = false;
	}
}
