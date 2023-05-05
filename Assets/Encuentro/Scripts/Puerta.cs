using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    Animator animator;
    MeshCollider meshCollider;
    BoxCollider boxCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        meshCollider = GetComponent<MeshCollider>();
        boxCollider = GetComponent<BoxCollider>();
    }
    public void CloseDoor()
    {
        animator.SetBool("doorOpened",false);
        meshCollider.enabled = false;
        boxCollider.enabled = true;
    }

    public void OpenDoor()
    {
        animator.SetBool("doorOpened", true);
        meshCollider.enabled = true;
        boxCollider.enabled = false;
    }

}
