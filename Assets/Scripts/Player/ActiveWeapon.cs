using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    // [SerializeField] private MonoBehaviour currentActiveWeapon;
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerControls playerControls;
    private float timeBetweenAttacks;
    private bool attackButtonDown, isAttacking = false;

    protected override void Awake() {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }
    // Start is called before the first frame update
    private void Start()
    {
        // playerControls.Combat.Attack.started += _ => Attack();
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        AttackCooldown();
    }

    private void Update() {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon) {
        CurrentActiveWeapon = newWeapon;

        if (CurrentActiveWeapon == null || !(CurrentActiveWeapon is IWeapon)) 
        {
            Debug.LogError("New weapon is not valid or does not implement IWeapon");
            return;
        }
        
        AttackCooldown();   //make sure khi chuyen doi qua lai giua cac vu khi se khong tu dong ban cung hoac ban phep tu truong => tranh player spam vo han
        //set thoi gian
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void WeaponNull() {
        CurrentActiveWeapon = null;
    }

    private void AttackCooldown() {
        isAttacking = true;
        StopAllCoroutines();    //dam bao khong bao gio co nhieu hon 1 lan giua cac cuoc tan cong at the same time
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine() {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    // public void ToggleIsAttacking(bool value) {
    //     isAttacking = value;
    // }

    private void StartAttacking() {
        attackButtonDown = true;
    }

    private void StopAttacking() {
        attackButtonDown = false;
    }

    private void Attack() {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            // isAttacking = true;
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}
