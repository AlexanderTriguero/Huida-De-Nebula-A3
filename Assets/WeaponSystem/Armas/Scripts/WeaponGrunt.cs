using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrunt : WeaponMelee
{
    public override void Swing()
    {
        Vector3 halfExtends = new Vector3(HorizontalRange / 2f, VerticalRange / 2f, forwardRange / 2);
        Collider[] colliders = Physics.OverlapBox(transform.position, halfExtends, transform.rotation, targetLayers);
        foreach (Collider c in colliders)
        {
            PlayerWithLife targetBase = c.GetComponent<PlayerWithLife>();
            targetBase?.NotifyGrunt();
        }

    }
}
