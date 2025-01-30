using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeLandSplatter : MonoBehaviour
{
    private SpriteFade spriteFade;  // reference to the SpriteFade script


    private void Awake()
    {
        spriteFade = GetComponent<SpriteFade>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(spriteFade.SlowFadeRoutine());
        Invoke(nameof(DisableCollider), 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(1, transform);
    }

    private void DisableCollider() {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
