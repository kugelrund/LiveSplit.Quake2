
using System.Collections.Generic;
using System.Diagnostics;
using LiveSplit.ComponentUtil;
using System.Windows.Forms;

namespace LiveSplit.Quake2
{
    class GameInfo
    {
        // 1 - main menu
        // 7 - in game
        private DeepPointer gameStateAddress;
        // current map
        private DeepPointer mapAddress;
        // 0 when not in intermission, something != 0 when in intermission
        private DeepPointer inIntermissionAddress;

        
        private const int MAX_MAP_LENGTH = 8;

        private Process gameProcess;

        private GameVersion gameVersion;

        public Process GameProcess
        {
            get
            {
                return gameProcess;
            }
        }
        public int PrevGameState { get; private set; }
        public int CurrGameState { get; private set; }
        public string PrevMap { get; private set; }
        public string CurrMap { get; private set; }
        public bool MapChanged { get; private set; }
        public bool InGame { get; private set; }
        public bool InIntermission
        {
            get
            {
                int inIntermission;
                if (inIntermissionAddress.Deref(gameProcess, out inIntermission))
                {
                    return inIntermission != 0;
                }

                return false;
            }
        }


        public GameInfo(Process gameProcess)
        {
            this.gameProcess = gameProcess;
            if (gameProcess.MainModuleWow64Safe().ModuleMemorySize == 5029888)
            {
                gameVersion = GameVersion.v2014_12_03;
            }
            else if (gameProcess.MainModuleWow64Safe().ModuleMemorySize == 5033984)
            {
                gameVersion = GameVersion.v2016_01_12;
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
                    gameStateAddress = new DeepPointer(0x31BDC0);
                    mapAddress = new DeepPointer(0x3086C4);
                    inIntermissionAddress = new DeepPointer(0x2C679C);
                    break;
                case GameVersion.v2016_01_12:
                    gameStateAddress = new DeepPointer(0x286400);
                    mapAddress = new DeepPointer(0x33FF44);
                    inIntermissionAddress = new DeepPointer(0x2FDF28);
                    break;
            }
        }

        private void UpdateMap()
        {
            string map = mapAddress.DerefString(gameProcess, MAX_MAP_LENGTH);
            if (map != null && map != CurrMap)
            {
                PrevMap = CurrMap;
                CurrMap = map;
                MapChanged = true;
            }
        }

        public void Update()
        {
            PrevGameState = CurrGameState;
            int currGameState;
            gameStateAddress.Deref(gameProcess, out currGameState);
            CurrGameState = currGameState;

            if (PrevGameState != CurrGameState)
            {
                UpdateMap();
                InGame = (CurrGameState == 7);
            }
            else
            {
                MapChanged = false;
            }
        }
    }

    struct Map
    {
        public Map(string name, int unit, bool isSecret)
        {
            this.name = name;
            this.unit = unit;
            this.isSecret = isSecret;
        }

        public string name;
        public int unit;
        public bool isSecret;
    }

    abstract class GameEvent
    {
        private static Dictionary<string, GameEvent> events = null;
        public abstract string Id { get; }

        public static Dictionary<string, GameEvent> GetEvents()
        {
            if (events == null)
            {
                #region maps
                Map[] maps = {new Map("base1", 1, false),
                              new Map("base2", 1, false),
                              new Map("base3", 1, false),
                              new Map("train", 1, true),
                              new Map("bunk1", 2, false),
                              new Map("ware1", 2, false),
                              new Map("ware2", 2, false),
                              new Map("jail1", 3, false),
                              new Map("jail2", 3, false),
                              new Map("jail3", 3, false),
                              new Map("jail4", 3, false),
                              new Map("jail5", 3, false),
                              new Map("security", 3, false),
                              new Map("mintro", 4, false),
                              new Map("mine1", 4, false),
                              new Map("mine2", 4, false),
                              new Map("mine3", 4, false),
                              new Map("mine4", 4, false),
                              new Map("fact1", 5, false),
                              new Map("fact3", 5, true),
                              new Map("fact2", 5, false),
                              new Map("power1", 6, false),
                              new Map("power2", 6, false),
                              new Map("cool1", 6, false),
                              new Map("waste1", 6, false),
                              new Map("waste2", 6, false),
                              new Map("waste3", 6, false),
                              new Map("biggun", 7, false),
                              new Map("hangar1", 8, false),
                              new Map("space", 8, true),
                              new Map("lab", 8, false),
                              new Map("hangar2", 8, false),
                              new Map("command", 8, false),
                              new Map("strike", 8, false),
                              new Map("city1", 9, false),
                              new Map("city2", 9, false),
                              new Map("city3", 9, false),
                              new Map("boss1", 10, false),
                              new Map("boss2", 10, false),
                              new Map("xswamp", 11, false),
                              new Map("xsewer1", 11, false),
                              new Map("xsewer2", 11, false),
                              new Map("xcompnd1", 12, false),
                              new Map("xcompnd2", 12, false),
                              new Map("xreactor", 12, false),
                              new Map("xware", 12, false),
                              new Map("xintell", 12, false),
                              new Map("industry", 13, false),
                              new Map("outbase", 13, false),
                              new Map("w_treat", 13, false),
                              new Map("badlands", 13, false),
                              new Map("refinery", 13, false),
                              new Map("xhangar1", 14, false),
                              new Map("xhangar2", 14, false),
                              new Map("xship", 14, false),
                              new Map("xmoon1", 15, false),
                              new Map("xmoon2", 15, false),
                              new Map("rmine1", 16, false),
                              new Map("rmine2", 16, true),
                              new Map("rlava1", 16, false),
                              new Map("rlava2", 16, false),
                              new Map("rware1", 17, false),
                              new Map("rware2", 17, false),
                              new Map("rbase1", 17, false),
                              new Map("rbase2", 17, false),
                              new Map("rhangar1", 18, false),
                              new Map("rsewer1", 18, false),
                              new Map("rsewer2", 18, false),
                              new Map("rhangar2", 18, false),
                              new Map("rammo1", 19, false),
                              new Map("rammo2", 19, false),
                              new Map("rboss", 20, false)
                };
                #endregion

                events = new Dictionary<string, GameEvent>();
                foreach (Map map in maps)
                {
                    events.Add("loaded_map_" + map.name, new LoadedMapEvent(map));
                }
            }

            return events;
        }

        public abstract bool HasOccured(GameInfo info);
    }
    
    abstract class MapEvent : GameEvent
    {
        protected readonly Map map;

        public int MapUnit
        {
            get
            {
                return map.unit;
            }
        }

        public MapEvent(Map map)
        {
            this.map = map;
        }
    }

    class LoadedMapEvent : MapEvent
    {
        public override string Id { get { return "loaded_map_" + map.name; } }

        public LoadedMapEvent(Map map) : base(map) { }

        public override bool HasOccured(GameInfo info)
        {
            return (info.PrevGameState != 7) && info.InGame && (info.CurrMap == map.name);
        }
                
        public override string ToString()
        {
            if (map.isSecret)
            {
                return "Loaded '" + map.name + "' (secret map)";
            }
            else
            {
                return "Loaded '" + map.name + "'";
            }
        }
    }
    
    class EmptyEvent : GameEvent
    {
        public override string Id { get { return "empty"; } }

        public override bool HasOccured(GameInfo info)
        {
            return false;
        }
    }

    public enum GameVersion
    {
        v2014_12_03, // latest version of original Q2PRO release, build from Dec 3 2014
        v2016_01_12  // first release of modified Q2PRO, build from Jan 12 2016
    }
}
