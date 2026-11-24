using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI volumeText;
    public bool isMuted;
    public static MenuManager Instance;
    public void ToggleMute(bool mute)
    {
        AudioListener.volume = mute ? 0 : 1;
        
        isMuted = mute;
    }
    
    public void SetVolume(float desiredVolume)
    {
        if(isMuted) return;
        
        AudioListener.volume = desiredVolume;
        volumeText.text = $"Volume: {(desiredVolume * 100).ToString(format: "F0")}%";
    }
    void Start()
    {
        
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
