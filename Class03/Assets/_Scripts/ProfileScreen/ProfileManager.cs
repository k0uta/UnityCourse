using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour 
{
    public RectTransform GridTransform;
    public ProfileEntry ProfileEntryPrefab;
    public int ProfileEntryCount;


    public List<ProfileEntry> profileEntryList;

	void Start () 
    {
        profileEntryList = new List<ProfileEntry>();

        var profiles = SaveManager.Instance.save.profiles;

        var profilesById = new Dictionary<int, ProfileInfo>();

        foreach (var profile in profiles)
        {
            profilesById[profile.playerSlot] = profile;
        }

        for (var i = 0; i < ProfileEntryCount; i++)
        {
            var profileEntry = Instantiate(ProfileEntryPrefab, GridTransform);
            profileEntryList.Add(profileEntry);

            var profile = profilesById.ContainsKey(i) ? profilesById[i] : null;

            profileEntry.Set(profile, OnCreateProfile, OnRemoveProfile);
        }
    }

    void OnCreateProfile(ProfileEntry profileEntry)
    {
        int profileSlot = profileEntryList.IndexOf(profileEntry);
        PopupScreen.Instance.Open(OnProfileCreate, profileSlot);
    }

    void OnProfileCreate(ProfileInfo info)
    {
        profileEntryList[info.playerSlot].Set(info);
        SaveManager.Instance.SaveGame();
    }

    void OnRemoveProfile(ProfileEntry profileEntry)
    {
        int profileSlot = profileEntryList.IndexOf(profileEntry);

        var profiles = SaveManager.Instance.save.profiles;

        foreach (var profile in profiles)
        {
            if(profile.playerSlot == profileSlot)
            {
                SaveManager.Instance.save.profiles.Remove(profile);
                break;
            }
        }

        profileEntryList[profileSlot].Set(null);
        SaveManager.Instance.SaveGame();
    }
}
