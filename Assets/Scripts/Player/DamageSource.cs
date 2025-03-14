using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    // [SerializeField] private int damageAmount = 1;
    private int damageAmount;
    
    private void Start() 
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;    
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if(other.gameObject.GetComponent<EnemyAI>()) {
        //     // Debug.Log("Uwwaahhh!");
        //     EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        //     enemyHealth.TakeDamage(1);
        // }
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(damageAmount);
    }
}
