using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

    float deltaTime = 0.0f;
    public Text FPSText;
    void Start()    {
        FPSText = gameObject.GetComponent<Text>();
    }
	void Update () {
        //Gets the frames per second and rounds it to a whole number.
        deltaTime = Mathf.Round( 1 / Time.deltaTime);
     
        FPSText.text = "FPS: " + deltaTime.ToString();
    }
}
