using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Static helpers for the slot files. Works in ANY scene (main menu included),
// because it does not need a SaveManager object.
public static class SaveSystem
{
    // Slot the player is currently playing in (-1 = none chosen yet).
    public static int CurrentSlot = -1;

    // Set right before the game scene loads; SaveManager reads it on startup.
    public static int PendingLoadSlot = -1;

    static string Dir => Path.Combine(Application.persistentDataPath, "Saves");
    public static string SlotPath(int slot) => Path.Combine(Dir, $"slot_{slot}.json");

    // Statics survive between Play sessions if "Enter Play Mode Options" has domain reload off.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        CurrentSlot = -1;
        PendingLoadSlot = -1;
    }

    public static bool HasSave(int slot) => File.Exists(SlotPath(slot));

    // Null if the slot is empty OR the file is unreadable. Use HasSave to tell them apart.
    public static SaveData Read(int slot)
    {
        if (!HasSave(slot)) return null;
        try
        {
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(SlotPath(slot)));
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not read save slot {slot}: {e.Message}");
            return null;
        }
    }

    public static void Write(int slot, SaveData data)
    {
        Directory.CreateDirectory(Dir);
        string path = SlotPath(slot);

        // Write to a temp file first so a crash mid-write can't corrupt the real save.
        string tmp = path + ".tmp";
        File.WriteAllText(tmp, JsonUtility.ToJson(data, true));
        if (File.Exists(path)) File.Delete(path);
        File.Move(tmp, path);
    }

    public static void Delete(int slot)
    {
        if (HasSave(slot)) File.Delete(SlotPath(slot));
        if (CurrentSlot == slot) CurrentSlot = -1;
    }

    // Loads the game scene and tells SaveManager to apply this slot once it has started.
    public static void LoadSlot(int slot, string gameSceneName)
    {
        if (!HasSave(slot))
        {
            Debug.Log($"Slot {slot} is empty.");
            return;
        }

        CurrentSlot = slot;
        PendingLoadSlot = slot;
        Time.timeScale = 1f;   // the Tab menu pauses the game; undo that before loading
        SceneManager.LoadScene(gameSceneName);
    }
}
