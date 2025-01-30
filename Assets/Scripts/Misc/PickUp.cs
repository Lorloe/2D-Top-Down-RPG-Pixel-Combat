using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class PickUp : MonoBehaviour
{
    private enum PickUpType
    {
        GoldCoin,
        HealthGlobe,
        StaminaGlobe
    }

    [SerializeField] private PickUpType pickUpType;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private float accelerationRate = 2f;
    [SerializeField] private float moveSpeedStart = 3f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;

    private Vector3 moveDir;
    private Rigidbody2D rb;
    private float moveSpeed;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update() 
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (PlayerController.Instance == null) 
        {
            Debug.LogError("PlayerController is not available.");
            return;
        }

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            if (moveSpeed < moveSpeedStart)
            {
                moveSpeed = moveSpeedStart;
            }
 
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate;
        }
        else
        {
            moveDir = Vector3.zero;
            // moveSpeed = 0;
            moveSpeed = Mathf.Max(moveSpeed, 0);    // make the coin keep sliding and not abruptly stop when it goes out of the attraction range (pickUpDistance)
        }
    }

    private void FixedUpdate() 
    {
        rb.linearVelocity = moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<PlayerController>()) {
            DetectPickUpType();
            Destroy(gameObject);
        }
    }

    // private void OnTriggerStay2D(Collider2D other) {
    //     if (other.gameObject.GetComponent<PlayerController>()) {
    //         Destroy(gameObject);
    //     }
    // }

    private IEnumerator AnimCurveSpawnRoutine() {
        Vector2 startPoint = transform.position;
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        // Vector2 endPoint = new Vector2(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-1f, 1f));
        Vector2 endPoint = new Vector2(randomX, randomY);

        float timePassed = 0f;
        while(timePassed < popDuration) 
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
            yield return null;
        }
    }

    private void DetectPickUpType() {
        switch (pickUpType)
        {
            case PickUpType.GoldCoin:
                // do GoldCoin stuff
                // Debug.Log("Gold Coin Collected!");
                EconomyManager.Instance.UpdateCurrentGold();
                break;
            case PickUpType.HealthGlobe:
                // do HealthGlobe stuff and increase player's health
                // Debug.Log("Healing 1HP Player!");
                PlayerHealth.Instance.HealPlayer();
                break;
            case PickUpType.StaminaGlobe:
                // do StaminaGlobe stuff and increase player's stamina
                // Debug.Log("Stamina Collected!");
                Stamina.Instance.RefreshStamina();
                break;
            default:
                break;
        }
    }
}
