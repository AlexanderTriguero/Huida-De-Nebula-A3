using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWithLifeThatNotifies  : TargetWithLife
{
    public interface IDeathNotifiable
    {
        public void NotifyDeath();
    }

    protected override void chechStillAlive()
    {
        if (life <= 0f)
        {
            GetComponent<IDeathNotifiable>()?.NotifyDeath();
            dyingSound?.Play();

        }
        else
        {
            damageSound?.Play();
        }
    }
}
