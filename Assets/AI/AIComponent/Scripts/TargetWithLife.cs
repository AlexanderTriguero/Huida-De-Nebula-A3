using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetWithLife : TargetBase
{
    [SerializeField] protected float life = 1f;
    [SerializeField] protected float maxLife = 1f;
    [SerializeField] float lifeLostPerShot = 0.2f;
    [SerializeField] float lifeLostPerSwing = 0.2f;
    [SerializeField] float lifeLostPerExplosion = 0.2f;
    [SerializeField] float lifeLostPerParticle = 0.2f;
    [SerializeField] float lifeLostPerShotGun = 0.2f;
    [SerializeField] protected UnityEvent onLifeLost;
    [SerializeField] protected UnityEvent onDeath;
    [SerializeField] protected AudioSource dyingSound;
    [SerializeField] protected AudioSource damageSound;


    private void onEnable()
    {
        //ACTIVAR LISENER
    }

    private void onDisable()
    {
        //DESACTIVAR LISENER
    }

    public override void NotifyShot()
    {
        life -= lifeLostPerShot;
        chechStillAlive();
    }
    public override void NotifySwing()
    {
        life -= lifeLostPerSwing;
        chechStillAlive();
    }
    public override void NotifyExplosion()
    {
        life -= lifeLostPerExplosion;
        chechStillAlive();
    }
    public override void NotifyParticle()
    {
        life -= lifeLostPerParticle;
        chechStillAlive();
    }

    public override void NotifyShotgunShot()
    {
        life -= lifeLostPerShotGun;
        chechStillAlive();
    }

    public  void Die()
    {
        life =0 ;
        chechStillAlive();
    }

    protected virtual void chechStillAlive()
    {
        if (life <= 0f)
        {
            dyingSound?.Play();
            Destroy(gameObject);
            onDeath.Invoke();
        }
        else
        {
            damageSound?.Play();

        }
    }

}
