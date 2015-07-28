using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.Quake2
{
    partial class Settings : UserControl
    {
        private readonly GameEvent[] eventList;
        private readonly Dictionary<string, int> order;

        public bool PauseGameTime { get; private set; }

        private bool eventsChanged = false;
        public event EventHandler EventsChanged;
        protected virtual void OnChanged(EventArgs e)
        {
            if (EventsChanged != null)
            {
                EventsChanged(this, e);
            }
        }

        public Settings()
        {
            InitializeComponent();

            eventList = GameEvent.GetEventList();
            order = new Dictionary<string, int>();
            for (int i = 0; i < eventList.Length; ++i)
            {
                order.Add(eventList[i].Id, i);
            }

            lstAvailEvents.Items.AddRange(eventList);
            PauseGameTime = false;
        }

        public GameEvent[] GetEventList()
        {
            int length = lstUsedEvents.Items.Count;
            GameEvent[] gameEvents = new GameEvent[length + 1];
            for (int i = 0; i < length; ++i)
            {
                gameEvents[i] = (lstUsedEvents.Items[i] as GameEvent);
            }
            gameEvents[length] = new EmptyEvent();

            return gameEvents;
        }

        private int GetEventPosition(ListBox.ObjectCollection collection, GameEvent gameEvent)
        {
            int top = collection.Count;
            if (top == 0) return 0;
            int bottom = 0;
            int mid;
            
            while (bottom + 1 != top)
            {
                mid = (bottom + top) / 2;
                if (gameEvent.CompareTo(collection[mid] as GameEvent) < 0)
                {
                    top = mid;
                }
                else
                {
                    bottom = mid;
                }
            }

            if (gameEvent.CompareTo(collection[bottom] as GameEvent) > 0)
            {
                return top;
            }
            else
            {
                return bottom;
            }
        }

        private void SwitchSelectedEvents(ListBox source, ListBox destination)
        {
            source.BeginUpdate();
            destination.BeginUpdate();
            destination.SelectedIndices.Clear();
            int insertPosition;
            while (source.SelectedItems.Count > 0)
            {
                insertPosition = GetEventPosition(destination.Items, source.SelectedItems[0] as GameEvent);
                destination.Items.Insert(insertPosition, source.SelectedItems[0]);
                destination.SelectedIndices.Add(insertPosition);
                source.Items.RemoveAt(source.SelectedIndices[0]);
            }
            source.EndUpdate();
            destination.EndUpdate();
            eventsChanged = true;
        }

        private void FillEvents(ListBox fill, ListBox empty)
        {
            empty.Items.Clear();
            fill.Items.Clear();
            fill.Items.AddRange(eventList);
            eventsChanged = true;
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            SwitchSelectedEvents(lstAvailEvents, lstUsedEvents);
        }

        private void btnRemoveEvent_Click(object sender, EventArgs e)
        {
            SwitchSelectedEvents(lstUsedEvents, lstAvailEvents);
        }

        private void btnAllEvents_Click(object sender, EventArgs e)
        {
            FillEvents(lstUsedEvents, lstAvailEvents);
        }

        private void btnNoEvents_Click(object sender, EventArgs e)
        {
            FillEvents(lstAvailEvents, lstUsedEvents);
        }

        private void chkPauseGameTime_CheckedChanged(object sender, EventArgs e)
        {
            PauseGameTime = chkPauseGameTime.Checked;
        }
        
        private void settings_HandleDestroyed(object sender, EventArgs e)
        {
            if (eventsChanged)
            {
                eventsChanged = false;
                OnChanged(EventArgs.Empty);
            }
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            XmlElement settingsNode = document.CreateElement("settings");

            XmlElement usedEventsNode = document.CreateElement("usedEvents");
            XmlElement eventNode;
            foreach (GameEvent gameEvent in lstUsedEvents.Items)
            {
                eventNode = document.CreateElement("event");
                eventNode.InnerText = gameEvent.Id;
                usedEventsNode.AppendChild(eventNode);
            }
            settingsNode.AppendChild(usedEventsNode);

            XmlElement pauseGameTimeNode = document.CreateElement("pauseGameTime");
            pauseGameTimeNode.InnerText = PauseGameTime.ToString();
            settingsNode.AppendChild(pauseGameTimeNode);

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            if (settings["usedEvents"] != null)
            {
                FillEvents(lstAvailEvents, lstUsedEvents);
                lstAvailEvents.BeginUpdate();
                lstUsedEvents.BeginUpdate();
                int currOrder;
                int i = 0;
                foreach (XmlNode node in settings["usedEvents"].ChildNodes)
                {
                    if (order.TryGetValue(node.InnerText, out currOrder))
                    {
                        lstUsedEvents.Items.Add(eventList[currOrder]);
                        lstAvailEvents.Items.RemoveAt(currOrder - i);
                        ++i;
                    }
                }
                lstAvailEvents.EndUpdate();
                lstUsedEvents.EndUpdate();

                eventsChanged = false;
                OnChanged(EventArgs.Empty);
            }

            bool pauseGameTime;
            if (settings["pauseGameTime"] != null && Boolean.TryParse(settings["pauseGameTime"].InnerText, out pauseGameTime))
            {
                PauseGameTime = pauseGameTime;
                chkPauseGameTime.Checked = PauseGameTime;
            }
        }
    }
}
