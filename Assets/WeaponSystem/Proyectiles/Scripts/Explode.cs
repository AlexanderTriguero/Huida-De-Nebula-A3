using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    public void Start()
    {
        Destroy(gameObject,15f);
    }
    [SerializeField] GameObject[] prefabToInstantiate;
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        foreach (GameObject go in prefabToInstantiate)
        {
            Instantiate(go,transform.position,transform.rotation);
        }
    }
}
