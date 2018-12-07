using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct ProfileData
{
    public int gold;
    public int crystal;
    public string playerName;
}

public class ProfileEntry : MonoBehaviour 
{
    public delegate void Callback(ProfileEntry profileEntry);
    private Callback _createCallback;
    private Callback _removeCallback;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI goldValueText;
    public TextMeshProUGUI crystalValueText;
    public TextMeshProUGUI playerProgressText;
    public Image playerProgressImage;
    public GameObject emptyProfile;
    public GameObject playerProfile;


    public void Set(ProfileInfo profile = null, Callback onCreate = null, Callback onRemove = null)
    {
        emptyProfile.SetActive(profile == null);
        playerProfile.SetActive(profile != null);

        if(profile != null)
        {
            playerNameText.text = profile.playerName;
            goldValueText.text = profile.playerGold.ToString();
            crystalValueText.text = profile.playerCrystal.ToString();
            playerProgressImage.fillAmount = profile.playerProgress;
            playerProgressText.text = Mathf.Round(profile.playerProgress * 100) + "%";
        }

        if (onCreate != null)
        {
            _createCallback = onCreate;
        }

        if (onRemove != null)
        {
            _removeCallback = onRemove;
        }
    }

    public void Load(ProfileData profileData)
    {
        playerNameText.text = profileData.playerName;
        goldValueText.text = profileData.gold.ToString();
        crystalValueText.text = profileData.crystal.ToString();
    }

    public void CreateNew()
    {
        _createCallback(this);
    }

    public void Remove ()
    {
        _removeCallback(this);
    }
}
