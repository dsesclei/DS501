using UnityEngine;
using System.Collections;

public class test_CanvasAlign : MonoBehaviour {
    
    GameObject canvas;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
        canvas.transform.forward = Camera.main.transform.forward;
	}
}
