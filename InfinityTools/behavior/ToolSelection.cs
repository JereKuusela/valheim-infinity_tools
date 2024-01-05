
using System.Globalization;
using System.Linq;
using InfinityHammer;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace InfinityTools;
public class ToolSelection : BaseSelection
{
  public RulerShape Shape = RulerShape.Circle;
  public Tool Tool;
  public ToolSelection(Tool tool)
  {
    Tool = tool;
    SelectedPrefab = new GameObject();
    var piece = SelectedPrefab.AddComponent<Piece>();
    piece.m_name = tool.Name;
    piece.m_icon = tool.Icon;
    piece.m_description = tool.Description;
    piece.m_clipEverything = true;
    Ruler.Create(tool);
  }

  public override float MaxPlaceDistance(float value) => 1000f;

  public override bool IsScalingSupported() => true;
  public override bool IsTool => true;
  public override bool Continuous => Tool.Continuous;
  public override bool PlayerHeight => Tool.PlayerHeight;
  public override bool TerrainGrid => Tool.TerrainGrid;
  public override void AfterPlace(GameObject obj)
  {
    HandleCommand();
    Object.Destroy(obj);
  }

  private void HandleCommand()
  {
    var ghost = HammerHelper.GetPlacementGhost().transform;
    var x = ghost.position.x.ToString(CultureInfo.InvariantCulture);
    var y = ghost.position.y.ToString(CultureInfo.InvariantCulture);
    var z = ghost.position.z.ToString(CultureInfo.InvariantCulture);
    var scale = Scaling.Get();
    var radius = scale.X.ToString(CultureInfo.InvariantCulture);
    var innerSize = Mathf.Min(scale.X, scale.Z).ToString(CultureInfo.InvariantCulture);
    var outerSize = Mathf.Max(scale.X, scale.Z).ToString(CultureInfo.InvariantCulture);
    var depth = scale.Z.ToString(CultureInfo.InvariantCulture);
    var width = scale.X.ToString(CultureInfo.InvariantCulture);
    if (Shape == RulerShape.Circle)
    {
      innerSize = radius;
      outerSize = radius;
    }
    if (Shape != RulerShape.Rectangle)
      depth = width;
    if (Shape == RulerShape.Square)
    {
      innerSize = radius;
      outerSize = radius;
    }
    if (Shape == RulerShape.Rectangle)
    {
      innerSize = width;
      outerSize = width;
    }
    var height = scale.Y.ToString(CultureInfo.InvariantCulture);
    var angle = ghost.rotation.eulerAngles.y.ToString(CultureInfo.InvariantCulture);
    if (TerrainGrid) angle = "0";

    var command = Tool.Command;
    var multiShape = command.Contains("#r") && (command.Contains("#w") || command.Contains("#d"));
    if (multiShape)
    {
      var circle = Shape == RulerShape.Circle || Shape == RulerShape.Ring;
      var args = command.Split(' ').ToList();
      for (var i = args.Count - 1; i > -1; i--)
      {
        if (circle && (args[i].Contains("#w") || args[i].Contains("#d")))
          args.RemoveAt(i);
        if (!circle && args[i].Contains("#r"))
          args.RemoveAt(i);
      }
      command = string.Join(" ", args);
    }
    if (command.Contains("#id"))
    {
      var hovered = Selector.GetHovered(InfinityHammer.Configuration.Range, InfinityHammer.Configuration.IgnoredIds);
      if (hovered == null)
      {
        Helper.AddError(Console.instance, "Nothing is being hovered.");
        return;
      }
      command = command.Replace("#id", Utils.GetPrefabName(hovered.gameObject));
    }
    command = command.Replace("#r1-r2", $"{innerSize}-{outerSize}");
    command = command.Replace("#w1-w2", $"{innerSize}-{outerSize}");

    if (Shape == RulerShape.Frame)
      command = command.Replace("#d", $"{innerSize}-{outerSize}");
    else
      command = command.Replace("#d", depth);
    command = command.Replace("#r", radius);
    command = command.Replace("#w", width);
    command = command.Replace("#a", angle);
    command = command.Replace("#x", x);
    command = command.Replace("#y", y);
    command = command.Replace("#z", z);
    command = command.Replace("#tx", x);
    command = command.Replace("#ty", y);
    command = command.Replace("#tz", z);
    command = command.Replace("#h", height);
    command = command.Replace("#ignore", InfinityHammer.Configuration.configIgnoredIds.Value);
    if (!InfinityHammer.Configuration.DisableMessages)
      Console.instance.AddString($"Hammering command: {command}");
    var prev = HideEffects.Active;
    HideEffects.Active = false;
    // Hide effects prevents some visuals from being shown (like status effects).
    Console.instance.TryRunCommand(command);
    HideEffects.Active = prev;
  }
  public override void Activate()
  {
    base.Activate();
    BindCommand.SetMode("command");
    Ruler.Create(Tool);

  }
  public override void Deactivate()
  {
    base.Deactivate();
    Ruler.Remove();
    BindCommand.SetMode("");
  }
}