using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    class QuestAcceptedNotice : MonoBehaviour
    {
        [SerializeField] Player player;
        [SerializeField] Text questAcceptedNotice;
        [SerializeField] GameObject questAcceptedCanvas;
        private bool show = false;
        private bool start = false;

        void Start()
        {
            player.QuestProgressChanged += ShowQuestMessage;
            questAcceptedCanvas.SetActive(false);
        }

        public void ShowQuestMessage(string questName)
        {
            start = true;
            show = true;
            questAcceptedCanvas.SetActive(true);
            questAcceptedNotice.text = $"{questName}";
            StartCoroutine(FadeQuestMesesage());
        }

        private IEnumerator FadeQuestMesesage()
        {
            Color c = questAcceptedNotice.color;

            while (start)
            {
                if (show)
                {
                    c.a += 0.01f;
                    if (c.a >= 1)
                    {
                        show = false;
                        yield return new WaitForSeconds(2f);
                    }
                }
                else
                {
                    c.a -= 0.01f;
                    if (c.a <= 0)
                    {
                        start = false;
                        questAcceptedCanvas.SetActive(false);
                    }
                }
                questAcceptedNotice.color = c;
                yield return null;
            }
            yield return null;
        }
    }
}
