using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour {

    public bool wantsCake;

    public Sprite cakeSprite;
    public Sprite raffleSprite;

    private SpriteRenderer sprite;

    void Start() {

        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (wantsCake) {

            sprite.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            sprite.sprite = cakeSprite;

        } else {

            sprite.flipX = true;
            sprite.sprite = raffleSprite;
        }
    }

    void FixedUpdate() {

        sprite.transform.LookAt(Camera.main.transform);
    }
}
