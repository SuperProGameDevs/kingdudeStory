using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Seeker))]
public class CharacterAI : MonoBehaviour 
{
    Character controller; // NPC controller
    Seeker seeker; // Path seeking object

    [SerializeField] Transform target; // Target to chase
    [SerializeField] float pathUpdateRate = 2; // How frequently we shoult update the path?
    [SerializeField] float 


	// Use this for initialization
	private void Start () {
        controller = GetComponent<Character>();
        seeker = GetComponent<Seeker>();
	}
	
	// Update is called once per frame
	private void Update () {
		
	}

	private void FixedUpdate()
	{
		
	}
}
