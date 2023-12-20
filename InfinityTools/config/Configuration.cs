using System.Collections.Generic;
using BepInEx.Configuration;
using InfinityHammer;
using Service;

namespace InfinityTools;
public partial class Configuration
{
#nullable disable
  public static ConfigEntry<bool> configShapeCircle;
  public static bool ShapeCircle => configShapeCircle.Value;
  public static ConfigEntry<bool> configShapeRing;
  public static bool ShapeRing => configShapeRing.Value;
  public static ConfigEntry<bool> configShapeSquare;
  public static bool ShapeSquare => configShapeSquare.Value;
  public static ConfigEntry<bool> configShapeRectangle;
  public static bool ShapeRectangle => configShapeRectangle.Value;
  public static ConfigEntry<bool> configShapeFrame;
  public static bool ShapeFrame => configShapeFrame.Value;
  public static ConfigEntry<bool> configIgnoreWards;
  public static ConfigEntry<bool> configShowCommandValues;
  public static bool AlwaysShowCommand => configShowCommandValues.Value;
  public static ConfigEntry<string> commandDefaultSize;
  public static float CommandDefaultSize => ConfigWrapper.TryParseFloat(commandDefaultSize);

  public static ConfigWrapper Wrapper;

#nullable enable


  public static void Init(ConfigWrapper wrapper)
  {
    Wrapper = wrapper;
    var section = "1. General";
    configShowCommandValues = wrapper.Bind(section, "Show command values", false, "Always shows the command in the tool descriptions.");
    configShapeCircle = wrapper.Bind(section, "Shape circle", true, "Enables circle shape for commands.");
    configShapeRing = wrapper.Bind(section, "Shape ring", false, "Enables ring shape for commands.");
    configShapeRectangle = wrapper.Bind(section, "Shape rectangle", true, "Enables rectangle shape for commands.");
    configShapeSquare = wrapper.Bind(section, "Shape square", true, "Enables square shape for commands.");
    configShapeFrame = wrapper.Bind(section, "Shape frame", false, "Enables frame shape for commands.");

    commandDefaultSize = wrapper.Bind(section, "Command default size", "10", "Default size for commands.");
    commandDefaultSize.SettingChanged += (s, e) =>
    {
      Scaling.Command.SetScaleX(CommandDefaultSize);
      Scaling.Command.SetScaleZ(CommandDefaultSize);
    };
    Scaling.Command.SetScaleX(CommandDefaultSize);
    Scaling.Command.SetScaleZ(CommandDefaultSize);
    Scaling.Command.SetScaleY(0f);

    InitBinds(wrapper);
  }

}
