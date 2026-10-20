using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// One row of the slot list: a label and Save / Load / Delete buttons.
// Everything is wired in code, so don't add OnClick events in the Inspector.
public class SaveSlotRow : MonoBehaviour
{
    public TMP_Text label;
    public Button saveButton;
    public Button loadButton;
    public Button deleteButton;

    int slot;
    SaveSlotMenu menu;

    // Overwrite and Delete need two clicks. The first click "arms" the button.
    Button armed;
    string saveText, deleteText;

    void Awake()
    {
        saveText = TextOf(saveButton);
        deleteText = TextOf(deleteButton);
    }

    void OnDisable() { Disarm(); }

    public void Setup(int slotIndex, SaveSlotMenu owner)
    {
        slot = slotIndex;
        menu = owner;
        Disarm();

        bool exists = SaveSystem.HasSave(slot);
        SaveData d = SaveSystem.Read(slot);   // null when empty or unreadable

        if (!exists)
        {
            label.text = $"Slot {slot + 1}: Empty";
        }
        else if (d == null)
        {
            label.text = $"Slot {slot + 1}: Unreadable save";
        }
        else
        {
            TimeSpan t = TimeSpan.FromSeconds(d.playTimeSeconds);
            label.text = $"Slot {slot + 1}  |  Lv {d.level}  |  {d.savedAt}  |  {(int)t.TotalHours}h {t.Minutes:00}m";
        }

        // Saving needs the game scene's SaveManager, so the Save button is hidden in the main menu.
        saveButton.gameObject.SetActive(SaveManager.Instance != null);
        loadButton.interactable = d != null;
        deleteButton.interactable = exists;

        saveButton.onClick.RemoveAllListeners();
        loadButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
        saveButton.onClick.AddListener(OnSave);
        loadButton.onClick.AddListener(OnLoad);
        deleteButton.onClick.AddListener(OnDelete);
    }

    void OnSave()
    {
        if (SaveManager.Instance == null) return;

        // Saving over an existing save asks for a second click.
        if (SaveSystem.HasSave(slot) && armed != saveButton)
        {
            Arm(saveButton, "Overwrite?");
            return;
        }

        SaveManager.Instance.SaveToSlot(slot);
        menu.Refresh();
    }

    void OnLoad()
    {
        SaveSystem.LoadSlot(slot, menu.gameSceneName);
    }

    void OnDelete()
    {
        if (armed != deleteButton)
        {
            Arm(deleteButton, "Sure?");
            return;
        }

        SaveSystem.Delete(slot);
        menu.Refresh();
    }

    void Arm(Button button, string text)
    {
        Disarm();
        armed = button;
        SetText(button, text);
    }

    void Disarm()
    {
        armed = null;
        SetText(saveButton, saveText);
        SetText(deleteButton, deleteText);
    }

    static string TextOf(Button b)
    {
        if (b == null) return null;
        TMP_Text t = b.GetComponentInChildren<TMP_Text>(true);
        return t != null ? t.text : null;
    }

    static void SetText(Button b, string text)
    {
        if (b == null || text == null) return;
        TMP_Text t = b.GetComponentInChildren<TMP_Text>(true);
        if (t != null) t.text = text;
    }
}
