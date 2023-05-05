using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encuentro : MonoBehaviour
{
    [SerializeField] Transform limites;
    [SerializeField] GameObject finDeNivel=null;

    Oleada[] oleadas;
    int currentOleada=-1;
    Puerta[] puertas;

    private void Awake()
    {
        oleadas = GetComponentsInChildren<Oleada>();
        puertas = limites.GetComponentsInChildren<Puerta>();
    }

     private void Start()
    {
        foreach (Oleada o in oleadas)
        {
            o.deactivateAllEnemies();
        }
        foreach (Puerta p in puertas)
        {
            p.OpenDoor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((currentOleada>=0) && (currentOleada < oleadas.Length)) { 
            if (oleadas[currentOleada].areAllEnemiesDead())
            {
                currentOleada++;
                if (currentOleada < oleadas.Length)
                {
                    oleadas[currentOleada].activeAllEnemies();
                }
                else
                {
                    foreach (Puerta p in puertas)
                    {
                        p.OpenDoor();
                    }
                    if (finDeNivel != null)
                    {
                        finDeNivel.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&currentOleada < 0)
        {
            currentOleada = 0;
            oleadas[currentOleada].activeAllEnemies();
            foreach (Puerta p in puertas)
            {
                p.CloseDoor();
            }
        }
    }
}
