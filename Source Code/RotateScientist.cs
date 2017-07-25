using UnityEngine;
using System.Collections;

public class RotateScientist : MonoBehaviour {
    float speed = 100f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(Vector3.up, speed * Time.deltaTime);

    }
}
