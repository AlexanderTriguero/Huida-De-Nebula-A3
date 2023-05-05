using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oleada : MonoBehaviour
{
    Enemigo[] enemigos;

    private void Awake()
    {
        enemigos = GetComponentsInChildren<Enemigo>();
    }
    internal void deactivateAllEnemies()
    {
        foreach (Enemigo e in enemigos)
        {
            e.gameObject.SetActive(false);
        }
    }

    internal void activeAllEnemies()
    {
        foreach (Enemigo e in enemigos)
        {
            e.gameObject.SetActive(true);
        }
    }

    internal bool areAllEnemiesDead()
    {
        bool allEnemiesAreDead = true;

        for(int i=0; allEnemiesAreDead && (i < enemigos.Length); i++)
        {
            allEnemiesAreDead = enemigos[i] == null;
        }
        return allEnemiesAreDead;
    }
}
