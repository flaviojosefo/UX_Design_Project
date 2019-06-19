using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    private Collider myCollider;

    private void Start() {
        myCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("NPC")) {
            Physics.IgnoreCollision(myCollider, collision.gameObject.GetComponent<Collider>());
        }
    }
}
