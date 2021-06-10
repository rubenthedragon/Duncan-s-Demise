using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PlayerUI
{
    class ChangeHealthTag : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private Vector2 target;
        private Text changeHealthTag;

        void Start()
        {
            changeHealthTag = GetComponent<Text>();
            player.HealthChanged += ChangeHealthNumber;
        }

        public void ChangeHealthNumber(int changedHealth)
        {
            StartCoroutine(MoveText(changedHealth));
        }

        private IEnumerator MoveText(int changedNumber)
        {
            changeHealthTag.text = changedNumber.ToString();
            transform.localPosition = startPosition;
            yield return ChangeAlpha(0);

            if (changedNumber > 0)
            {
                yield return ChangeColor(Color.green);
                yield return ChangeAlpha(1);
            }
            else
            {
                yield return ChangeColor(Color.red);
                yield return ChangeAlpha(1);
            } 



            while (Vector2.Distance(transform.localPosition, target) > 0.1f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, target, 1 * Time.deltaTime);
                yield return null;
            }

            yield return ChangeAlpha(0);
        }

        private IEnumerator ChangeColor(Color c)
        {
            changeHealthTag.color = c;
            yield return new WaitForSeconds(0.1f);
        }

        private IEnumerator ChangeAlpha(float a)
        {
            Color c = changeHealthTag.color;
            c.a = a;
            changeHealthTag.color = c;
            yield return null;
        }
    }
}
