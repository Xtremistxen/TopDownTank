using UnityEngine;
public class StartOnKeypress : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            GameManager.Instance.ActivateMainMenu();
        }
    }
}