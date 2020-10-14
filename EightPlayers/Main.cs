using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using System.Reflection;
using UnityEngine.XR;

namespace EightPlayers
{
    public class Main
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
                // this.RefreshList();
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
            __instance.view = __instance.GetComponent<PhotonView>();
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


