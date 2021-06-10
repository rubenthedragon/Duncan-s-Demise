using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text dialogText;
    [SerializeField] private GameObject canvas;

    [SerializeField] private float typingSpeed;

    [SerializeField] private DialogOptionsContainer optionsContainer;
    [SerializeField] private RectTransform dialogTextRect;

    [SerializeField] private Vector2 startDialogRectLeftRightOptions;
    [SerializeField] private Vector2 dialogRectLeftRightOptions;

    [SerializeField] private Dialog dialog = null;
    [SerializeField] private AudioClip acceptedQuest;
    private int currentDialog = 0;

    private bool doneTyping = true; 

    /// <summary>
    /// Initialize the DialogUI
    /// </summary>
    private void Start() 
    {
        optionsContainer.DisableOptions();
        canvas.SetActive(false);  
    }

    /// <summary>
    /// Allows the user to progress to the next dialog
    /// and to skip the typing of the current dialog
    /// </summary>
    private void Update()
    {
        if (dialog != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (doneTyping)
                {
                    NextDialog();
                }
                else
                {
                    if (currentDialog > dialog.dialog.Length)
                    {
                        doneTyping = true;
                        dialogText.text = dialog.dialog[currentDialog].Line;
                    }
                }

            }
        }
    }

    /// <summary>
    /// Start the dialog and freeze the time
    /// </summary>
    /// <param name="dialog"> The start dialog</param>
    public void StartDialog(Dialog startDialog)
    {
        if (dialog == null)
        {
            canvas.SetActive(true);

            dialogTextRect.offsetMin = new Vector2(startDialogRectLeftRightOptions.x, dialogTextRect.offsetMin.y);
            dialogTextRect.offsetMax = new Vector2(-startDialogRectLeftRightOptions.y, dialogTextRect.offsetMax.y);


            optionsContainer.DisableOptions();

            dialog = startDialog;
            currentDialog = -1;
            NextDialog();
        }
    }

    /// <summary>
    /// Stop the dialog and resume the time
    /// </summary>
    public void StopDialog()
    {
        canvas.SetActive(false);
        dialog = null;
    }

    /// <summary>
    /// Loads the next dialog line to display
    /// </summary>
    private void NextDialog()
    {
        if (!doneTyping) return;

        currentDialog++;

        if (currentDialog >= dialog.dialog.Length)
        {
            if (dialog.Response.Length == 0)
            {
                StopDialog();
            }
            else
            {
                ShowOptions();
            }
        }
        else
        {
            nameText.text = dialog.dialog[currentDialog].Name;
            dialogText.text = string.Empty;
            StartCoroutine(TypeText());
        }
    }

    /// <summary>
    /// Types the chars of the dialog line
    /// </summary>
    /// <returns></returns>
    private IEnumerator TypeText()
    {
        doneTyping = false;
        char[] chars = dialog.dialog[currentDialog].Line.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            if (doneTyping) break;

            dialogText.text += chars[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        doneTyping = true;
    }

    /// <summary>
    /// Ajusts the text to display correctly and shows the options
    /// </summary>
    private void ShowOptions()
    {
        dialogTextRect.offsetMin = new Vector2(dialogRectLeftRightOptions.x, dialogTextRect.offsetMin.y);
        dialogTextRect.offsetMax = new Vector2(-dialogRectLeftRightOptions.y, dialogTextRect.offsetMax.y);
        optionsContainer.ShowOptions(dialog.Response, acceptedQuest);
    }

    /// <summary>
    /// Responds to the option being selected
    /// </summary>
    /// <param name="dialog"> The response</param>
    public void EnterOption(Dialog dialog)
    {
        StopDialog();

        if(dialog != null)
        {
            StartDialog(dialog);
        }
    }
}