using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainPanel : MonoBehaviour {
    public Button btnEnterGame;
    public Button btnHelp;
    public Button btnQuit;
    public GameObject helpPanel;
    void Start () 
    {
        btnEnterGame.onClick.AddListener(EnterMainScenes);
        btnHelp.onClick.AddListener(ShowHelpPabel);
        btnQuit.onClick.AddListener(Quit);
    }
    void EnterMainScenes() 
    {
        SceneManager.LoadScene("Level1");

    }
    void ShowHelpPabel()
    {
        helpPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
    void Quit()
    {
        Application.Quit();
    }


}
