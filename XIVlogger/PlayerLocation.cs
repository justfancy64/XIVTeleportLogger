
using System;
using System.Linq;
using System.IO;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.Game;
using Dalamud.Plugin.Services;
using System.Runtime.InteropServices.Marshalling;
using System.Collections.Generic;
using Dalamud.Game.Network.Structures;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using Lumina.Excel.Sheets;

namespace XIVstats
{



    public unsafe class OnTerritoryChange : IDisposable
    {
        public Dictionary<string, Location> locations;

        public Lumina.Excel.ExcelSheet<Lumina.Excel.Sheets.TerritoryType> territorysheet { get; set; } = null!;

        public uint currMapId;

        public event Action<uint>? OnMapChanged;
        public string? name;


      
        public OnTerritoryChange()
        {
            territorysheet = Plugin.DataManager.GameData.Excel.GetSheet<Lumina.Excel.Sheets.TerritoryType>();
            InitializeCurrentMapId();
            var datamanager = new DataManager();
            locations = datamanager.dataManager();
        }
        private void InitializeCurrentMapId()
        {
            var GamePtr = GameMain.Instance();
            if (GamePtr != null)
            {
                currMapId = GamePtr->CurrentMapId;
            }
            else
            {
                currMapId = 123;
            }
        }
        public void onTerritoryChange()
        {
            Plugin.framework.Update += this.OnFrameWorkTick;
        }
        public void Dispose()
        {
            Plugin.framework.Update -= this.OnFrameWorkTick;
        }

        public void OnFrameWorkTick(IFramework framework)
        {

            var GamePtr = GameMain.Instance();

            if (GamePtr == null)
            {
                return;
            }
            var newmapId = GamePtr->CurrentMapId;
            name = territorysheet.GetRow(currMapId).Name.ToString();
            

            if (newmapId == currMapId) return;

            //add newmapId to database and fetch its actuall name

            Plugin.framework.RunOnTick(() =>
            {
                OnMapChanged?.Invoke(newmapId);
                //Add newMapId to the database and fetch its actual name
                var datamanager = new DataManager();
                locations[newmapId.ToString()].Count += 1;
                datamanager.WriteToJson(locations);
                currMapId = newmapId;

            });
            
            

            //Service.logger.Information($"changed area to: {currMapId}");


        }


    }

}
