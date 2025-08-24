using UnityEngine;

public class GameOver : MonoBehaviour
{
     public void OnBackToMenuButtonPressed()
    {
        //Goes back to main menu
        GameManager.Instance.ActivateMainMenu();
    }
}
