using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Aliases;

public class BoxHitDetector : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] Attack[] attacks;
    [SerializeField] LayerMask hurtLayer;
    [SerializeField] BoxCollider2D[] hitboxes;

    IAttacking attacker;
    bool isHit;
    List<Collider2D> hurtboxes;
    List<GameObject> enemies;

    void Start()
    {
        attacker = transform.parent.gameObject.GetComponent<IAttacking>();
        isHit = false;

        if (hitboxes.Length == 0) {
            hitboxes = this.GetComponents<BoxCollider2D>();
        }

        hurtboxes = new List<Collider2D>(50);
        enemies = new List<GameObject>(20);
    }

    private void Update()
    {
        if (attacker.IsAttacking && attacks.Contains(attacker.CurrentAttack) && !isHit) {
            isHit = true;


            foreach (BoxCollider2D hitbox in hitboxes) {
                //GameObject debugCube = GameObject.Find("Cube");
                //debugCube.transform.position = new Vector3(hitbox.bounds.center.x, hitbox.bounds.center.y, debugCube.transform.position.z);
                //debugCube.transform.localScale = new Vector3(hitbox.bounds.size.x, hitbox.bounds.size.y, 0);
                hurtboxes.AddRange(Physics2D.OverlapBoxAll(hitbox.bounds.center, hitbox.bounds.size, hitbox.transform.rotation.z, hurtLayer));
            }

            foreach (Collider2D hurtbox in hurtboxes) {
                if (!enemies.Contains(hurtbox.gameObject)) {
                    enemies.Add(hurtbox.gameObject);
                    hurtbox.SendMessageUpwards("OnTakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                }
            }

            hurtboxes.Clear();
            enemies.Clear();
        }

        if (!attacker.IsAttacking) {
            isHit = false;
        }
    }
}
