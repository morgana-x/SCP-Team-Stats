using Exiled.API.Interfaces;
using System.ComponentModel;
using UnityEngine;

namespace SCPTeamStatsv2
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = true;

        [Description("How many seconds between each hint")]
        public int DisplayRefreshRate { get; set; } = 2;

        [Description("Text size (PERCENTAGE NOT PIXEL)")]
        public int textSize { get; set; } = 65;

        [Description("Should NPCs be hidden on list")]
        public bool hideNPCs { get; set; } = true;

        [Description("Should display be shown on bottom of the screen")]
        public bool bottomDisplay { get; set; } = false;

        [Description("Border for SCP stats (1 character only at this time) WARNING USING '{}' DOES NOT WORK AND WILL BUG THE SYSTEM!!!!")]
        public string borderCharacters { get; set; } = "[]";

        [Description("Border color")]
        public Color borderColor { get; set; } = Color.white;
    }
}