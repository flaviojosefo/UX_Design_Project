using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

    public TradeManager player;

    public List<GameObject> cakes;
    public List<GameObject> raffles;
    
    void Update() {

        if (CheckItems(cakes) && CheckItems(raffles) &&
           (player.cakes == 0) && (player.raffles == 0)) {

            RespawnItems(cakes);
            RespawnItems(raffles);
        } 
    }

    private bool CheckItems(List<GameObject> items) {

        foreach (GameObject g in items) {

            if (g.activeSelf) return false;
        }

        return true;
    }

    private void RespawnItems(List<GameObject> items) {

        foreach (GameObject g in items) {

            g.SetActive(true);
        }
    }
}
