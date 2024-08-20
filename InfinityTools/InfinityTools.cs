using BepInEx;
namespace InfinityTools;
[BepInPlugin(GUID, NAME, VERSION)]
public class InfinityTools : BaseUnityPlugin
{
  public const string GUID = "infinity_tools";
  public const string NAME = "Infinity Tools";
  public const string VERSION = "1.7";
  public void Awake()
  {
    Logger.LogWarning("This mod currently does nothing and should be removed");
  }
}
