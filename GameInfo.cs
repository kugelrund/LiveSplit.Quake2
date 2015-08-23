using System.Collections.Generic;
using System.Diagnostics;
using LiveSplit.ComponentUtil;

namespace LiveSplit.Quake2
{
    class GameInfo
    {
        // 1 - main menu
        // 7 - in game
        private static readonly DeepPointer gameStateAddress = new DeepPointer(0x31BDC0, new int[] {});
        private static readonly DeepPointer mapAddress = new DeepPointer(0x3086C4, new int[] { });
        private static readonly DeepPointer inIntermissionAddress = new DeepPointer(0x2C679C, new int[] { });

        
        private const int MAX_MAP_LENGTH = 8;

        private Process gameProcess;

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
        }

        private void UpdateMap()
        {
            string map;
            mapAddress.Deref(gameProcess, out map, MAX_MAP_LENGTH);
            if (map.Length > 0 && map != CurrMap)
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
            gameStateAddress.Deref<int>(gameProcess, out currGameState);
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
                              new Map("boss2", 10, false) };
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
}
