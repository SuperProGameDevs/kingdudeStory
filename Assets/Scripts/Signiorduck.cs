using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signiorduck : Character, IDamageable {

    Rigidbody2D duckRB;
    Animator animator;

    [SerializeField] float totalHealth = 100;
    float currentHealth;

    // Use this for initialization
    protected new void Start () {
        currentHealth = totalHealth;

        duckRB = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
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

        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}
