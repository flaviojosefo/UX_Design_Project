using UnityEngine;

public class Cheat : MonoBehaviour {

    public Transform basketBall;

    public Transform hoops;
    
    void Update() {

        if (Input.GetKeyDown(KeyCode.F1)) {

            ActivateHoops();
            basketBall.position = new Vector3(-12.3f, 10, 42.2f);
        }
    }

    private void ActivateHoops() {

        if (!hoops.gameObject.activeSelf) {

            hoops.gameObject.SetActive(true);
        }
    }
}
