using Exiled.API.Interfaces;
using System.ComponentModel;

namespace SCPTeamStatsv2
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        [Description("How many seconds between each hint")]
        public int DisplayRefreshRate { get; set; } = 2;

        [Description("Text size (PERCENTAGE NOT PIXEL)")]
        public int textSize { get; set; } = 65;

        [Description("Should NPCs be hidden on list")]
        public bool hideNPCs { get; set; } = true;

        [Description("Should display be shown on bottom of the screen")]
        public bool bottomDisplay { get; set; } = false;
    }
}