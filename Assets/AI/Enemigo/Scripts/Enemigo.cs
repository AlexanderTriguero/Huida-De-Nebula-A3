using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavigateToTransform))]
public class Enemigo : MonoBehaviour, TargetWithLifeThatNotifies.IDeathNotifiable
{
    // Start is called before the first frame update

    NavigateToTransform navigateToTransform;
    WeaponBase meleeweapon;
    [SerializeField] float attackDistance=1.5f;
    [SerializeField] float attackPerSecond = 0.5f;
    [SerializeField] float destroyAfterDieTIme = 4f;
    Animator animator;
    CapsuleCollider enemyCollider;

    enum State
    {
        Seek,
        Attack,
        Die
    }

    [SerializeField] State state = State.Seek;
    void Awake()
    {
        navigateToTransform = GetComponent<NavigateToTransform>();
        meleeweapon = GetComponentInChildren<WeaponBase>();
        animator = GetComponentInChildren<Animator>();
        enemyCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        navigateToTransform.transformGoTo = PlayerMovement.instance.transform;
        animator.SetBool("IsWalking", true);
    }

    float timeForNextAttack=0f;
    void Update()
    {
        switch (state)
        {
            case State.Seek:
                if (isInAttackRange())
                {
                    animator.SetBool("IsWalking", false);
                    state = State.Attack;
                    navigateToTransform.transformGoTo = null;
                }
                break;
            case State.Attack:
                timeForNextAttack -= Time.deltaTime;
                this.transform.LookAt(new Vector3(PlayerMovement.instance.transform.position.x, this.transform.position.y, PlayerMovement.instance.transform.position.z));
                if (timeForNextAttack < 0f)
                {
                    if (!isInAttackRange())
                    {
                        animator.SetBool("IsWalking", true);
                        state = State.Seek;
                        navigateToTransform.transformGoTo = PlayerMovement.instance.transform;
                    }
                    else 
                    {
                        meleeweapon.Swing();
                        timeForNextAttack += 1f / attackPerSecond;
                    }
                }
                
                break;
            case State.Die:
                break;
        }
    }

    private bool isInAttackRange()
    {
        return (Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < attackDistance);
    }

     void TargetWithLifeThatNotifies.IDeathNotifiable.NotifyDeath()
    {
        if (state != State.Die)
        {
            state = State.Die;
            navigateToTransform.transformGoTo = null;
            //Lanzar animación de muerte, detener cualquier otra cosa que se esté haciendo
            animator.SetBool("Alive", false);
            enemyCollider.enabled = false;
            Destroy(gameObject, destroyAfterDieTIme);
        }
    }
}
