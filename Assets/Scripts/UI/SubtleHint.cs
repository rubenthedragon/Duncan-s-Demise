using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.UI
{
    class SubtleHint : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Image subtleHintImage;
        private bool brighter = true;

        void Start()
        {
            player.StatPointsChanged += ActivateSubtleHint;
            StartCoroutine(SubtleHintThingy());
        }

        public void ActivateSubtleHint(int statPoints)
        {
            StartCoroutine(SubtleHintThingy());
        }

        private IEnumerator SubtleHintThingy()
        {
            if (player.StatPoints == 0)
            {
                Color c = subtleHintImage.color;
                c.a = 0f;
                subtleHintImage.color = c;
                yield return null;
            }
            else
            {
                yield return ChangeALpha();
            }
        }

        private IEnumerator ChangeALpha()
        {
            Color c = subtleHintImage.color;

            while (player.StatPoints != 0)
            {
                if (brighter)
                {
                    c.a += 0.005f;
                    if (c.a >= 1f)
                    {
                        brighter = false;
                    }
                }
                else
                {
                    c.a -= 0.005f;
                    if (c.a <= 0f)
                    {
                        brighter = true;
                    }
                }
                subtleHintImage.color = c;
                yield return null;
            }
            yield return null;
        }
    }
}
