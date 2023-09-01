using Exiled.API.Interfaces;
using System.ComponentModel;

namespace SCP_Team_Stats
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        [Description("How many seconds between each hint")]
        public int DisplayRefreshRate { get; set; } = 2;

        [Description("Format of text (UNUSED)")]
        public string SCPStatString { get; set; } = "<color=red>{SCP}</color> <color=green>{HEALTH}</color> <color=#5a97fa>{HUME}</color>";

        [Description("Text size (PERCENTAGE NOT PIXEL)")]
        public int textSize { get; set; } = 65;
    }
}