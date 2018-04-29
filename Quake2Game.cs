﻿
using System;

namespace LiveSplit.Quake2
{
    using ComponentAutosplitter;

    class Quake2Game : Game
    {
        private static readonly Type[] eventTypes = new Type[] { typeof(LoadedMapEvent),
                                                                 typeof(EndEvent) };
        public override Type[] EventTypes => eventTypes;

        public override string Name => "Quake II";
        public override string[] ProcessNames => new string[] {"q2pro", "quake2"};
        public override bool GameTimeExists => false;
        public override bool LoadRemovalExists => true;

        public Quake2Game() : base(new CustomSettingBool[] {})
        {
        }

        public override GameEvent ReadLegacyEvent(string id)
        {
             // fallback to read old autosplitter settings
            if (id.StartsWith("loaded_map_"))
            {
                return new LoadedMapEvent(id.Replace("loaded_map_", ""));
            }
            else if (id == "empty")
            {
                return new EmptyEvent();
            }
            else
            {
                return new EmptyEvent();
            }
        }
    }

    class LoadedMapEvent : MapEvent
    {
        public override string Description => "A certain map was loaded.";

        public LoadedMapEvent() : base()
        {
        }

        public LoadedMapEvent(string map) : base(map)
        {
        }

        public override bool HasOccured(GameInfo info)
        {
            return (info.PrevGameState != Quake2State.InGame) && info.InGame && (info.CurrMap == map);
        }

        public override string ToString()
        {
            return "Map '" + map + "' was loaded";
        }
    }

    class EndEvent : NoAttributeEvent
    {
        public override string Description => "Final button was pressed.";

        public override bool HasOccured(GameInfo info)
        {
            if (info.CurrMap == "boss2")
            {
                return info.FinalButtonPressed;
            }

            return false;
        }

        public override string ToString()
        {
            return "Final button pressed";
        }
    }

    public enum GameVersion
    {
        v2014_12_03, // latest version of original Q2PRO release, build from Dec 3 2014
        v2016_01_12, // first release of modified Q2PRO, build from Jan 12 2016
        v2018_04_22  // first release of QPRO Speed, r1063, build from Apr 22 2018
    }

    public enum Quake2State
    {
        MainMenu = 1, InGame = 7
    }
}

namespace LiveSplit.ComponentAutosplitter
{
    using System.Text;
    using System.Windows.Forms;
    using ComponentUtil;
    using Quake2;

    partial class GameInfo
    {
        // 1 - main menu
        // 7 - in game
        private Int32 gameStateAddress;
        // current map
        private Int32 mapAddress;
        // 0 when not in intermission, something != 0 when in intermission
        private Int32 inIntermissionAddress;
        private Int32 yPositionAddress;
        
        private GameVersion gameVersion;
        
        public Quake2State PrevGameState { get; private set; }
        public Quake2State CurrGameState { get; private set; }
        public string PrevMap { get; private set; }
        public string CurrMap { get; private set; }
        public bool MapChanged { get; private set; }
        public bool InIntermission
        {
            get
            {
                int inIntermission;
                if (gameProcess.ReadValue(baseAddress + inIntermissionAddress, out inIntermission))
                {
                    return inIntermission != 0;
                }

                return false;
            }
        }

        public bool FinalButtonPressed
        {
            get
            {
                gameProcess.ReadValue(baseAddress + yPositionAddress, out float yPosition);
                return yPosition >= 164.75 || yPosition <= -2084.75;
            }
        }

        partial void GetVersion()
        {
            if (gameProcess.MainModuleWow64Safe().ModuleMemorySize == 5029888)
            {
                gameVersion = GameVersion.v2014_12_03;
            }
            else if (gameProcess.MainModuleWow64Safe().ModuleMemorySize == 5033984)
            {
                gameVersion = GameVersion.v2016_01_12;
            }
            else if (gameProcess.MainModuleWow64Safe().ModuleMemorySize == 5079040)
            {
                gameVersion = GameVersion.v2018_04_22;
            }
            else
            {
                MessageBox.Show("Unsupported game version", "LiveSplit.Quake2", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                gameVersion = GameVersion.v2014_12_03;
            }

            switch (gameVersion)
            {
                case GameVersion.v2014_12_03:
                    gameStateAddress = 0x31BDC0;
                    mapAddress = 0x3086C4;
                    inIntermissionAddress = 0x2C679C;
                    yPositionAddress = 0x155744;
                    break;
                case GameVersion.v2016_01_12:
                    gameStateAddress = 0x286400;
                    mapAddress = 0x33FF44;
                    inIntermissionAddress = 0x2FDF28;
                    yPositionAddress = 0x156744;
                    break;
                case GameVersion.v2018_04_22:
                    gameStateAddress = 0x291180;
                    mapAddress = 0x3A7490;
                    inIntermissionAddress = 0x308C68;
                    yPositionAddress = 0x161584;
                    break;
            }
        }

        partial void UpdateInfo()
        {
            PrevGameState = CurrGameState;
            int currGameState;
            gameProcess.ReadValue(baseAddress + gameStateAddress, out currGameState);
            CurrGameState = (Quake2State)currGameState;

            if (PrevGameState != CurrGameState)
            {
                UpdateMap();
            }
            else
            {
                MapChanged = false;
            }

            InGame = (CurrGameState == Quake2State.InGame);
            if (InGame)
            {
                if (InIntermission)
                {
                    InGame = false;
                }
            }
        }

        private void UpdateMap()
        {
            StringBuilder mapBuilder = new StringBuilder(32);
            gameProcess.ReadString(baseAddress + mapAddress, mapBuilder);
            string map = mapBuilder.ToString();

            if (map != null && map != CurrMap)
            {
                PrevMap = CurrMap;
                CurrMap = map;
                MapChanged = true;
            }
        }
    }
}
