using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    [SerializeField] float damage;

    IAttacking attacker;
    bool isHit;

    void Start()
    {
        attacker = transform.parent.gameObject.GetComponent<IAttacking>();
        isHit = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(attacker.IsAttacking());
        Debug.Log(isHit);
        if (attacker.IsAttacking() && !isHit) {
            Debug.Log("Hit");
            collision.SendMessageUpwards("OnTakeDamage", damage);
            isHit = true;
        } else {
            isHit = false;
        }
    }
}
