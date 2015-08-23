using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace LiveSplit.Quake2
{
    class Component : LogicComponent
    {
        private Settings settings = new Settings();
        private TimerModel model = null;

        private Process gameProcess = null;
        private GameInfo info = null;
        private GameEvent[] eventList = null;

        public override string ComponentName => "Quake 2 Auto Splitter";

        public Component(LiveSplitState state)
        {
            model = new TimerModel() { CurrentState = state };
            model.CurrentState.OnStart += State_OnStart;

            eventList = settings.GetEventList();
            settings.EventsChanged += settings_EventsChanged;
        }

        private void State_OnStart(object sender, EventArgs e)
        {
            model.InitializeGameTime();
        }

        public override void Update(UI.IInvalidator invalidator, Model.LiveSplitState state, float width, float height, UI.LayoutMode mode)
        {
            if (gameProcess != null && !gameProcess.HasExited)
            {
                info.Update();
                if (eventList[state.CurrentSplitIndex + 1].HasOccured(info))
                {
                    if (state.CurrentPhase == TimerPhase.NotRunning)
                    {
                        state.IsGameTimePaused = false;
                        model.Start();
                    }
                    else
                    {
                        model.Split();
                    }
                }
                
                if (settings.PauseGameTime)
                {
                    state.IsGameTimePaused = !info.InGame || info.InIntermission;
                }
            }
            else
            {
                gameProcess = Process.GetProcessesByName("q2pro").FirstOrDefault();
                if (gameProcess != null)
                {
                    info = new GameInfo(gameProcess);
                }
            }
        }

        private void settings_EventsChanged(object sender, EventArgs e)
        {
            eventList = settings.GetEventList();
        }
        
        public override System.Windows.Forms.Control GetSettingsControl(UI.LayoutMode mode)
        {
            return settings;
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            return settings.GetSettings(document);
        }

        public override void SetSettings(XmlNode settings)
        {
            this.settings.SetSettings(settings);
        }

        public override void Dispose()
        {
            model.CurrentState.OnStart -= State_OnStart;
            settings.EventsChanged -= settings_EventsChanged;
            settings.Dispose();
        }
    }
}
