using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CanvasManager : MonoBehaviour {

    public GameObject[] uiElements;

    public GameObject[] introElements;

    public FirstPersonController fps;

    //public TradeManager player;

    void Update() {

        CheckInput();
    }

    public void GoToNext() {

        introElements[0].SetActive(false);

        introElements[1].SetActive(true);
    }

    public void Begin() {

        uiElements[0].SetActive(false);
        
        for (int i = 1; i < uiElements.Length - 1; i++) {

            uiElements[i].SetActive(true);
        }

        fps.enabled = true;
    }

    private void CheckInput() {

        if (Input.GetKeyDown(KeyCode.Tab)) {

            if (uiElements[5].activeSelf && !uiElements[0].activeSelf) {

                OpenShop(false);

            } else if (!uiElements[5].activeSelf && !uiElements[0].activeSelf) {

                OpenShop(true);
            }
        }
    }

    private void OpenShop(bool open) {

        if (open) {

            for (int i = 2; i < uiElements.Length; i++) {

                if (uiElements[i].name == "Shop") {

                    uiElements[i].SetActive(true);
                    continue;
                }

                uiElements[i].SetActive(false);
            }
            fps.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;

        } else {

            for (int i = 2; i < uiElements.Length; i++) {

                if (uiElements[i].name == "Shop") {

                    uiElements[i].SetActive(false);
                    continue;
                }

                uiElements[i].SetActive(true);
            }
            fps.enabled = true;
            return;
        }
    }
}
