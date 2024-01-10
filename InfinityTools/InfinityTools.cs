using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Service;
namespace InfinityTools;
[BepInPlugin(GUID, NAME, VERSION)]
[BepInDependency("world_edit_commands", "1.52")]
[BepInDependency("server_devcommands", "1.68")]
[BepInDependency("infinity_hammer", "1.45")]
public class InfinityTools : BaseUnityPlugin
{
  public const string GUID = "infinity_tools";
  public const string NAME = "Infinity Tools";
  public const string VERSION = "1.1";
#nullable disable
  public static ManualLogSource Log;
  public static ConfigWrapper Wrapper;
#nullable enable
  public void Awake()
  {
    Log = Logger;
    new Harmony(GUID).PatchAll();
    Wrapper = new("tool_config", Config);
    Configuration.Init(Wrapper);
    try
    {
      SetupWatcher();
      ToolManager.CreateFile();
      ToolManager.SetupWatcher();
      ToolManager.FromFile();
    }
    catch
    {
      //
    }
  }
#pragma warning disable IDE0051
  private void OnDestroy()
  {
#pragma warning restore IDE0051
    Config.Save();
  }

  private void SetupWatcher()
  {
    FileSystemWatcher watcher = new(Path.GetDirectoryName(Config.ConfigFilePath), Path.GetFileName(Config.ConfigFilePath))
    {
      NotifyFilter = NotifyFilters.Size
    };
    watcher.Changed += ReadConfigValues;
    watcher.IncludeSubdirectories = true;
    watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
    watcher.EnableRaisingEvents = true;
  }

  private void ReadConfigValues(object sender, FileSystemEventArgs e)
  {
    if (!File.Exists(Config.ConfigFilePath)) return;
    try
    {
      Log.LogDebug("ReadConfigValues called");
      Config.Reload();
    }
    catch
    {
      Log.LogError($"There was an issue loading your {Config.ConfigFilePath}");
      Log.LogError("Please check your config entries for spelling and format!");
    }
  }
  public void Start()
  {
    new ToolShapeCommand();
    new ToolCommand();
    new ToolImportCommand();
    new ToolExportCommand();
  }
  public void LateUpdate()
  {
    Ruler.Update();
  }
}

// Chat awake is the earliest point where commands can be safely executed.
[HarmonyPatch(typeof(Chat), nameof(Chat.Awake)), HarmonyPriority(Priority.LowerThanNormal)]
public class SetCommands
{
  private static bool Initialized = false;
  static void CreateAlias()
  {
    if (Initialized) return;
    Initialized = true;
    var pars = "from=<x>,<z>,<y> circle=<r>-<r2> angle=<a> rect=<w>-<w2>,<d>";
    var parsSpawn = "from=<x>,<z>,<y> radius=<r>-<r2>";
    var parsTo = "to=<x>,<z>,<y> circle=<r>-<r2> rect=<w>-<w2>,<d>";
    var sub = ServerDevcommands.Settings.Substitution;
    Console.instance.TryRunCommand($"alias tool_terrain terrain {pars}");
    Console.instance.TryRunCommand($"alias t_t tool tool_terrain");
    Console.instance.TryRunCommand($"alias tool_object object {pars} height=<h> ignore=<ignore>");
    Console.instance.TryRunCommand($"alias t_o tool tool_object");
    Console.instance.TryRunCommand($"alias tool_spawn spawn_object {sub} {parsSpawn}");
    Console.instance.TryRunCommand($"alias t_s tool tool_spawn");

    Console.instance.TryRunCommand($"alias tool_terrain_to terrain {parsTo}");
    // Bit pointless but kept for legacy.
    Console.instance.TryRunCommand($"alias tool_slope tool_terrain_to slope");

    Console.instance.TryRunCommand($"alias tool_area hammer from=<x>,<z>,<y> circle=<r> angle=<a> rect=<w>,<d> height=<h>");

  }
  static void Postfix()
  {
    CreateAlias();
    // Binds track the init status on its own.
    InfinityTools.Wrapper.Bind();
  }
}
