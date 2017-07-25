using UnityEngine;
using System.Collections;

public class ChangeMiniMapSize : MonoBehaviour {


    public Camera minimapCamera;
    private bool isSmall = true;
    private Rect bigRect = new Rect(0, 0, 0.3f, 0.5f);
    private Rect smallRect = new Rect(0, 0, 0.15f, 0.25f);
   // private RectTransform smallButton = new RectTransform();
    // Use this for initialization
    void Start () {
      ////  smallButton.anchoredPosition = new Vector2(-806, -449);
        ///smallButton.sizeDelta = new Vector2(300, 300);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeSize()
    {
        if(isSmall )
        {
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-660, -300, 0);
           gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 600);
            minimapCamera.rect = bigRect;
            isSmall = false;
           

        }else if (!isSmall)
        {
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-806, -449, 0);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
          
            minimapCamera.rect = smallRect;
            isSmall = true;
        }



    }
}
