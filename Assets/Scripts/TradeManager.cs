using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeManager : MonoBehaviour {

    public float maxDetectDistance = 3f;
    public TMP_Text moneyAmount;
    public TMP_Text cakesAmount;
    public TMP_Text rafflesAmount;

    public float cakesCost = 15f;
    public float rafflesCost = 10f;

    public int cakesSell = 30;
    public int rafflesSell = 20;

    public GameObject knob;
    public GameObject hand;

    public GameObject warning;
    public GameObject warning2;

    public Transform props;
    public GameObject storeTop;
    public Transform buttons;
    public GameObject cost;
    public GameObject endMessage;
    public Transform price;

    private TMP_Text priceText;

    public CanvasManager cm;

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

    private bool buy;

    void Start() {

        priceText = price.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

        raffles = 0;
        cakes = 0;

        money = 50f;
        moneyAmount.text = money.ToString();
        cakesAmount.text = cakes.ToString();
        rafflesAmount.text = raffles.ToString();

        npcSeen = false;
        cakeSeen = false;
        raffleSeen = false;
        ballSeen = false;

        _camera = transform.GetChild(0);
        sfx = transform.GetChild(1).GetComponent<AudioSource>();

        buy = false;
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

            if (cakeSeen && (money <= cakesCost) && currentItem != null && currentItem.activeSelf) ShowWarning();

            if (raffleSeen && (money >= rafflesCost) && currentItem != null && currentItem.activeSelf) AddRaffle();

            if(raffleSeen && (money <= rafflesCost) && currentItem != null && currentItem.activeSelf) ShowWarning();

            if (ballSeen) hit.transform.GetComponent<Rigidbody>().AddForceAtPosition((transform.forward + transform.up) * 400, hit.point);
        }
    }

    private void DetectNPC() {

        if (Physics.Raycast(_camera.position, _camera.TransformDirection(Vector3.forward), out hit, maxDetectDistance)) {
            
            if (hit.transform.CompareTag("NPC")) {

                currentNPC = hit.transform;
                if (!price.gameObject.activeSelf) ShowPrice(true, GetNpcItem(hit.transform.GetComponent<NPCBehaviour>().wantsCake));
                if (!npcSeen) npcSeen = true;
                
                return;

            } else if (hit.transform.CompareTag("Cake")) {

                currentItem = hit.transform.gameObject;
                if (!price.gameObject.activeSelf) ShowPrice(false, (int)cakesCost);
                if (!cakeSeen) cakeSeen = true;

                return;

            } else if (hit.transform.CompareTag("Raffle")) {

                currentItem = hit.transform.gameObject;
                if (!price.gameObject.activeSelf) ShowPrice(false, (int)rafflesCost);
                if (!raffleSeen) raffleSeen = true;

                return;

            } else if (hit.transform.CompareTag("Ball")) {

                if (!ballSeen) ballSeen = true;

            } else {

                if (price.gameObject.activeSelf) HidePrice();
                if (npcSeen) npcSeen = false;
                if (cakeSeen) cakeSeen = false;
                if (raffleSeen) raffleSeen = false;
                if (ballSeen) ballSeen = false;
                if (currentItem != null) currentItem = null;
            }

        } else {

            if (price.gameObject.activeSelf) HidePrice();
            if (npcSeen) npcSeen = false;
            if (cakeSeen) cakeSeen = false;
            if (raffleSeen) raffleSeen = false;
            if (ballSeen) ballSeen = false;
            if (currentItem != null) currentItem = null;
        }
    }

    private int GetNpcItem(bool cake) {

        if (cake) {

            return cakesSell;
        }

        return rafflesSell;
    }

    private void ShowPrice(bool isNpc, int prc) {

        if (isNpc) {

            priceText.text = $"Vender: {prc}€";

        } else {

            priceText.text = $"Preço: {prc}€";
        }
        
        price.gameObject.SetActive(true);
    }

    private void HidePrice() {
        
        price.gameObject.SetActive(false);
    }

    private void Trade(Transform npc) {

        bool cake = npc.GetComponent<NPCBehaviour>().wantsCake;

        if (cake) {

            if (cakes > 0) {

                sfx.Play();
                cakes -= 1;
                UpdateUI(cakesSell);
                currentNPC.GetComponent<NPCBehaviour>().Traded = true;

            } else {

                ShowWarning2();
            }

        } else {

            if (raffles > 0) {

                sfx.Play();
                raffles -= 1;
                UpdateUI(rafflesSell);
                currentNPC.GetComponent<NPCBehaviour>().Traded = true;

            } else {

                ShowWarning2();
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

    public void UpdateUI(float amount) {

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

    private void ShowWarning() {

        warning.SetActive(true);

        Invoke("HideWarning", 2);
    }

    private void HideWarning() {

        warning.SetActive(false);
    }

    private void ShowWarning2() {

        warning2.SetActive(true);

        Invoke("HideWarning2", 2);
    }

    private void HideWarning2() {

        warning2.SetActive(false);
    }

    public void Buy(int cost) {

        if (cost < money) {
            
            buy = true;
            UpdateUI(-cost);

        } else {

            ShowWarning();
        }
    }

    public void ActivateItem(int itemNumber) {

        if (buy) {

            sfx.Play();
            props.GetChild(itemNumber).gameObject.SetActive(true);
            buttons.GetChild(itemNumber).GetComponent<Button>().interactable = false;
            CheckButtons();
            buy = false;
        }
    }

    private void CheckButtons() {

        for (int i = 0; i < buttons.childCount; i++) {

            if (buttons.GetChild(i).GetComponent<Button>().interactable) {

                return;
            }
        }

        DestroyStore();
        ActivateEndMessage();
    }

    private void DestroyStore() {
        
        storeTop.SetActive(false);
        buttons.gameObject.SetActive(false);
        cost.SetActive(false);
    }

    private void ActivateEndMessage() {

        endMessage.SetActive(true);
    }

    private void OnDrawGizmos() {

        if (_camera != null)
            Gizmos.DrawRay(_camera.position, _camera.TransformDirection(Vector3.forward));
    }
}
