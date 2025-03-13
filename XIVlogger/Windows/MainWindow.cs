using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace XIVstats.Windows;

public class MainWindow : Window, IDisposable
{
    private uint worldName;
    private Dictionary<string,Location> locations;
    public string? name;

    public MainWindow(Plugin plugin,Dictionary<string, Location> Locations) : base("XIVStats##hidden")
    {

        locations = Locations;

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
    }
    public void updateMapId(uint newMapId)
    {
        this.worldName = newMapId;
        //this.IsOpen = true;
    }
    public void Dispose() { }


    public override void Draw()
    {
        ImGui.Text($"Test text : {worldName}");
        ImGui.Spacing();
      
        ImGui.BeginTabBar("mytabbar");
        {
            if (ImGui.BeginTabItem("Cities"))
            {
                foreach (KeyValuePair<string,Location> entry in locations) 
                {
                    ImGui.Text($"{entry.Value.Name} visit count: {entry.Value.Count}");
                }
                //ImGui.Text($"{locations["12"].Name} visit count: {locations["12"].Count}");
            }
            
            ImGui.EndTabBar();
        }
        //ImGui.Separator();
        //ImGui.TreePop();
        


    }
}
