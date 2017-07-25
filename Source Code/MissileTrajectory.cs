using UnityEngine;
using System.Collections;

public class MissileTrajectory : MonoBehaviour {


	public GameObject explosion;
	Object thisExplosion;
    public AudioSource explosionSound;
    private float        missileLifetime;
    private float lifetimeThreshold = 2.5f;
   // public GameObject smallRocks;
    public CarControlScript carControl;
    

	// Use this for initialization
	void Start () {
        explosionSound = GameObject.FindGameObjectWithTag("EXPLOSIONSOUND").GetComponent<AudioSource>();
       // smallRocks = GameObject.FindGameObjectWithTag("SMALLROCKS");
        carControl = GameObject.FindGameObjectWithTag("CAR").GetComponent<CarControlScript>();
        missileLifetime = 0f;

	}


	void FixedUpdate () {
		GetComponent<Rigidbody>().AddForce (transform.TransformDirection (Vector3.forward) *100);
        missileLifetime += Time.deltaTime;

        //Destroy the missile after it's been in the air for a certain period of time with no collisions.
        if(missileLifetime >= lifetimeThreshold)
        {
            explosionSound.Play();
            thisExplosion = Instantiate(explosion, new Vector3(gameObject.transform.position.x, (gameObject.transform.position.y ), gameObject.transform.position.z), Quaternion.identity);
            Destroy(gameObject);
            Destroy(thisExplosion, 2.0f);
            
        }
	
	}

	public void OnCollisionEnter(Collision collision) {
	  
		ContactPoint contact = collision.contacts[0];
		thisExplosion = Instantiate(explosion, contact.point + (contact.normal * 5.0f) , Quaternion.identity);
		
		if (collision.gameObject.tag == "BRIDGEROCK")
		{
            carControl.rockHealth--;
          //  carControl.isBigRockDestroyed = true;
           // smallRocks.SetActive(true);
            explosionSound.Play();

            if(carControl.rockHealth <= 0)
            {
               Destroy (collision.gameObject);
            }



        }
        explosionSound.Play();
		Destroy (thisExplosion, 2.0f);
		Destroy (gameObject);
	}

}






