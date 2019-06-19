using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour {

    public bool wantsCake;

    public Sprite cakeSprite;
    public Sprite raffleSprite;

    public bool Traded { get; set; }

    private SpriteRenderer sprite;
    private Animator anim;
    private GameObject e1;
    private GameObject e2;
    private GameObject playerFeet;
    private Vector3 startPos;
    private bool exit1;
    private bool startMovement;
    private bool moveToSpot;
    private float stopTime;
    private float time;
    private float timeLimit;
    private float currentLerpTime;
    private float lerpTime;
    private float perc;

    void Start() {
        exit1 = false;
        startMovement = false;
        moveToSpot = false;
        Traded = false;
        stopTime = 0;
        time = 0;
        timeLimit = Random.Range(5, 16);
        currentLerpTime = 0;
        lerpTime = 3;

        anim = GetComponent<Animator>();
        sprite = transform.GetChild(2).GetComponent<SpriteRenderer>();
        startPos = transform.position;

        e1 = GameObject.Find("E1");
        e2 = GameObject.Find("E2");
        playerFeet = GameObject.Find("Feet");

        // Randomize what the npc wants
        wantsCake = Random.Range(0, 2) == 0 ? true : false;

        if (wantsCake) {

            sprite.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            sprite.sprite = cakeSprite;

        } else {

            sprite.flipX = true;
            sprite.sprite = raffleSprite;
        }

        anim.SetBool("Walking", true);
    }

    private void Update() {
        if (!Traded) {
            if (!exit1) {
                UpdateLerpTime();
                transform.position = Vector3.Lerp(startPos, e1.transform.position, perc);
                transform.LookAt(e1.transform.position);
                float dist = Vector3.Distance(transform.position, e1.transform.position);
                if (dist < 0.5f) {
                    startPos = transform.position;
                    currentLerpTime = 0;
                    exit1 = true;
                }
            } else if (!startMovement) {
                UpdateLerpTime();
                transform.position = Vector3.Lerp(startPos, e2.transform.position, perc);
                transform.LookAt(e2.transform.position);
                float dist = Vector3.Distance(transform.position, e2.transform.position);
                if (dist < 0.5f) {
                    startPos = transform.position;
                    currentLerpTime = 0;
                    startMovement = true;
                    stopTime = timeLimit;
                }
            }

            stopTime += Time.deltaTime;

            if (startMovement && stopTime >= timeLimit) {
                anim.SetBool("Walking", true);
                Vector3 direction = transform.forward;
                if (moveToSpot) {
                    time += Time.deltaTime * 2;
                    transform.position += direction * Time.deltaTime * 2;

                    if (time > 5) {
                        timeLimit = Random.Range(5, 16);
                        startPos = transform.position;
                        moveToSpot = false;
                        stopTime = 0;
                        time = 0;
                    }
                } else {
                    direction = new Vector3(0, Random.Range(0, 360), 0);
                    transform.rotation = Quaternion.Euler(direction);
                    if (!(Physics.Raycast(transform.position, transform.forward, 6))) {
                        direction = transform.forward;
                        moveToSpot = true;
                    }
                }
            }
        }else if (Traded) {
            gameObject.tag = "Untagged";
            sprite.enabled = false;
            if (exit1) {
                anim.SetBool("Walking", true);
                startMovement = false;
                UpdateLerpTime();
                transform.position = Vector3.Lerp(startPos, e2.transform.position, perc);
                transform.LookAt(e2.transform.position);
                float dist = Vector3.Distance(transform.position, e2.transform.position);
                if (dist < 0.5f) {
                    exit1 = false;
                    startPos = transform.position;
                    currentLerpTime = 0;
                }
            } else if (!startMovement) {
                UpdateLerpTime();
                transform.position = Vector3.Lerp(startPos, e1.transform.position, perc);
                transform.LookAt(e1.transform.position);
                float dist = Vector3.Distance(transform.position, e1.transform.position);
                if (dist < 0.5f) {
                    GameObject.Find("GameManager").GetComponent<GameManager>().currentNPCs--;
                    Destroy(gameObject);
                }
            }
        }
    }

    private void UpdateLerpTime() {

        currentLerpTime += Time.deltaTime;

        if (currentLerpTime >= lerpTime) {

            currentLerpTime = lerpTime;
        }

        perc = (currentLerpTime / lerpTime);
    }

    void FixedUpdate() {
        if(startMovement && stopTime < timeLimit) {
            anim.SetBool("Walking", false);
            float dist = Vector3.Distance(transform.position, playerFeet.transform.position);
            if (dist < 8) {
                Vector3 lookPos = playerFeet.transform.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
            }
        }

        sprite.transform.LookAt(Camera.main.transform);
    }
}
