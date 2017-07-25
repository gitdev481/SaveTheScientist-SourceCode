using UnityEngine;
using System.Collections;

public class RotateArrow : MonoBehaviour {

    // Use this for initialization
    float speed = 100f;
    private float tempValue;
    private Vector3 tempPosition;
    private float amplitude  = 0.25f;
	void Start () {

        tempValue = transform.position.y;
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(Vector3.up, speed * Time.deltaTime) ;
        tempPosition.x = transform.position.x;
        tempPosition.z = transform.position.z;
        tempPosition.y = tempValue + amplitude * Mathf.Sin(5 * Time.time);
        transform.position = tempPosition;
	}
}
