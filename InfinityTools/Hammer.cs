using System;
using HarmonyLib;
using InfinityHammer;
using ServerDevcommands;
using Service;
using UnityEngine;
namespace InfinityTools;

public static class Hammer
{
  public static bool IsHammer(string name) => ToolManager.Tools.ContainsKey(name.ToLower());
  public static bool IsHammer(GameObject obj) => obj && IsHammer(Utils.GetPrefabName(obj));
  public static bool IsHammer(ItemDrop.ItemData item) => item != null && IsHammer(item.m_dropPrefab);
  public static bool HasHammer(Player player) => player && IsHammer(player.GetRightItem());
  public static void Equip()
  {
    var player = Helper.GetPlayer();
    if (HasHammer(player)) return;
    var inventory = player.GetInventory();
    var item = inventory.m_inventory.Find(IsHammer) ?? throw new InvalidOperationException($"Unable to find the hammer.");

    player.EquipItem(item);
  }

  public static bool Is(ItemDrop.ItemData item) => item != null && item.m_shared.m_buildPieces != null;

  public static bool HasAny()
  {
    var player = Helper.GetPlayer();
    return player && Is(player.GetRightItem());
  }
  public static string Get()
  {
    var player = Helper.GetPlayer();
    if (!player) return "";
    var item = player.GetRightItem();
    if (item == null) return "";
    return Utils.GetPrefabName(item.m_dropPrefab).ToLower();
  }
}
