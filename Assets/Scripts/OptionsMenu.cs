using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Audio;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer mainAudioMixer;
    public TextMeshProUGUI sensitivityText;

    private void Start()
    {
        sensitivityText.SetText(PlayerMouseView.mouseSensitivity.ToString());
    }
    public void SetVolume (float volume)
    {
        mainAudioMixer.SetFloat("Volume", volume);
    }
    
    public void SetSensitivity (float sensitivity)
    {
        PlayerMouseView.mouseSensitivity = sensitivity;
        sensitivityText.SetText(PlayerMouseView.mouseSensitivity.ToString());
    }
}
