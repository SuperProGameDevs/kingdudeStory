using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Signiorduck : Character, IDamageable {

    Rigidbody2D duckRB;
    Animator animator;

    // AI variables
    Seeker seeker;
    Path path;


    [SerializeField] float totalHealth = 100;
    float currentHealth;

    // Use this for initialization
    protected new void Start()
    {
        currentHealth = totalHealth;

        duckRB = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        seeker = this.GetComponent<Seeker>();
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPathFound(Path path)
    {
        if (path.error) {
            Debug.LogError("Failed to find path: " + path.errorLog);
            return;
        }

        this.path = path;
    }

    void FixedUpdate() 
    {
        
    }

    protected override Transform GetGroundChecker()
    {
        throw new System.NotImplementedException();
    }

    protected override LayerMask GetGroundMask()
    {
        throw new System.NotImplementedException();
    }

    protected override Rigidbody2D GetRB()
    {
        return duckRB;
    }

    public void OnTakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}
