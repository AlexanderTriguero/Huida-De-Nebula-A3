using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    [Header("Información De Armas")]
    WeaponBase[] availableWeapons;
    float[] reloadtime;
    float[] timeForNextShot;
    public WeaponBase currentWeapon;
    [SerializeField] int currentWeaponIndex=0;
    int granadeLaucherIndex;
    [SerializeField] GameObject parentWeaponInterface;
    [SerializeField] GameObject[] weaponInterface;
    TMP_Text[] granadeLaucherReloadInterface;


    Animator animator;

    [Header("Bate")]
    [SerializeField] GameObject batCharges;
    int golpesMax = 3;
    [SerializeField] WeaponBase meleeWeapon;
    [SerializeField] TMP_Text batReload;
    [SerializeField] float batMaxReloadTime = 5f;
    [SerializeField] float meleeAttackTime=1f;
    [SerializeField] GameObject batVisuals;
    float batActualReloadTime = 0;
    int golpesDisponibles;
    Image[] golpesImages;
    bool isSwinging=false;
    
    // Start is called before the first frame update

    private void Awake()
    {
        //Si le pones true te pilla todos los componentes, aunque estén inactivos
        availableWeapons = GetComponentsInChildren<WeaponBase>(true);
        List<WeaponBase> nonMeleeWeaponList = availableWeapons.ToList();
        nonMeleeWeaponList.Remove(meleeWeapon);
        availableWeapons = nonMeleeWeaponList.ToArray();



        timeForNextShot=new float[availableWeapons.Length];
        reloadtime = new float[availableWeapons.Length];
        //añadimos una lista de tiempo de disparo para cada arma
        for (int i = 0; i < availableWeapons.Length; i++)
        {
            timeForNextShot[i] = 0f;
            reloadtime[i] = availableWeapons[i].ReloadTime();
            //getGranadeLaucherIndex
            if (availableWeapons[i].GetComponent<WeaponInstantiate>()!=null)
            {
                granadeLaucherIndex = i;
            }
        }

        //setGranadeLaucherReloadTimeInterface
        granadeLaucherReloadInterface = parentWeaponInterface.GetComponentsInChildren<TMP_Text>();


        //Animator
        animator = GetComponentInChildren<Animator>();

        //Cargamos datos para golpes disponibles del bate
        batReload.text = "";
        golpesImages = batCharges.GetComponentsInChildren<Image>();
        golpesMax= golpesImages.Length;
        golpesDisponibles = golpesMax;
        batVisuals.SetActive(false);

    }
    void Start()
    {
        SelecCurrentWeapon(currentWeaponIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
        bool isDead = animator.GetBool("IsDead");
        if (!isDead)
        {
            if (!UIPausa.pausado) {
                UpdateShotTime();
                UpdateBatValues();
                if (!isSwinging)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (currentWeapon.InfiniteShot())
                        {
                            currentWeapon.Shot();
                            currentWeapon.StartShooting();
                        }
                        else
                        {
                            if (timeForNextShot[currentWeaponIndex] <= 0)
                            {
                                currentWeapon.Shot();
                                currentWeapon.StartShooting();
                                timeForNextShot[currentWeaponIndex] = reloadtime[currentWeaponIndex];
                            }
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        if (golpesDisponibles > 0)
                        {
                            batActualReloadTime = batMaxReloadTime;
                            golpesDisponibles--;
                            UseBatCharge();
                            meleeWeapon.Swing();

                            batVisuals.SetActive(true);
                            isSwinging = true;
                            animator.SetBool("IsAttacking", true);
                            currentWeapon?.StopShooting();
                            currentWeapon.gameObject.SetActive(false);

                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        currentWeaponIndex--; 
                        SelecCurrentWeapon(currentWeaponIndex);

                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        currentWeaponIndex++;
                        SelecCurrentWeapon(currentWeaponIndex);

                    }
                }
                else
                {
                    if (batActualReloadTime <= batMaxReloadTime-meleeAttackTime)
                    {
                        isSwinging = false;
                        animator.SetBool("IsAttacking", false);
                        batVisuals.SetActive(false);
                        currentWeapon.gameObject.SetActive(true);
                        SelecCurrentWeapon(currentWeaponIndex);
                    }
                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    currentWeapon.StopShooting();
                }
            }
        }
    }
    private void SelecCurrentWeapon(int weaponIndex)
    {
        if (weaponIndex < 0)
        {
            currentWeaponIndex = availableWeapons.Length - 1;

        }
        else if(weaponIndex >= availableWeapons.Length)
        {
            currentWeaponIndex = 0;
        }
        currentWeapon?.StopShooting();
        currentWeapon = availableWeapons[currentWeaponIndex];


        TimeSpan GranadeLaucherTime = TimeSpan.FromSeconds(timeForNextShot[granadeLaucherIndex]);
        

        for (int i=0; i< availableWeapons.Length; i++)
        {
            //set active weapon
            availableWeapons[i].gameObject.SetActive(i== currentWeaponIndex);
            //set active weapon interface
            weaponInterface[i].gameObject.SetActive(i == currentWeaponIndex);
            if (timeForNextShot[granadeLaucherIndex] <= 0)
            {
                granadeLaucherReloadInterface[currentWeaponIndex].text = "";
            }
            else
            {
                granadeLaucherReloadInterface[currentWeaponIndex].text = GranadeLaucherTime.ToString("%s");
            }
            

        }
        /*foreach (WeaponBase w in availableWeapons)
        {
            w.gameObject.SetActive(w==currentWeapon);
       }
        */
    }

    private void UpdateShotTime()
    {
        float time = Time.deltaTime;
        for (int i = 0; i < availableWeapons.Length; i++)
        {
            if (timeForNextShot[i] > 0)
            {
                timeForNextShot[i] -= time;
                if (i == granadeLaucherIndex)
                {
                    TimeSpan GranadeLaucherTime = TimeSpan.FromSeconds(timeForNextShot[i]);
                    granadeLaucherReloadInterface[currentWeaponIndex].text = GranadeLaucherTime.ToString("%s");
                }
            }
            else
            {
                if (i == granadeLaucherIndex)
                {
                    granadeLaucherReloadInterface[currentWeaponIndex].text = "";
                }
            }
        }
    }

    private void UpdateBatValues()
    {
        if (batActualReloadTime > 0)
        {
            batActualReloadTime -= Time.deltaTime;

        }
        if (batActualReloadTime <= 0)
        {
            golpesDisponibles = golpesMax;
            EnableAllBatCharges();
            batReload.text = "";
        }
        else
        {
            TimeSpan batTIme = TimeSpan.FromSeconds(batActualReloadTime);
            batReload.text = batTIme.ToString("%s");
        }
    }

    private void EnableAllBatCharges()
    {
        foreach(Image i in golpesImages)
        {
            i.enabled = true;
        }
    }

    private void UseBatCharge()
    {
        foreach (Image i in golpesImages)
        {
            if (i.enabled)
            {
                i.enabled = false;
                break;
            }
        }
    }

    public bool IsSwinging()
    {
        return isSwinging;
    }
}
