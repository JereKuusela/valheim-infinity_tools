using System.IO;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using InfinityHammer;
using Service;
namespace InfinityTools;
[BepInPlugin(GUID, NAME, VERSION)]
[BepInDependency("world_edit_commands", "1.20")]
[BepInDependency("server_devcommands", "1.20")]
[BepInDependency("infinity_hammer", "1.44")]
public class InfinityTools : BaseUnityPlugin
{
  public const string GUID = "infinity_tools";
  public const string NAME = "Infinity Tools";
  public const string VERSION = "1.0";
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
    new ToolScaleCommand();
    new ToolZoomCommand();
  }
  public void LateUpdate()
  {
    Ruler.Update();
  }
}


[HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Start))]
public class FejdStartupStart
{
  static void Create()
  {
    var pars = "from=x,z,y circle=r1-r2 angle=a rect=w1-w2,d";
    var parsSpawn = "from=x,z,y radius=r1-r2";
    var parsTo = "to=tx,tz,ty circle=r1-r2 rect=w1-w2,d";
    var sub = ServerDevcommands.Settings.Substitution;
    Console.instance.TryRunCommand($"alias tool_terrain terrain {pars}");
    Console.instance.TryRunCommand($"alias tool_object object {pars} height=h ignore=ignore");
    Console.instance.TryRunCommand($"alias tool_spawn spawn_object {sub} {parsSpawn}");

    Console.instance.TryRunCommand($"alias tool_terrain_to tool_shape rectangle;terrain {parsTo}");
    Console.instance.TryRunCommand($"alias tool_slope tool_terrain_to slope; tool_scale_x {sub}");

  }
  static void Postfix()
  {
    var pars = "from=x,z,y circle=r angle=a rect=w,d";
    Console.instance.TryRunCommand($"alias tool_area hammer {pars} height=h");
    Create();
  }
}


[HarmonyPatch(typeof(Chat), nameof(Chat.Awake))]
public class ChatAwake
{
  static void Postfix()
  {
    InfinityTools.Wrapper.Bind();
  }
}
