using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    // [SerializeField] private Transform weaponCollider;
    [SerializeField] private float SwordAttackCD = 0.5f;
    [SerializeField] private WeaponInfo weaponInfo;

    // private PlayerControls playerControls;
    private Transform weaponCollider;
    private Animator myAnimator;
    // private PlayerController playerController;
    // private ActiveWeapon activeWeapon;
    // private bool attackButtonDown, isAttacking = false;
    private GameObject slashAnim;
    private void Awake() {
        // playerController = GetComponentInParent<PlayerController>();
        // activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        // playerControls = new PlayerControls();
    }

    private void Start() {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashAnimSpawnPoint").transform;
    }

    // private void OnEnable() {
    //     playerControls.Enable();
    // }
    // // Start is called before the first frame update
    // void Start()
    // {
    //     // playerControls.Combat.Attack.started += _ => Attack();
    //     playerControls.Combat.Attack.started += _ => StartAttacking();
    //     playerControls.Combat.Attack.canceled += _ => StopAttacking();
    // }

    // Update is called once per frame
    private void Update()
    {
        MouseFollowWithOffset();
        // Attack();
    }

    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }

    // private void StartAttacking() {
    //     attackButtonDown = true;
    // }

    // private void StopAttacking() {
    //     attackButtonDown = false;
    // }
    public void Attack() {
        // if(attackButtonDown && !isAttacking){
            // isAttacking = true;
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);

            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
            // StartCoroutine(AttackCDRoutine());  //vi co nhieu vu khi nen se thay doi phan AttackCDRoutine
        // }
    }

    // private IEnumerator AttackCDRoutine() {
    //     yield return new WaitForSeconds(SwordAttackCD);
    //     // isAttacking = false;
    //     ActiveWeapon.Instance.ToggleIsAttacking(false);
    // }

    public void DoneAttackingAnimEvent() {
        weaponCollider.gameObject.SetActive(false);
    }

    private void SwingUpFlipAnimEvent() {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if(PlayerController.Instance.FacingLeft) {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void SwingDownFlipAnimEvent() {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if(PlayerController.Instance.FacingLeft) {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Face of Game Character will Follow The Mouse
    private void MouseFollowWithOffset() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if(mousePos.x < playerScreenPoint.x) {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        } 
        if(mousePos.x > playerScreenPoint.x) {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
