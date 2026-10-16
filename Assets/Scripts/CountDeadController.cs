using TMPro;
using UnityEngine;

public class CountDeadController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemyCountText;
    [SerializeField] PlayerController playerController;

    private void OnEnable()
    {

        playerController.OnEnemyKilled += UpdateUI;
    }

    void UpdateUI(int count)
    {
        enemyCountText.text = "Inimigos abatidos: " + count.ToString();
    }
}
