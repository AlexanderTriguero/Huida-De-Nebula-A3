using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetBase : MonoBehaviour
{
    // Start is called before the first frame update
    public abstract void NotifyShot();
    public abstract void NotifySwing();
    public abstract void NotifyExplosion();
    public abstract void NotifyParticle();
    public abstract void NotifyShotgunShot();
}
