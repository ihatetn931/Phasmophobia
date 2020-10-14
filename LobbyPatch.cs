﻿using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using System.Reflection;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using UnityEngine.XR;

namespace GhostMod
{
    static class Main
    {
        public static UnityModManager.ModEntry mod;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            Debug.Log($"Id:{modEntry.Info.Id}");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            mod = modEntry;
            return true;
        }
    }

    [HarmonyPatch(typeof(HeartRateData))]
    [HarmonyPatch("Update")]
    internal class HeartRateData_Update_Patch
    {
        public static int averageSanity;
        public static int p1Sanity;
        public static int p2Sanity;
        public static int p3Sanity;
        public static int p4Sanity;
        public static int playerCount;

        [HarmonyPrefix]
        public static bool Prefix(HeartRateData __instance)
        {
            var ghost = __instance.transform.GetComponentsInChildren<Text>();
            var image = __instance.transform.GetComponentsInChildren<Image>();

            if (ghost != null)
            {
                foreach (var test in ghost)
                {
                    foreach (var images in image)
                    {
                        if (test.rectTransform.name.Contains("P1HRValueText") && (images.rectTransform.name.Contains("P1HeartImage")))
                        {
                            if (p1Sanity > 75)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(0, 1, 0, 0.1f);
                            }
                            else if (p1Sanity < 75 && p1Sanity > 25)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(1, 0.92f, 0.016f, 0.1f);
                            }
                            else if (p1Sanity < 25)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(1, 0, 0, 0.1f);
                            }
                            GetPlayer1Insanity(test.text.Replace("%", ""));
                        }
                        if (test.rectTransform.name.Contains("P2HRValueText") && (images.rectTransform.name.Contains("P2HeartImage")))
                        {
                            if (p2Sanity > 75)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(0, 1, 0, 0.10f);
                            }
                            else if (p2Sanity < 75 && p2Sanity > 25)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(1, 0.92f, 0.016f, 0.10f);
                            }
                            else if (p2Sanity < 25)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(1, 0, 0, 0.10f);
                            }
                            GetPlayer2Insanity(test.text.Replace("%", ""));
                        }
                        if (test.rectTransform.name.Contains("P3HRValueText") && (images.rectTransform.name.Contains("P3HeartImage")))
                        {
                            if (p3Sanity > 75)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(0, 1, 0, 0.10f);
                            }
                            else if (p3Sanity < 75 && p3Sanity > 25)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(1, 0.92f, 0.016f, 0.10f);
                            }
                            else if (p3Sanity < 25)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(1, 0, 0, 0.10f);
                            }
                            GetPlayer3Insanity(test.text.Replace("%", ""));
                        }
                        if (test.rectTransform.name.Contains("P4HRValueText") && (images.rectTransform.name.Contains("P4HeartImage")))
                        {
                            if (p4Sanity > 75)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(0, 1, 0, 0.10f);
                            }
                            else if (p4Sanity < 75 && p4Sanity > 25)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(1, 0.92f, 0.016f, 0.10f);
                            }
                            else if (p4Sanity < 25)
                            {
                                test.color = new Color(1, 1, 1, 1);
                                images.color = new Color(1, 0, 0, 0.10f);
                            }
                            GetPlayer4Insanity(test.text.Replace("%", ""));
                        }
                        if (test.rectTransform.name.Contains("TitleText"))
                        {

                            averageSanity = p1Sanity + p2Sanity + p3Sanity + p4Sanity;
                            test.color = new Color(1, 1, 1, 1);
                            test.fontSize = 10;
                            if (playerCount != 0)
                            {
                                test.text = $"Team Sanity\n Avg. {averageSanity / playerCount }%\n";
                            }
                        }
                    }
                }
            }
            return true;
        }
        public static void GetPlayer1Insanity(string player)
        {
            if (player != null)
            {
                int d;
                bool success = int.TryParse(player, out d);
                if (success)
                {
                    p1Sanity = d;
                    playerCount = 1;
                }
            }
        }
        public static void GetPlayer2Insanity(string player)
        {
            if (player != null)
            {
                int d;
                bool success = int.TryParse(player, out d);
                if (success)
                {
                    p2Sanity = d;
                    playerCount = 2;
                }
            }
        }
        public static void GetPlayer3Insanity(string player)
        {
            if (player != null)
            {
                int d;
                bool success = int.TryParse(player, out d);
                if (success)
                {
                    p3Sanity = d;
                    playerCount = 3;
                }

            }
        }
        public static void GetPlayer4Insanity(string player)
        {
            if (player != null)
            {
                int d;
                bool success = int.TryParse(player, out d);
                if (success)
                {
                    p4Sanity = d;
                    playerCount = 4;
                }
            }
        }
    }


    [HarmonyPatch(typeof(LobbyManager))]
    [HarmonyPatch("CreateServer")]
    internal class LobbyManager_CreateServer_Patch
    {
        public static bool Prefix(bool isPrivate)
        {
            if (XRDevice.isPresent)
            {
               // this.SaveVRPlayerPositions();
            }
            else
            {
                //this.SavePCPlayerPositions();
            }
            PlayerPrefs.SetInt("isPublicServer", isPrivate ? 0 : 1);
            RoomOptions roomOptions = new RoomOptions
            {
                IsOpen = true,
                IsVisible = !isPrivate,
                MaxPlayers = 8,
                PlayerTtl = 5000
            };
            Debug.Log($"isPrivate: {isPrivate}");
            if (!isPrivate)
            {
                PhotonNetwork.CreateRoom(PhotonNetwork.player.NickName + "#" + UnityEngine.Random.Range(0, 999999).ToString("000000"), roomOptions, TypedLobby.Default);
                return false;
            }
            PhotonNetwork.CreateRoom(UnityEngine.Random.Range(0, 999999).ToString("000000"), roomOptions, TypedLobby.Default);
            return false;
        }
    }

    [HarmonyPatch(typeof(LobbyManager))]
    [HarmonyPatch("JoinServer")]
    internal class LobbyManager_JoinServer_Patch
    {

        public static bool Prefix(RoomInfo info)
        {
            
            if (!info.IsOpen || info.PlayerCount >= (int)info.MaxPlayers || !info.IsVisible)
            {
               // Main.lobby.RefreshList();
                return false;
            }
            if (XRDevice.isPresent)
            {
                // this.SaveVRPlayerPositions();
            }
            else
            {
                //this.SavePCPlayerPositions();
            }
            Debug.Log($"[GhostMod] 4");
            PhotonNetwork.JoinRoom(info.Name);
            return false;
        }
    }

    [HarmonyPatch(typeof(ServerListItem))]
    [HarmonyPatch("Awake")]
    internal class LobbyManager_Update_Patch
    {
        public static bool Prefix(ServerListItem __instance)
        {
            //var ghost = __instance.GetComponentInChildren<LobbyManager>();
            Debug.Log($"[GhostMod] instance {__instance.lobbyManager}");
            return true;
        }
    }

    [HarmonyPatch(typeof(InventoryItem))]
    [HarmonyPatch("Awake")]
    internal class InventoryItem_Awake_Patch
    {
        public static bool Prefix(InventoryItem __instance)
        {
             __instance.view =  __instance.GetComponent<PhotonView>();
            PlayerPrefs.SetInt("current" + __instance.itemName + "Amount", 0);
            PlayerPrefs.SetInt("total" + __instance.itemName + "Amount", 0);
            for (int i = 0; i < 8; i++)
            {
                __instance.players.Add(new InventoryItem.PlayerInventoryItem());
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(InventoryManager))]
    [HarmonyPatch("AddButton")]
    internal class InventoryManager_AddButton_Patch
    {
        public static bool Prefix(InventoryItem item)
        {
            //Debug.Log($"[GhostMod] View: {item.}");
            item.maxAmount = 8;
            return true;
        }
    }
}


