using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    [SerializeField] private GameObject observable;

    private Vector3 offset;

	// Use this for initialization
	void Start () 
    {
        offset = this.transform.position - observable.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    // Runs after every Update is fully processed ( AfterUpdate would be a better name :) )
    private void LateUpdate()
    {
        this.transform.position = observable.transform.position + offset;;
    }
}
