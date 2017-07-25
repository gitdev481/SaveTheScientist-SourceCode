using UnityEngine;
using System.Collections;

public class MissileLauncherOld : MonoBehaviour {


	public GameObject missile;

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0))
		{
		    Vector3 position = new Vector3(0f, 30.0f, 1f) * 10.0f;
			position = transform.TransformPoint (position);
			GameObject thisMissile = Instantiate (missile, position, transform.rotation) as GameObject;
			Physics.IgnoreCollision(thisMissile.GetComponent<Collider>(), GetComponent<Collider>());
		}

	
	}
}
