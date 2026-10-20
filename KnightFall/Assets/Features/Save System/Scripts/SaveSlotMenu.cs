using System.Collections.Generic;
using UnityEngine;

// Builds the list of save slots. Use it in two places:
//  - the Main menu's LoadPanel (no SaveManager there, so rows only show Load / Delete)
//  - a panel inside the in-game Tab menu (rows show Save / Load / Delete)
public class SaveSlotMenu : MonoBehaviour
{
    [Min(1)] public int slotCount = 3;
    public string gameSceneName = "SampleScene";
    public SaveSlotRow rowPrefab;
    [Tooltip("Where the rows are created. Give it a Vertical Layout Group. Leave empty to use this object.")]
    public Transform container;

    readonly List<SaveSlotRow> rows = new List<SaveSlotRow>();

    void OnEnable() { Refresh(); }

    // Also refresh on Start: if this panel starts active, SaveManager may not have run Awake yet in OnEnable.
    void Start() { Refresh(); }

    public void Refresh()
    {
        Transform parent = container != null ? container : transform;

        while (rows.Count < slotCount)
            rows.Add(Instantiate(rowPrefab, parent));

        for (int i = 0; i < rows.Count; i++)
        {
            bool used = i < slotCount;
            rows[i].gameObject.SetActive(used);
            if (used) rows[i].Setup(i, this);
        }
    }
}
