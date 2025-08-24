using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public void OnBackToMenuButtonPressed()
    {
        //Goes back to main menu
        GameManager.Instance.ActivateMainMenu();
    }
}
