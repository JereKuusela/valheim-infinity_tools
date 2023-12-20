using System;
using InfinityHammer;
using ServerDevcommands;
namespace InfinityTools;
public class ToolScaleCommand
{
  private static void Scale(string amountStr, string direction, Action<float> action)
  {
    var amount = Parse.Direction(direction) * Parse.Float(amountStr, 1f);
    action(amount);
  }
  private static void CommandAxis(string name, string axis, Func<ToolScaling, Action<float>> action)
  {
    name = $"{name}_{axis}";
    AutoComplete.Register(name, (int index) => index == 0 ? ParameterInfo.Create("Amount.") : null);
    Helper.Command(name, $"[amount] - Sets the scale of {axis} axis (if the object supports it).", (args) =>
    {
      HammerHelper.CheatCheck();
      if (!Helper.GetPlayer().InPlaceMode()) return;
      Helper.ArgsCheck(args, 2, "Missing the amount.");
      if (!Selection.Get().IsTool) return;
      if (!Helper.GetPlayer().InPlaceMode()) return;
      var direction = args.Length > 2 ? args[2] : "";
      Scale(args[1], direction, action(Scaling.Get()));
    });
  }
  private static void Command(string name)
  {
    AutoComplete.Register(name, (int index, int subIndex) => index == 0 ? ServerDevcommands.ParameterInfo.XZY("Amount of scale.", subIndex) : null);
    Helper.Command(name, "[amount or x,z,y] - Sets the scale (if the object supports it).", (args) =>
    {
      HammerHelper.CheatCheck();
      if (!Helper.GetPlayer().InPlaceMode()) return;
      if (!Selection.Get().IsTool) return;
      var scaling = Scaling.Get();
      var scale = Parse.Scale(Parse.Split(args[1])) * Parse.Direction(args.Args, 2);
      scaling.SetScale(scale);
      Scaling.UpdateGhost();
    });
  }
  public ToolScaleCommand()
  {
    var name = "tool_scale";
    CommandAxis(name, "x", (scale) => scale.SetScaleX);
    CommandAxis(name, "y", (scale) => scale.SetScaleY);
    CommandAxis(name, "z", (scale) => scale.SetScaleZ);
    Command(name);
  }
}
