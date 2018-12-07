using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupScreen : MonoBehaviour
{
    public delegate void Callback(ProfileInfo info);
    private Callback _callbackProfile;

    public CanvasGroup popupCanvasGroup;
    public TMP_InputField playerNameInputField;
    public TMP_InputField goldValueInputField;
    public TMP_InputField crystalValueInputField;
    public Slider playerProgressSlider;

    private int currentProfileSlot;

    static PopupScreen _instance;

    public static PopupScreen Instance
    {
        get
        {
            return _instance ?? (_instance = FindObjectOfType<PopupScreen>());
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    public void Open(Callback callback, int profileSlot)
    {
        popupCanvasGroup.alpha = 1;
        popupCanvasGroup.interactable = true;
        popupCanvasGroup.blocksRaycasts = true;

        currentProfileSlot = profileSlot;

        _callbackProfile = callback;
    }

    public void Close()
    {
        popupCanvasGroup.alpha = 0;
        popupCanvasGroup.interactable = false;
        popupCanvasGroup.blocksRaycasts = false;
    }

    public void Create()
    {
        var profile = new ProfileInfo
        {
            playerName = playerNameInputField.text,
            playerGold = int.Parse(goldValueInputField.text),
            playerCrystal = int.Parse(crystalValueInputField.text),
            playerProgress = playerProgressSlider.value,
            playerSlot = currentProfileSlot
        };

        SaveManager.Instance.save.profiles.Add(profile);

        _callbackProfile(profile);

        Close();
    }

    public void Cancel()
    {
        Close();
    }
}
