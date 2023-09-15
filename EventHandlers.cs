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

        public CoroutineHandle displayHandle;
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
        public List<int> getGenerators()
        {
            // todo: Optimise whatever the heck this is
            int engagedGenerators = 0;
            int activeGenerators = 0;
            foreach (Generator g in Generator.List)
            {
                if (g.IsEngaged)
                {
                    engagedGenerators++;
                    continue;
                }
                if (g.IsActivating)
                {
                    activeGenerators++;
                    continue;
                }

            }
            return new List<int>(){engagedGenerators, activeGenerators};
        }
        public void RoundEnded(RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines(displayHandle);
        }
        public int getZombieCount()
        {
            return Player.List.Where(x => x.Role.Type == RoleTypeId.Scp0492).Count();
        }
        public string borderText(string text)
        {
            if (!Plugin.Instance.Config.showBorder)
                return text + " ";
            return "<color=" + Plugin.Instance.Config.borderColor.ToHex() + "> " +
                    Plugin.Instance.Config.borderCharacters[0] + "</color>" + text +
                    "<color=" +
                    Plugin.Instance.Config.borderColor.ToHex() +
                    ">" +
                    Plugin.Instance.Config.borderCharacters[1] +
                    "</color> "; ;
        }
        public string formatPlayerInfo(Player pl)
        {
            string cool = "<color=red>" + "SCP-" + pl.Role.Type.ToString().Replace("Scp", string.Empty) + "</color> ";

            if (pl.Role.Type != RoleTypeId.Scp079)
            {
                cool = cool + "<color=green>" + Math.Ceiling(pl.Health).ToString() + "</color>";
                if (pl.HumeShield > 0)
                    cool = cool + " <color=#5a97fa>" + Math.Ceiling(pl.HumeShield).ToString() + "</color>";
                if (pl.ArtificialHealth > 0)
                    cool = cool + " <color=yellow> " + Math.Ceiling(pl.ArtificialHealth).ToString() + "</color>";
            }
            else
            {
                // todo: Optimise whatever the heck this is
                cool = cool + "<color=#1081e3>LVL </color><color=#5a97fa>" + pl.Role.As<Scp079Role>().Level.ToString() + "</color>";
                List<int> gens = getGenerators();
                int engagedGenerators = gens[0];
                int activeGenerators = gens[1];
                if (engagedGenerators > 0 || activeGenerators > 0)
                {
                    string col = "#5a97fa";
                    if (activeGenerators > 0)
                        col = "#f54c4c";
                    cool = cool + "<color=#1081e3> GEN </color><color=" + col + ">" + (engagedGenerators + activeGenerators).ToString() + "</color>";
                }
            }
            return borderText(cool);
        }
        public string formatDisplayExperiment(List<Player> scps)
        {
            string display = string.Empty;

            if (Plugin.Instance.Config.bottomDisplay)
                display = display + new string('\n', 18);

            display = display + "<size=" + Plugin.Instance.Config.textSize.ToString() + "%>";



            foreach (Player pl in scps)
                display = display + formatPlayerInfo(pl);
        
            if (Plugin.Instance.Config.showZombies)
            {
                int zombies = getZombieCount();
                if (zombies > 0)
                    display = display + borderText("<color=red>SCP-049-2</color> <color=#5a97fa>" + zombies.ToString() + "</color>");
            }
            if (!Plugin.Instance.Config.bottomDisplay)
                display = display + new string('\n', 51);
            return display;
        }
        public IEnumerator<float> refreshDisplay()
        {
            while (true)
            {
                List<Player> scps = getSCPs();

                if (scps.Count <= 0)
                    yield return Timing.WaitForSeconds(Plugin.Instance.Config.DisplayRefreshRate);


                string displayText = formatDisplayExperiment(scps);

                foreach (Player pl in Player.List.Where(x => x.IsScp && !x.IsNPC))
                    pl.ShowHint(displayText, duration:(Plugin.Instance.Config.DisplayRefreshRate + 1f));


                yield return Timing.WaitForSeconds(Plugin.Instance.Config.DisplayRefreshRate);
            }
        }

        public void KillDisplay()
        {
            Timing.KillCoroutines(displayHandle);
            Log.Debug("Killed SCP Team Stats Display");
        }

        public void StartDisplay()
        {
            displayHandle = Timing.RunCoroutine(refreshDisplay());
            Log.Debug("Started SCP Team Stats Display");
        }
        // There no longer does that thing that freezes the server! :)
        public void RoundStarted()
        {
            KillDisplay(); // Im not paranoid I swear
            StartDisplay(); 
        }
        public void RoundRestarting()
        {
            KillDisplay();
        }
        public void WaitingForPlayers()
        {
            KillDisplay();
        }
    }
}