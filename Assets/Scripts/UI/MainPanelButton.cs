using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainPanelButton : MonoBehaviour {
    public Button btnReturn;
    void Start()
    {
        btnReturn.onClick.AddListener(ShowMainPanel);
    }
    public void ShowMainPanel()
    {
        SceneManager.LoadScene("MainScene");

    }
}
