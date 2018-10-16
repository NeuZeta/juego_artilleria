using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGenerator : MonoBehaviour {

    public Camera cam;
    public GameObject bombPrefab;

	void Start () {
		
	}
	
	
	void Update () {
		
       if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickedScreenPosition = Input.mousePosition;
            Debug.Log(clickedScreenPosition);

            Vector3 clickedViewportPosition = cam.ScreenToViewportPoint(clickedScreenPosition);
            Debug.Log(clickedViewportPosition);

            Vector3 clickedWorldPosition = cam.ScreenToWorldPoint(clickedScreenPosition);
            Debug.Log(clickedWorldPosition);



        }


	}
}
