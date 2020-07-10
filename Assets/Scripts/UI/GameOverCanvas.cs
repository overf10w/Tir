using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameOverCanvas : MessageHandler
    {
        private CanvasGroup mainPanel;

        private void Start()
        {
            InitMessageHandler();
            mainPanel = transform.GetComponentInChildren<CanvasGroup>();
        }

        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.GameOver };
            MessageBus.Instance.AddSubscriber(msc);
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
            //PlayerModel.Nullify();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void OnExitButtonClick()
        {
            Application.Quit();
        }
    }
}