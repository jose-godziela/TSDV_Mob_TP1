using UnityEngine;
using UnityEngine.UI;

public class TakeBags : MonoBehaviour
{
    Image uiInventory;
    [SerializeField] Sprite [] stockImages;
    [SerializeField] Sprite maxStock;
    [SerializeField] Player camionPlayer;

    float timerLastStock;
    float timerMaxStock;
    float timer;

    void Start()
    {
        timerLastStock = 0.5f;
        timerMaxStock = 1.0f;
        timer = 0;

        uiInventory = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if(camionPlayer.CantBolsAct < stockImages.Length-1)
        {
            uiInventory.sprite = stockImages[camionPlayer.CantBolsAct];
        }
        else if(camionPlayer.CantBolsAct == stockImages.Length - 1)
        {
            timer += Time.deltaTime;

            if(timer < timerLastStock && timer < timerMaxStock)
            {
                uiInventory.sprite = stockImages[camionPlayer.CantBolsAct];
            }
            if(timer > timerLastStock && timer < timerMaxStock)
            {
                uiInventory.sprite = maxStock;
            }

            if (timer > timerLastStock && timer > timerMaxStock)
                timer = 0;
        }
    }
}
