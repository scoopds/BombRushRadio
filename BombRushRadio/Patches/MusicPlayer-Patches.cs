﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Reptile;
using Reptile.Phone;
using UnityEngine;

namespace BombRushRadio;

[HarmonyPatch(typeof(AppMusicPlayer), nameof(AppMusicPlayer.OnAppInit))]
public class AppMusicPlayer_Patches
{
    public static AppMusicPlayer instance;
    static void Prefix(AppMusicPlayer __instance)
    {
        instance = __instance;
    }
}

[HarmonyPatch(typeof(AppMusicPlayer), nameof(AppMusicPlayer.OnAppDisable))]
public class AppMusicPlayerDisable_Patches
{
    static void Prefix(AppMusicPlayer __instance)
    {
        AppMusicPlayer_Patches.instance = null;
    }
}


[HarmonyPatch(typeof(MusicPlayer), nameof(MusicPlayer.StartMusicPlayer))]
public class MusicPlayer_Patches
{
    static bool Prefix(MusicPlayer __instance)
    {
        if (BombRushRadio.inMainMenu || BombRushRadio.loading)
            return false; // don't do it in the mainmenu

        BombRushRadio.mInstance = __instance;

        if (AppMusicPlayer_Patches.instance != null)
        {

            foreach (MusicTrack track in BombRushRadio.audios)
            {
                if (__instance.musicTrackQueue.currentMusicTracks.Find(m => m.Title == track.Title && m.Artist == track.Artist) != null)
                    continue;

                __instance.musicTrackQueue.currentMusicTracks.Add(track);
            }
        
            __instance.musicTrackQueue.ClearBuffer();
            __instance.musicTrackQueue.ClearMusicQueue();
            
            __instance.musicTrackQueue.BufferTracksInQueue();
            
            __instance.musicTrackQueue.AddAllCurrentTracksToQueue();
            
            AppMusicPlayer_Patches.instance.GameMusicPlayer = __instance;
        
            Debug.Log("Refreshed songs on app, total: " + __instance.musicTrackQueue.AmountOfTracks);
            AppMusicPlayer_Patches.instance.RefreshList();
            
            Debug.Log(AppMusicPlayer_Patches.instance.m_TrackList.musicAlfabeticList.Count + " - length");
            
            // basically, theres a MTrackList class that overrides some methods of the regular PhoneScroll class;
            // this calls the RefreshList method, which in turns updates everything with an alphabetical list of song indexes in the __instance.musicTrackQueue.currentMusicTracks list
            // and since they call the UpdateList method, and the MTrackList class overrides that, it'll basically call "SetContent(this.m_MusicApp.GetMusicTrack(this.musicAlfabeticList[contentIndex]));"
            
            
        }

        return true;
    }
}

[HarmonyPatch(typeof(MusicTrackQueue), nameof(MusicTrackQueue.HasMusicTrack))]
public class MusicTrackQueue_Patches
{
    static bool Prefix(MusicTrack musicTrack) // ignore unlocking for custom stuff
    {
        if (BombRushRadio.audios.Find(m => musicTrack.Artist == m.Artist && musicTrack.Title == m.Title))
            return false;

        return true;
    }
}

[HarmonyPatch(typeof(MusicTrackQueue), nameof(MusicTrackQueue.SelectNextTrack))]
public class MusicTrackQueue_Patches_SelectNextTrack
{
    static bool Prefix(MusicTrackQueue __instance)
    {
        Debug.Log("[BRR] Finding next track. Amount: "  + __instance.AmountOfTracks);
        return true;
    }
}

[HarmonyPatch(typeof(MusicTrackQueue), nameof(MusicTrackQueue.EvaluateNextTrack))]
public class MusicTrackQueue_Patches_EvaluateNextTrack
{
    static bool Prefix(MusicTrackQueue __instance, int nextTrackIndex)
    {
        Debug.Log("[BRR] Next Track!");

        if (BombRushRadio.CacheAudios.Value && !BombRushRadio.PreloadCache.Value)
        {
            MusicTrack t = __instance.currentMusicTracks[nextTrackIndex];

            if (BombRushRadio.audios.Find(m => m.Title == t.Title && m.Artist == t.Artist) && t.AudioClip == null)
            {
                string cacheName = Helpers.FormatMetadata(new string[] { t.Artist, t.Title }, "dash");

                string[] sp = BombRushRadio.filePaths[cacheName].Split(',');
                t.AudioClip = Helpers.LoadACFromCache(sp[0], sp[1]);
                Debug.Log("[BRR] Loaded cache for " + t.Title + ". Length: " + t.AudioClip.length);
            }
        }

        return true;
    }
}


[HarmonyPatch(typeof(MusicPlayer), nameof(MusicPlayer.UpdateIsPlayingMusic))]
public class MusicPlayer_Patches_UpdateIsPlayingMusic
{
    static void Prefix(MusicPlayer __instance)
    {
    }
}


[HarmonyPatch(typeof(MusicPlayer), nameof(MusicPlayer.PlayFrom))]
public class MusicPlayer_Patches_PlayFrom
{
    static bool Prefix(MusicPlayer __instance, int index, int playbackSamples = 0)
    {
        __instance.isPlayingFromPlaylist = false;
        __instance.wasPlayingLastFrame = false;
        __instance.playbackSamples = playbackSamples;
        __instance.musicTrackQueue.SetCurrentTrack(index);
        __instance.musicTrackQueue.ClearBuffer();
        __instance.musicTrackQueue.ClearMusicQueue();
        __instance.musicTrackQueue.AddAllCurrentTracksToQueue();

        if (BombRushRadio.CacheAudios.Value && !BombRushRadio.PreloadCache.Value)
        {
            MusicTrack t = __instance.musicTrackQueue.currentMusicTracks[index];

            if (BombRushRadio.audios.Find(m => m.Title == t.Title && m.Artist == t.Artist) && t.AudioClip == null)
            {
                string cacheName = Helpers.FormatMetadata(new string[] { t.Artist, t.Title }, "dash");

                string[] sp = BombRushRadio.filePaths[cacheName].Split(',');
                t.AudioClip = Helpers.LoadACFromCache(sp[0], sp[1]);
                Debug.Log("[BRR] Loaded cache for " + t.Title + ". Length: " + t.AudioClip.length);
            }
        }

        __instance.StartMusicPlayer();
        return false;
    }
}
