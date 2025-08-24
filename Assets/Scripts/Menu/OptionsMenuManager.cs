using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class NewBehaviourScript : MonoBehaviour
{
    public Slider mainVolumeSlider;
    public Slider SFXVolumeSlider;
    public AudioMixer audioMixer;
    public Toggle splitScreenToggle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        // setting our volume settings for sliders
        float temp;
        audioMixer.GetFloat("VolumeMaster", out temp);
        mainVolumeSlider.value = temp;

        audioMixer.GetFloat("VolumeSFX", out temp);
        SFXVolumeSlider.value = temp;

    }

    public void OnBackToMenuButtonPressed()
    {
        //Goes back to main menu
        GameManager.Instance.ActivateMainMenu();
    }

    public void OnChangeMainVolume()
    {
        audioMixer.SetFloat("VolumeMaster", mainVolumeSlider.value);
    }

    public void OnChangeSFXVolume()
    {
        audioMixer.SetFloat("VolumeSFX", mainVolumeSlider.value);
    }

    public void OnChangeSplitScreenToggle()
    {
        // Sets the game to split screen with the gamemanager
        GameManager.Instance.isSplitScreen = splitScreenToggle.isOn;
    }
}
