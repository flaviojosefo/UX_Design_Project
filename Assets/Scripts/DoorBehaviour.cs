using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.layer == 8) {

            print("OPEN");
        }
    }

    void OnTriggerClose(Collider other) {

        if (other.gameObject.layer == 8) {

            print("CLOSE");
        }
    }
}
