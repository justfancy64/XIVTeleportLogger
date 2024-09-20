using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using XIVstats.Windows;
using System.Collections.Generic;

namespace XIVstats;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IFramework framework { get; private set; } = null!;

    private OnTerritoryChange terrChange;

    private const string CommandName = "/stats";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("SamplePlugin");

    public readonly IClientState _clientstate;
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }

    private MainWindow TestWindow { get; init; }

    private Dictionary<string, Location> locations;


    public Plugin(IClientState clientState)
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        _clientstate = clientState;

        // you might normally want to embed resources and load them from the manifest stream

        //adding current map to testwindow

        terrChange = new OnTerritoryChange();
        terrChange.onTerritoryChange();
        locations = terrChange.locations;




        ConfigWindow = new ConfigWindow(this);

        MainWindow = new MainWindow(this, locations);

        terrChange.OnMapChanged += OnMapChanged;

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);
        //terrChange.Dispose();

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Opens the main stat window"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        //PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;

    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        TestWindow.Dispose();
        CommandManager.RemoveHandler(CommandName);
    }

    private void OnMapChanged(uint newMapId)
    {
        TestWindow.updateMapId(newMapId);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just toggle the display status of our main ui
        ToggleMainUI();
 
    }

    private void DrawUI() => WindowSystem.Draw();

    //public void ToggleConfigUI() => ConfigWindow.Toggle();

    public void ToggleMainUI() => MainWindow.Toggle();


}
