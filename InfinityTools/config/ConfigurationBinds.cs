using BepInEx.Configuration;
using InfinityHammer;
using Service;
using UnityEngine;
namespace InfinityTools;

public partial class Configuration
{
#nullable disable
  public static ConfigEntry<KeyboardShortcut> shapeKey;
  public static ConfigEntry<KeyboardShortcut> commandRadius;
  public static ConfigEntry<KeyboardShortcut> commandRotate;
  public static ConfigEntry<KeyboardShortcut> commandDepth;
  public static ConfigEntry<KeyboardShortcut> commandHeight;
  public static ConfigEntry<KeyboardShortcut> commandModifier1;
  public static ConfigEntry<KeyboardShortcut> commandModifier2;
  public static ConfigEntry<string> commandHeightAmount;
#nullable enable
  public static string ModifierKey1()
  {
    if (commandModifier1 == null) return "leftalt";
    return ConfigWrapper.GetKeys(commandModifier1.Value);
  }
  public static string ModifierKey2()
  {
    if (commandModifier2 == null) return "leftcontrol";
    return ConfigWrapper.GetKeys(commandModifier2.Value);
  }
  private static void InitBinds(ConfigWrapper wrapper)
  {
    var section = "2. Binds";
    commandModifier1 = wrapper.Bind(section, "Command modifier 1", new KeyboardShortcut(KeyCode.LeftAlt), "");
    commandModifier2 = wrapper.Bind(section, "Command modifier 2", new KeyboardShortcut(KeyCode.LeftControl), "");

    commandHeightAmount = wrapper.Bind(section, "Command height amount", "0.1", "Meters to move.");

    shapeKey = wrapper.BindCommand("tool_shape", section, "Change shape", new KeyboardShortcut(KeyCode.Q), "Changes the selection shape.", "command");
    commandRadius = wrapper.BindWheelCommand(() => $"tool_zoom_x {(Selection.Get().TerrainGrid ? "0.5" : "1")}", section, "Command radius (mouse wheel)", new KeyboardShortcut(KeyCode.None), "Changes the command radius.", "command");
    commandDepth = wrapper.BindWheelCommand(() => $"tool_zoom_z {(Selection.Get().TerrainGrid ? "0.5" : "1")}", section, "Command depth (mouse wheel)", new KeyboardShortcut(KeyCode.LeftShift, KeyCode.LeftAlt), "Changes the command rectangle depth.", "command");
    commandHeight = wrapper.BindWheelCommand(() => $"tool_zoom_y {commandHeightAmount.Value}", section, "Command height (mouse wheel)", new KeyboardShortcut(KeyCode.LeftShift), "Changes the command height.", "command");
    commandRotate = wrapper.BindWheelCommand("hammer_rotate", section, "Command rotation (mouse wheel)", new KeyboardShortcut(KeyCode.LeftShift, KeyCode.LeftControl), "Changes the command rotation.", "command");
  }
}
