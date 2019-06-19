using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoors : MonoBehaviour
{
    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 8) {
            anim.SetBool("Open", true);
            StartCoroutine(CloseDoor());
        }
    }

    private IEnumerator CloseDoor() {
        yield return new WaitForSeconds(3);
        anim.SetBool("Open", false);
    }
}
