using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieForFalling : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponentInChildren<TargetWithLife>().Die();

    }
}
