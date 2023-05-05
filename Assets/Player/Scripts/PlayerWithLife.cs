using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;


public class PlayerWithLife : TargetWithLife
{
    [SerializeField] float lifeLostPerRunner = 0.5f;
    [SerializeField] float lifeLostPerGrunt = 1f;
    [SerializeField] float lifeLostPerTank = 2f;
    [SerializeField] TMP_Text cantidadDeVida;
    [SerializeField] Image barraDeVida;
    [SerializeField] protected AudioSource musica;
    Animator animator;
    PlayerShooting playerShooting;


    CharacterController playerController;

    public void Awake()
    {
        barraDeVida.fillAmount = life/maxLife;
        cantidadDeVida.text = ""+life;
        animator = GetComponentInChildren<Animator>();
        playerShooting=GetComponent<PlayerShooting>();
    }

    public void Update()
    {
        if (transform.position.y <= -50)
        {
            life = -1;
            playerAlive();
        }
    }

    public void NotifyRunner()
    {
        if (life > 0) {
            life -= lifeLostPerRunner;
            playerAlive();
        }
        
    }
    public void NotifyGrunt()
    {
        if(life > 0)
        {
            life -= lifeLostPerGrunt;
            playerAlive();
        }
    }
    public void NotifyTank()
    {
        if (life > 0)
        {
            life -= lifeLostPerTank;
            playerAlive();
        }
    }


    private  void playerAlive()
    {
        if (life < 0f)
        {
            life = 0;
        }
        barraDeVida.fillAmount = life / maxLife; ;
        cantidadDeVida.text = "" + life;
        if (life <= 0f)
        {
            if (!animator.GetBool("IsDead"))
            {
                cantidadDeVida.text = "0";
                animator.SetBool("IsDead", true);
                dyingSound?.Play();
                playerShooting.currentWeapon?.StopShooting();
                musica.Stop();
                onDeath.Invoke();
            }

        }
        else
        {
            damageSound?.Play();
        }
    }
}
