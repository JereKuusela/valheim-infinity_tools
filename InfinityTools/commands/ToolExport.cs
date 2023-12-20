using ServerDevcommands;
using UnityEngine;

namespace InfinityTools;

public class ToolExportCommand
{
  public ToolExportCommand()
  {
    Helper.Command("tool_export", "[hammer/hoe/etc] [name] - Export a tool.", Execute);
    AutoComplete.Register("tool_export", (int index, int subIndex) => null);
  }
  protected static void Execute(Terminal.ConsoleEventArgs args)
  {
    Helper.ArgsCheck(args, 2, "Missing the equipment.");
    Helper.ArgsCheck(args, 3, "Missing the tool.");
    var equipment = args[1];
    var name = string.Join(" ", args.Args, 2, args.Length - 1);
    GUIUtility.systemCopyBuffer = ToolManager.Export(equipment, name);
    Helper.AddMessage(args.Context, $"Export tool {name} to the clipboard.");
  }
}