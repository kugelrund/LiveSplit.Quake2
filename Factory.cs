using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

namespace LiveSplit.Quake2
{
    public class Factory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Quake 2 Auto Splitter"; }
        }
        public ComponentCategory Category
        {
            get { return ComponentCategory.Control; }
        }
        public string Description
        {
            get { return "Automates splitting for Quake 2 and allows to remove loadtimes."; }
        }
        public IComponent Create(LiveSplitState state)
        {
            return new Component(state);
        }
        public string UpdateName
        {
            get { return ComponentName; }
        }
        public string UpdateURL
        {
            get { return "https://raw.githubusercontent.com/kugelrund/LiveSplit.Quake2/master/"; }
        }
        public Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
        public string XMLURL
        {
            get { return UpdateURL + "Components/update.LiveSplit.Quake2.xml"; }
        }
    }
}