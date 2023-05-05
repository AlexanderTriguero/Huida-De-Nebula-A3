using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : WeaponBase
{
    public float forwardRange= 1f;
    public float HorizontalRange= 1f;
    public float VerticalRange= 1f;
    [SerializeField] protected AudioSource swingSound;
    [SerializeField] protected float soundPlayTime=0.5f;

    public override void Swing()
    {
        swingSound.PlayDelayed(soundPlayTime);
        Vector3 halfExtends = new Vector3(HorizontalRange/2f,VerticalRange/2f,forwardRange/2);
        Collider[] colliders=Physics.OverlapBox(transform.position, halfExtends,transform.rotation, targetLayers);
        foreach(Collider c in colliders)
        {
           TargetBase targetBase= c.GetComponent<TargetBase>();
           targetBase?.NotifySwing();
        }

    }
}
