using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedCounter : MonoBehaviour {

    public Text carSpeedText;
    public CarControlScript carControl;
    public GameObject car;
    public float carActualSpeed;
	// Use this for initialization
	void Start () {
        carSpeedText = gameObject.GetComponent<Text>();
        carControl = GameObject.FindGameObjectWithTag("CAR").GetComponent<CarControlScript>();
        car = GameObject.FindGameObjectWithTag("CAR");

        
	}
	
	// Update is called once per frame
	void Update () {
        carActualSpeed = car.GetComponent<Rigidbody>().velocity.magnitude * 10;
        carSpeedText.text = "MPH: " + (Mathf.Round(carActualSpeed).ToString());
    }
}
