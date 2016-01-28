using UnityEngine;
using System.Collections;

public class EndDamageScript : MonoBehaviour {

	void OnColliderEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "AI")
            this.GetComponentInChildren<ParticleSystem>().Play();
    }
}
