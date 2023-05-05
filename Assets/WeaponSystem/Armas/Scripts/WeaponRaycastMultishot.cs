using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRaycastMultishot : WeaponRayCast
{
    [SerializeField] int numShots = 10;
    [SerializeField] float timePerShot = 1f;

    float timeforNextShot = 0f;
    private void Awake()
    {
        shotPrefab = GetComponentInChildren<ParticleSystem>();
    }
    void Update()
    {
        if (timeforNextShot > 0)
        {
            timeforNextShot -= Time.deltaTime;
        }
    }
    public override void Shot()
    {
        if (timeforNextShot <= 0) { 
            for (int i = 0; i < numShots; i++)
            {
                //si existe se ejecuta
                timeforNextShot = timePerShot;
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
                if (Physics.Raycast(shootPoint.position, shotForward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    Debug.DrawLine(shootPoint.position, hit.point, Color.cyan, 10f);
                    Debug.DrawRay(hit.point, hit.normal, Color.red, 10f);


                    TargetBase targetbase = hit.collider.GetComponent<TargetBase>();
                    targetbase?.NotifyShotgunShot();
                }
            }
        }
    }
}
