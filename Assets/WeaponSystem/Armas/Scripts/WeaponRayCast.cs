using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Para cambiar canSHoot por canShootOnce, pero que no se quiten las asignaciones echas
using UnityEngine.Serialization;
public class WeaponRayCast : WeaponBase
{
    [Header("Wapon info")]
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected float scatterAngle =0f;
    [SerializeField] protected float shotPerSecond = 5f;
    [FormerlySerializedAs("canShoot")]
    [SerializeField] protected bool canShootOnce;
    [SerializeField] protected bool canShotContinuosly;

    [Header("Debug")]
    [SerializeField] protected bool debugShot;

    [Header("Sounds")]
    [SerializeField] protected AudioSource shotSound;
    [SerializeField] protected AudioSource reloadSound;
    [SerializeField] protected AudioSource outOfAmmoSound;

    protected ParticleSystem shotPrefab;

    bool isShootingContinuosly;
    private void OnValidate()
    {
        if (debugShot)
        {
            Shot();
            debugShot = false;
        }
    }

    float timeForNextShot = 0f;

    private void Awake()
    {
        shotPrefab = GetComponentInChildren<ParticleSystem>();
    }
    private void Update()
    {
        if (isShootingContinuosly)
        {
            timeForNextShot -=Time.deltaTime;
            if (timeForNextShot <= 0f)
            {
                InternalShot();
                timeForNextShot += 1f / shotPerSecond;
            }
        }
    }

    public override void Shot()
    {
        if (canShootOnce)
        {
            InternalShot();
        }
    }
    protected void InternalShot()
    {
        //si existe se ejecuta
        shotSound?.Play();
        shotPrefab?.Play();
        RaycastHit hit;
        //base.Shot();
        //Physics.Spherecast un circulo en lugar de un punto
        //Physics.Spherecast un circulo en lugar de un punto

        float HorizontalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
        Quaternion horizontalScatter = Quaternion.AngleAxis(HorizontalScatterAngle, shootPoint.up);
        float VerticaalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
        Quaternion verticallScatter = Quaternion.AngleAxis(VerticaalScatterAngle, shootPoint.up);

        Vector3 shotForward = verticallScatter * (horizontalScatter * shootPoint.forward);
        if (Physics.Raycast(shootPoint.position, shotForward, out hit,Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(shootPoint.position, hit.point, Color.cyan, 10f);
            Debug.DrawRay(hit.point, hit.normal, Color.red, 10f);


            TargetBase targetbase=hit.collider.GetComponent<TargetBase>();
            targetbase?.NotifyShot();
        }
    }

    public override void StartShooting()
    {
        isShootingContinuosly = canShotContinuosly;
        base.StartShooting();
    }

    public override void StopShooting()
    {
        isShootingContinuosly = false;
        base.StartShooting();
    }
}
