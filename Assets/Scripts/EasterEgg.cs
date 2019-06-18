using UnityEngine;

public class EasterEgg : MonoBehaviour {

    public TradeManager player;

    void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("Ball")) {

            player.UpdateUI(10000);
        }
    }
}
