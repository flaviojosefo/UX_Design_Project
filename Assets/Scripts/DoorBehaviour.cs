using UnityEngine;

public class DoorBehaviour : MonoBehaviour {
    
    void Start() {


    }

    void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("NPC")) {

            print("OPEN");
        }
    }

    void OnTriggerClose(Collider other) {

        if (other.CompareTag("NPC")) {

            print("CLOSE");
        }
    }
}
