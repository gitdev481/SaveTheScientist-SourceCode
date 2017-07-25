using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	public void Clicked()
	{
		Application.LoadLevel ("Game");
	}
    public void MainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
	
}
