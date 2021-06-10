using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PlayerUI
{
    class ChangeManaTag : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private Vector2 target;
        private Text changeManaTag;

        void Start()
        {
            changeManaTag = GetComponent<Text>();
            player.ManaChanged += ChangeManaNumber;
        }

        public void ChangeManaNumber(int changedMana)
        {
            StartCoroutine(MoveText(changedMana));
        }

        private IEnumerator MoveText(int changedNumber)
        {
            changeManaTag.text = changedNumber.ToString();
            transform.localPosition = startPosition;
            yield return ChangeAlpha(0);

            if (changedNumber > 0)
            {
                yield return ChangeColor(Color.blue);
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
            changeManaTag.color = c;
            yield return new WaitForSeconds(0.1f);
        }

        private IEnumerator ChangeAlpha(float a)
        {
            Color c = changeManaTag.color;
            c.a = a;
            changeManaTag.color = c;
            yield return null;
        }
    }
}
