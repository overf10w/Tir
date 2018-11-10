using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MessageHandler
{
    private CanvasGroup mainPanel;

	// Use this for initialization
	void Start ()
	{
	    mainPanel = transform.GetComponentInChildren<CanvasGroup>();
	}
	
    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.GameOver)
        {
            StartCoroutine(FadeInStartPanel(mainPanel));
        }
    }

    public IEnumerator FadeInStartPanel(CanvasGroup panel)
    {
        float delayBeforeMenuInteractable = 0.19f;
        while (panel.alpha < 1.0f)
        {
            if (panel.alpha >= delayBeforeMenuInteractable)
            {
                panel.interactable = true;
                panel.blocksRaycasts = true;
            }
            panel.alpha += Time.deltaTime / 2.3f;
            yield return null;
        }
    }

    public void LoadScene(string sceneName)
    {
        MessageBus.Nullify();
        //PlayerData.Nullify();
        SceneManager.LoadScene(sceneName);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
