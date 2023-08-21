using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.API.Features;
using Exiled.API.Enums;
using PlayerRoles;
using Exiled.API.Features.Roles;
using PluginAPI.Roles;
using UnityEngine;
using Exiled.API.Extensions;
using MEC;
using Mirror;
using System.Text.RegularExpressions;
namespace SCPTeamStatsv2
{
    public class EventHandlers
    {
        public List<Player> getSCPs()
        {
            List<Player> scps = new List<Player>();
            foreach (Player pl in Player.List)
            {
                if (Plugin.Instance.Config.hideNPCs && pl.IsNPC)
                {
                    continue;
                }
                if (pl.IsScp && (pl.Role.Type != RoleTypeId.Scp0492))
                {
                    scps.Add(pl);
                }
            }
            return scps;
        }
        public string RepeatLinq(string text, int n)
        {
            return string.Concat(System.Linq.Enumerable.Repeat(text, (int)n));
        }
        public string formatDisplayExperiment(List<Player> scps)
        {
            string display = string.Empty;

            if (Plugin.Instance.Config.bottomDisplay)
            {
                display = display + new string('\n', 18);
            }

            display = display + "<size=" + Plugin.Instance.Config.textSize.ToString() + "%>";



            foreach (Player pl in scps)
            {
                string cool = "<color=red>" + "SCP-" + pl.Role.Type.ToString().Replace("Scp", string.Empty) + "</color> ";

                if (pl.Role.Type != RoleTypeId.Scp079)
                    cool = cool + "<color=green>" + Math.Ceiling(pl.Health).ToString() + "</color> " + "<color=#5a97fa>" + Math.Ceiling(pl.HumeShield).ToString() + "</color>";
                else
                    cool = cool + "<color=#1081e3>LVL </color><color=#5a97fa>" + pl.Role.As<Scp079Role>().Level.ToString() + "</color>";

                display = display + "<color=white>[</color>" + cool + "<color=white>]</color> ";
            }


            if (!Plugin.Instance.Config.bottomDisplay)
            {
                display = display + new string('\n', 51);
            }
            return display;
        }
        public IEnumerator<float> refreshDisplay()
        {
            while (true)
            {
                List<Player> scps = getSCPs();
                if (scps.Count <= 0)
                {
                    yield return Timing.WaitForSeconds(Plugin.Instance.Config.DisplayRefreshRate);
                }
                string displayText = formatDisplayExperiment(scps);
                foreach (Player pl in Player.List)
                {
                    if (!pl.IsScp)
                    {
                        continue;
                    }
                    if (pl.IsNPC)
                    {
                        continue;
                    }
                    pl.ShowHint(displayText);
                }
                yield return Timing.WaitForSeconds(Plugin.Instance.Config.DisplayRefreshRate);
            }
        }
    }
}