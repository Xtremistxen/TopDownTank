using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartButtonPressed()
    {
        GameManager.Instance.ActivateGameplay();
    }

    public void OnCreditsButtonPressed()
    {
        GameManager.Instance.ActivateCreditsScreen();
    }

    public void OnOptionsButtonPressed()
    {
        GameManager.Instance.ActivateOptionsScreen();
    }

    public void OnQuitButtonPressed()
    {
        //quit game
        Application.Quit();
    }
}
