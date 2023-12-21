using System;
using ServerDevcommands;
namespace InfinityHammer;
public class ToolZoomCommand
{
  private static void Zoom(string amountStr, string direction, Action<float, float> action)
  {
    if (amountStr.EndsWith("%", StringComparison.Ordinal))
    {
      var amount = Parse.Direction(direction) * Parse.Float(amountStr.Substring(0, amountStr.Length - 1), 5f) / 100f;
      action(0f, amount);
    }
    else
    {
      var amount = Parse.Direction(direction) * Parse.Float(amountStr, 1f);
      action(amount, 0f);
    }
  }
  private static void CommandAxis(string name, string axis, Func<ToolScaling, Action<float, float>> action)
  {
    name = $"{name}_{axis}";
    AutoComplete.Register(name, (int index) =>
    {
      if (index == 0) return ParameterInfo.Create("Flat amount or percentage to scale.");
      return null;
    });
    Helper.Command(name, $"[amount or percentage] - Zooms the {axis} axis (if the object supports it).", (args) =>
    {
      HammerHelper.CheatCheck();
      Helper.ArgsCheck(args, 2, "Missing the amount.");
      if (!Selection.Get().IsTool) return;
      if (!Helper.GetPlayer().InPlaceMode()) return;
      var direction = args.Length > 2 ? args[2] : "";
      Zoom(args[1], direction, action(Scaling.Get()));
      Scaling.UpdateGhost();
      Scaling.PrintScale(args.Context);
    });
  }
  private static void Command(string name)
  {
    AutoComplete.Register(name, (int index, int subIndex) =>
    {
      if (index == 0) return ParameterInfo.Create("Flat amount or percentage to scale.");
      return null;
    });
    Helper.Command(name, "[amount/percentage or x,z,y] - Zooms the selection (if the object supports it).", (args) =>
    {
      HammerHelper.CheatCheck();
      Helper.ArgsCheck(args, 2, "Missing the amount.");
      if (!Selection.Get().IsTool) return;
      if (!Helper.GetPlayer().InPlaceMode()) return;
      var scale = Scaling.Get();
      var split = args[1].Split(',');
      var direction = args.Length > 2 ? args[2] : "";
      if (split.Length == 1)
        Zoom(split[0], direction, scale.Zoom);
      else if (split.Length == 3)
      {
        Zoom(split[0], direction, scale.ZoomX);
        Zoom(split[1], direction, scale.ZoomZ);
        Zoom(split[2], direction, scale.ZoomY);
      }
      else
        throw new InvalidOperationException("Must either have 1 or 3 values.");
      Scaling.UpdateGhost();
      Scaling.PrintScale(args.Context);
    });
  }
  public ToolZoomCommand()
  {
    var name = "tool_zoom";
    CommandAxis(name, "x", (scale) => scale.ZoomX);
    CommandAxis(name, "y", (scale) => scale.ZoomY);
    CommandAxis(name, "z", (scale) => scale.ZoomZ);
    Command(name);
  }
}
