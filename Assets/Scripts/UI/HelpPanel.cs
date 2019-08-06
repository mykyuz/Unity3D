using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour {

    public GameObject mainPanel;
    public Button btnReturn;
    void Start()
    {
        btnReturn.onClick.AddListener(ShowMainPanel);
    }
    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        this.gameObject.SetActive(false);

    }
   
	
	
}
