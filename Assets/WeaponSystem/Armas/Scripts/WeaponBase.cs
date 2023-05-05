using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]protected LayerMask targetLayers=Physics.DefaultRaycastLayers;
    [SerializeField] float reloadtime=0f;
    public enum WeaponUseType
    {
        Swing,
        Shot,
        ConituousShoot,
        ReloadTime,
        Undefined
    }

    public virtual WeaponUseType GetUseType()
    {
        return WeaponUseType.Shot;
    }

    public virtual void Swing()
    {
        Debug.Log("Swing called in weaponBase");
    }
    public virtual void Shot(){
        Debug.Log("Shot called in weaponBase");
    }

    public virtual void StartShooting()
    {

    }

    public virtual void StopShooting()
    {

    }
    public bool InfiniteShot()
    {
        return reloadtime <= 0;
    }

    public virtual float ReloadTime()
    {
        return reloadtime;
    }
}
