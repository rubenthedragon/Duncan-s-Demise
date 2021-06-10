using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text loadText;
    [SerializeField] bool DontUseDataControl;
    public bool ReadyToLoad { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DecreaseAlpha());
    }

    public IEnumerator DecreaseAlpha()
    {
        Color cI = image.color;
        Color cT = loadText.color;

        while (cI.a > 0)
        {
            cI.a -= 0.01f;
            cT.a -= 0.01f;
            image.color = cI;
            loadText.color = cT;
            yield return null;
        }
        yield return null;
    }

    public IEnumerator IncreaseAlpha(string SceneName)
    {
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            player.MovementSpeed = 0;
        }

        Color cI = image.color;
        Color cT = loadText.color;

        while (cI.a < 1)
        {
            cI.a += 0.01f;
            cT.a += 0.01f;
            image.color = cI;
            loadText.color = cT;
            yield return null;
        }

        if (DontUseDataControl)
        {
            if (DataControl.control != null)
            {
                Destroy(DataControl.control.gameObject);
            }
            SceneManager.LoadScene(SceneName);
        }
        else
        {
            DataControl.control.LoadNextScene(SceneName);
        }
        yield return null;
    }
}
