using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeManager : MonoBehaviour {

    public float maxDetectDistance = 3f;
    public TMP_Text moneyAmount;
    public TMP_Text cakesAmount;
    public TMP_Text rafflesAmount;

    public float cakesCost = 2f;
    public float rafflesCost = 1f;

    public GameObject knob;
    public GameObject hand;

    private float money;

    [HideInInspector]
    public int raffles;

    [HideInInspector]
    public int cakes;

    private RaycastHit hit;

    private bool npcSeen;
    private bool cakeSeen;
    private bool raffleSeen;
    private bool ballSeen;

    private Transform currentNPC;
    private GameObject currentItem;

    private Transform _camera;
    private AudioSource sfx;

    void Start() {

        raffles = 0;
        cakes = 0;

        money = 10f;
        moneyAmount.text = money.ToString();
        cakesAmount.text = cakes.ToString();
        rafflesAmount.text = raffles.ToString();

        npcSeen = false;
        cakeSeen = false;
        raffleSeen = false;
        ballSeen = false;

        _camera = transform.GetChild(0);
        sfx = transform.GetChild(1).GetComponent<AudioSource>();
    }
    
    void Update() {
        
        ChangeCursors();
        CheckInput();
    }

    void FixedUpdate() {

        DetectNPC();
    }

    private void ChangeCursors() {

        if (npcSeen || cakeSeen || raffleSeen && currentItem != null && currentItem.activeSelf) {
            
            ShowHand();

        } else {

            HideHand();
        }
    }

    private void CheckInput() {

        if (Input.GetMouseButtonDown(0)) {

            if (npcSeen) Trade(currentNPC);

            if (cakeSeen && (money >= cakesCost) && currentItem != null && currentItem.activeSelf) AddCake();

            if (raffleSeen && (money >= rafflesCost) && currentItem != null && currentItem.activeSelf) AddRaffle();

            if (ballSeen) hit.transform.GetComponent<Rigidbody>().AddForceAtPosition((transform.forward + transform.up) * 400, hit.point);
        }
    }

    private void DetectNPC() {

        if (Physics.Raycast(_camera.position, _camera.TransformDirection(Vector3.forward), out hit, maxDetectDistance)) {
            
            if (hit.transform.CompareTag("NPC")) {

                currentNPC = hit.transform;

                if (!npcSeen) npcSeen = true;
                
                return;

            } else if (hit.transform.CompareTag("Cake")) {

                currentItem = hit.transform.gameObject;
                if (!cakeSeen) cakeSeen = true;

                return;

            } else if (hit.transform.CompareTag("Raffle")) {

                currentItem = hit.transform.gameObject;
                if (!raffleSeen) raffleSeen = true;

                return;

            } else if (hit.transform.CompareTag("Ball")) {

                if (!ballSeen) ballSeen = true;

            } else {

                if (npcSeen) npcSeen = false;
                if (cakeSeen) cakeSeen = false;
                if (raffleSeen) raffleSeen = false;
                if (ballSeen) ballSeen = false;
                if (currentItem != null) currentItem = null;
            }

        } else {

            if (npcSeen) npcSeen = false;
            if (cakeSeen) cakeSeen = false;
            if (raffleSeen) raffleSeen = false;
            if (ballSeen) ballSeen = false;
            if (currentItem != null) currentItem = null;
        }
    }

    private void Trade(Transform npc) {

        bool cake = npc.GetComponent<NPCBehaviour>().wantsCake;

        if (cake) {

            if (cakes > 0) {

                sfx.Play();
                cakes -= 1;
                UpdateUI(5);

            } else {

                // WARNING
            }

        } else {

            if (raffles > 0) {

                sfx.Play();
                raffles -= 1;
                UpdateUI(2);

            } else {

                // WARNING
            }
        }
    }

    private void AddCake() {

        currentItem.SetActive(false);
        HideHand();
        sfx.Play();
        cakes += 1;
        UpdateUI(-cakesCost);
    }

    private void AddRaffle() {
        
        currentItem.SetActive(false);
        HideHand();
        sfx.Play();
        raffles += 1;
        UpdateUI(-rafflesCost);
    }

    private bool HasItems(bool cake) {

        if (cake) {

            if (cakes > 0) return true;

        } else {

            if (raffles > 0) return true;
        }

        return false;
    }

    private void UpdateUI(float amount) {

        money += amount;

        moneyAmount.text = money.ToString();
        cakesAmount.text = cakes.ToString();
        rafflesAmount.text = raffles.ToString();
    }

    private void ShowHand() {
        
        if (!hand.activeSelf) hand.SetActive(true);
        if (knob.activeSelf) knob.SetActive(false);
    }

    private void HideHand() {
        
        if (hand.activeSelf) hand.SetActive(false);
        if (!knob.activeSelf) knob.SetActive(true);
    }

    private void OnDrawGizmos() {

        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));
    }
}
