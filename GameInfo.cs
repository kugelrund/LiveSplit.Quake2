using System;
using System.Diagnostics;

namespace LiveSplit.Quake2
{
    class GameInfo
    {
        // 1 - main menu
        // 7 - in game
        private static readonly DeepPointer gameStateAddress = new DeepPointer(0x31BDC0, new int[] {});
       
        private static readonly DeepPointer mapAddress = new DeepPointer(0x3086C4, new int[] { });


        // longest map name is forgeboss
        private const int MAX_MAP_LENGTH = 10;

        private Process gameProcess;

        public int PrevGameState { get; private set; }
        public int CurrGameState { get; private set; }
        public string PrevMap { get; private set; }
        public string CurrMap { get; private set; }
        public bool MapChanged { get; private set; }
        public bool InGame { get; private set; }


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

    abstract class GameEvent : IComparable<GameEvent>
    {
        private static GameEvent[] eventList = null;

        private int order = -1;
        public int Order { get { return order; } }
        public abstract string Id { get; }

        public static GameEvent[] GetEventList()
        {
            if (eventList == null)
            {
                eventList = new GameEvent[] { 
                    #region events
                    new LoadedMapEvent("base1"),
                    new LoadedMapEvent("base2"),
                    new LoadedMapEvent("base3"),
                    new LoadedMapEvent("bunk1"),
                    new LoadedMapEvent("ware1"),
                    new LoadedMapEvent("ware2"),
                    new LoadedMapEvent("jail1"),
                    new LoadedMapEvent("jail2"),
                    new LoadedMapEvent("jail3"),
                    new LoadedMapEvent("jail4"),
                    new LoadedMapEvent("jail5"),
                    new LoadedMapEvent("security"),
                    new LoadedMapEvent("mintro"),
                    new LoadedMapEvent("mine1"),
                    new LoadedMapEvent("mine2"),
                    new LoadedMapEvent("mine3"),
                    new LoadedMapEvent("mine4"),
                    new LoadedMapEvent("fact1"),
                    new LoadedMapEvent("fact2"),
                    new LoadedMapEvent("power1"),
                    new LoadedMapEvent("power2"),
                    new LoadedMapEvent("biggun"),
                    new LoadedMapEvent("hangar1"),
                    new LoadedMapEvent("lab"),
                    new LoadedMapEvent("hangar2"),
                    new LoadedMapEvent("command"),
                    new LoadedMapEvent("strike"),
                    new LoadedMapEvent("city1"),
                    new LoadedMapEvent("city2"),
                    new LoadedMapEvent("city3"),
                    new LoadedMapEvent("boss1"),
                    new LoadedMapEvent("boss2"),
                    new LoadedMapEvent("victory.pc"),
                    #endregion
                };
                for (int i = 0; i < eventList.Length; ++i)
                {
                    eventList[i].order = i;
                }
            }

            return eventList;
        }

        public int CompareTo(GameEvent other)
        {
            if (order == -1 || other.order == -1)
            {
                throw new ArgumentException();
            }
            else
            {
                return order - other.order;
            }
        }

        public abstract bool HasOccured(GameInfo info);
    }
    
    abstract class MapEvent : GameEvent
    {
        protected readonly string map;

        public MapEvent(string map)
        {
            this.map = map;
        }
    }

    class LoadedMapEvent : MapEvent
    {
        public override string Id { get { return "loaded_map_" + map; } }

        public LoadedMapEvent(string map) : base(map) { }

        public override bool HasOccured(GameInfo info)
        {
            return (info.PrevGameState != 7) && info.InGame && (info.CurrMap == map);
        }
                
        public override string ToString()
        {
            return "Loaded '" + map + "'";
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
