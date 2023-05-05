using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] GameObject prefabExplosionVisual;
    [SerializeField] float radio;
    [SerializeField] AudioClip explosionAudioCLip;
    void Start()
    {
        Destroy(gameObject);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radio);
        foreach (Collider c in colliders)
        {
            TargetBase target = c.GetComponent<TargetBase>();
            target?.NotifyExplosion();
        }

        AudioSource.PlayClipAtPoint(explosionAudioCLip,transform.position);
        Instantiate(prefabExplosionVisual, transform.position, Quaternion.identity);
    }


}
