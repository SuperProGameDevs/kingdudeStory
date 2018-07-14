using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Seeker))]
public class CharacterAI : MonoBehaviour 
{
    Character character; // NPC controller
    Seeker seeker; // Path seeking object

    [SerializeField] Transform target; // Target to chase
    [SerializeField] float pathUpdateRate = 2; // How frequently we should update the path?
    [SerializeField] float speed = 4;
    [SerializeField] float waypointChangeThreshold = 3;

    public Path path;
    int currentWaypoint = 0;

    [HideInInspector] public bool pathIsEnded = false;


	// Use this for initialization
	private void Start () 
    {
        character = GetComponent<Character>();
        seeker = GetComponent<Seeker>();

        StartCoroutine(UpdatePath());
	}
	
	// Update is called once per frame
	private void Update () 
    {
        if (!this.HasTarget()) {
            // TODO: Search target and restart coroutine
        }
	}

	private void FixedUpdate()
	{
        // Only chase the target
        if (this.HasTarget() && this.HasPath()) {
            if (currentWaypoint >= path.vectorPath.Count) {
                Debug.Log("Reached the end of the path");
                path = null;
                return;
            }

            float currentWaypointDistance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (currentWaypointDistance < waypointChangeThreshold) {
                currentWaypoint++;
            }

            // At least this way we won't have any issues with accident asset X-Flip when reaching next waypoint
            if (currentWaypoint < path.vectorPath.Count) {
                int direction = Math.Sign(path.vectorPath[currentWaypoint].x - transform.position.x);
                character.Move(direction * speed);
            }
        }
	}

    private IEnumerator UpdatePath()
    {
        while (true) {
            if (!this.HasTarget()) {
                yield break;
            }

            seeker.StartPath(transform.position, target.position, OnPathComplete);
            yield return new WaitForSeconds(1 / pathUpdateRate);
        }
    }

    private void OnPathComplete(Path p)
    {
        Debug.Log("Path build complete!");
        if (p.error) {
            Debug.Log("Path building failed: " + p.errorLog);
            return;
        }

        path = p;
        currentWaypoint = 0;
    }

    // Check if there is a path to the target
    public bool HasPath()
    {
        return path != null;
    }

    public bool HasTarget()
    {
        return target != null;
    }
}
