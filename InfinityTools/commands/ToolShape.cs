using System.Collections.Generic;
using InfinityHammer;
using ServerDevcommands;
namespace InfinityTools;
public class ToolShapeCommand
{
  public ToolShapeCommand()
  {
    List<string> shapes = [
      RulerShape.Circle.ToString(),
      RulerShape.Rectangle.ToString(),
      RulerShape.Square.ToString(),
    ];
    AutoComplete.Register("tool_shape", (int index, int subIndex) => index == 0 ? shapes : null);
    Helper.Command("tool_shape", "[shape] - Toggles or sets the selection shape.", (args) =>
    {
      if (Selection.Get() is not CommandSelection selection) return;
      if (args.Length > 1)
      {
        var arg = args[1].ToLower();
        if (arg == RulerShape.Circle.ToString().ToLower()) selection.Shape = RulerShape.Circle;
        else if (arg == RulerShape.Ring.ToString().ToLower()) selection.Shape = RulerShape.Ring;
        else if (arg == RulerShape.Rectangle.ToString().ToLower()) selection.Shape = RulerShape.Rectangle;
        else if (arg == RulerShape.Frame.ToString().ToLower()) selection.Shape = RulerShape.Frame;
        else if (arg == RulerShape.Square.ToString().ToLower()) selection.Shape = RulerShape.Square;
        else return;
      }
      else
      {
        if (selection.Shape == RulerShape.Circle) selection.Shape = RulerShape.Ring;
        else if (selection.Shape == RulerShape.Ring) selection.Shape = RulerShape.Square;
        else if (selection.Shape == RulerShape.Square) selection.Shape = RulerShape.Frame;
        else if (selection.Shape == RulerShape.Frame) selection.Shape = RulerShape.Rectangle;
        else if (selection.Shape == RulerShape.Rectangle) selection.Shape = RulerShape.Circle;

        Ruler.SanityCheckShape(selection);
      }
      Helper.AddMessage(args.Context, $"Selection shape set to {selection.Shape}.");
    });
  }
}
