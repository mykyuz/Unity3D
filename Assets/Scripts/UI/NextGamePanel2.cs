using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NextGamePanel2 : MonoBehaviour {

    public Button btnNext;
    public Button btnQuit;
    public Button btnMain;
    void Start()
    {
        btnNext.onClick.AddListener(Next);
        btnQuit.onClick.AddListener(Quit);
        btnMain.onClick.AddListener(MainScene);
    }
    void Next()
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
