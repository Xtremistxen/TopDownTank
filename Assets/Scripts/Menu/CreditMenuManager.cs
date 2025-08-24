using UnityEngine;

public class CreditMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnBackToMenuButtonPressed()
    {
        //Goes back to main menu
        GameManager.Instance.ActivateMainMenu();
    }

}
