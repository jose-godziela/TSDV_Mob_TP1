using UnityEngine;
using TMPro;

public class FinalScoreSingle : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scorePlayer;

    TransferScores transferData;
    void Start()
    {
        transferData = FindObjectOfType<TransferScores>();
    }

    void Update()
    {
        scorePlayer.text = "Money Amount\n $ " + transferData?.GetPlayer1Money().ToString();
    }
}
