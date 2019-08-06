using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverPanel : MonoBehaviour {
    public Button btnRe;
    public Button btnQuit;
    public Button btnMain;
    void Start ()
    {
        btnRe.onClick.AddListener(Re);
        btnQuit.onClick.AddListener(Quit);
        btnMain.onClick.AddListener(MainScene);
    }
    void Re()
    {
        SceneManager.LoadScene("Level1");
    }
    void Quit()
    {
        Application.Quit();
    }
    void ShowMainPanel()
    {
        SceneManager.LoadScene("MainScene");

    }
    void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
