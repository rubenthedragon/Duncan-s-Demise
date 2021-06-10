using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemSplitter : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Slider splitSlider;
    [SerializeField] private TextMeshProUGUI handleText;

    private int cellToSplitIndex;

    public event Action<int, int, int> StackSplit;
    public event Action SplitCompleted;

    private void Start()
    {
        confirmButton.onClick.AddListener(SplitConfirmed);
        cancelButton.onClick.AddListener(CloseWindow);
        splitSlider.onValueChanged.AddListener(ChangeHandleText);
    }

    private void SplitConfirmed()
    {
        StackSplit?.Invoke(cellToSplitIndex, (int)(splitSlider.maxValue - splitSlider.value), (int)splitSlider.value);
        CloseWindow();
    }

    public void Init(int cellIndex, int itemAmount)
    {
        splitSlider.maxValue = itemAmount;
        cellToSplitIndex = cellIndex;
        splitSlider.value = itemAmount / 2;
        handleText.text = splitSlider.value.ToString();
    }

    private void CloseWindow()
    {
        SplitCompleted?.Invoke();
        Destroy(this.gameObject);
    }

    private void ChangeHandleText(float newValue)
    {
        handleText.text = newValue.ToString();
    }
}
