using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverPanel3 : MonoBehaviour {
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
        SceneManager.LoadScene("Level3");
    }
    void Quit()
    {
        Application.Quit();
    }
    void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
