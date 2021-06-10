using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTabSwitcher : MonoBehaviour
{
    [SerializeField] private Text tabTitle;
    [SerializeField] private Text tabButtonText;

    [field: SerializeField] public InventoryTab[] Tabs { get; private set; }

    private Queue<InventoryTab> tabQueue;
    private InventoryTab startTab;

    private void Awake()
    {
        tabQueue = new Queue<InventoryTab>(Tabs);
        startTab = tabQueue.Peek();
    }

    private void OnEnable() => SwitchToSpecificTab(startTab);

    public void SwitchToSpecificTab(InventoryTab tab)
    {
        if (Tabs.FirstOrDefault(t => t.Equals(tab)) == null) return;

        for (int i = 0; i < tabQueue.Count; i++)
        {
            InventoryTab invTab = tabQueue.Peek();

            if (invTab.Equals(tab))
            {
                SwitchTab();
                break;
            }
            else
            {
                InventoryTab tempTab = tabQueue.Dequeue();
                tabQueue.Enqueue(tempTab);
            }

        }
    }

    public void SwitchTab()
    {
        InventoryTab newTab = tabQueue.Dequeue();
        tabQueue.Enqueue(newTab);

        tabTitle.text = newTab.Name;
        UpdateButtonText(tabQueue.Peek().Name);

        foreach (InventoryTab tab in Tabs)
        {
            tab.tab.SetActive(tab.Equals(newTab));
        }
    }

    private void UpdateButtonText(string tabName) => tabButtonText.text = "Switch to " + tabName;
}