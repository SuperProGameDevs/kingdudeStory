using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Signiorduck : MonoBehaviour, IDamageable 
{
    Animator animator;


    [SerializeField] float totalHealth = 100;
    float currentHealth;

    // Use this for initialization
    protected new void Start()
    {
        currentHealth = totalHealth;

        animator = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void FixedUpdate() 
    {
        
    }

    //protected override Transform GetGroundChecker()
    //{
    //    throw new System.NotImplementedException();
    //}

    //protected override LayerMask GetGroundMask()
    //{
    //    throw new System.NotImplementedException();
    //}

    //protected override Rigidbody2D GetRB()
    //{
    //    return duckRB;
    //}

    public void OnTakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}
