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
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace SCP_Team_Stats

{
    public class EventHandlers
    {
        System.Random rnd = new System.Random();
        public List<Player> getSCPs()
        {
            List<Player> scps = new List<Player>();
            foreach (Player pl in Player.List)
            {
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
            bool horizontalDisplay = true;
            bool alignBottom = false;

            display = display + "<size=" + Plugin.Instance.Config.textSize.ToString() + "%>";

            if (horizontalDisplay)
            {
                if (alignBottom)
                {
                    display = display + new string('\n', 28);
                }
                // display = display + new string('\t', 1);
            }
            else
            {
                display = display + new string('\n', 29 - scps.Count);// "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
            }
            foreach (Player pl in scps)
            {
                string cool = string.Empty; //Plugin.Instance.Config.SCPStatString;
                if (!horizontalDisplay)
                {
                    cool = new string('\t', 1) + cool;
                }
                //cool = cool.Replace("{NAME}", string.Empty); // pl.DisplayNickname);
                int hp = rnd.Next(989, 1024); //(int)Math.Ceiling(pl.Health);
                int ap = rnd.Next(989, 1024);//(int)Math.Ceiling(pl.HumeShield);

                cool = cool + "<color=red>" + "SCP-" + pl.Role.Type.ToString().Replace("Scp", string.Empty) + "</color> ";
                if (pl.Role.Type != RoleTypeId.Scp079)
                {
                    cool = cool + "<color=green>" + hp.ToString() + "</color> ";
                    cool = cool + "<color=#5a97fa>" + ap.ToString() + "</color>";
                }
                else
                {
                    cool = cool + "<color=#1081e3>LVL </color><color=#5a97fa>" + pl.Role.As<Scp079Role>().Level.ToString()+"</color>";
                }
                /*cool = cool.Replace("{SCP}", pl.Role.Type.ToString().Replace("Scp", string.Empty));
                cool = cool.Replace("{HEALTH}", Math.Ceiling(pl.Health).ToString());
                cool = cool.Replace("{HUME}", Math.Ceiling(pl.HumeShield).ToString());*/
                if (horizontalDisplay)
                {
                    display = display + "<color=white>[</color>" + cool + "<color=white>]</color> ";
                }
                else
                {
                    display = display + cool + "\n";
                }
            }
            if (horizontalDisplay && !alignBottom)
            {
                display = display + new string('\n', 51);
            }
            //display = display + "</align>";
            // Log.Info(display);
            return display;
        }
        public string formatDisplayOld(List<Player> scps)
        {
            string display = string.Empty;
            bool horizontalDisplay = true;
            bool alignBottom = false;

            display = display + "<size=" + Plugin.Instance.Config.textSize.ToString() + "%>";

            if (horizontalDisplay)
            {
                if (alignBottom)
                {
                    display = display + new string('\n', 28);
                }
               // display = display + new string('\t', 1);
            }
            else
            {
                display = display + new string('\n', 29 - scps.Count);// "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
            }
            string nameline = "";
            string hpapline = "";
            foreach (Player pl in scps)
            {
                string cool = Plugin.Instance.Config.SCPStatString;
                if (!horizontalDisplay)
                {
                    cool = new string('\t', 1) + cool;
                }
                //cool = cool.Replace("{NAME}", string.Empty); // pl.DisplayNickname);
                cool = cool.Replace("{SCP}", pl.Role.Type.ToString().Replace("Scp", string.Empty));
                cool = cool.Replace("{HEALTH}", Math.Ceiling(pl.Health).ToString());
                cool = cool.Replace("{HUME}", Math.Ceiling(pl.HumeShield).ToString());
                if (horizontalDisplay)
                {
                    display = display + "<color=red>[</color>" + cool + "<color=red>]</color> ";
                }
                else
                {
                    display = display + cool + "\n";
                }
            }
            if (horizontalDisplay && !alignBottom)
            {
                display = display + new string('\n', 51);
            }
            //display = display + "</align>";
            // Log.Info(display);
            return display;
        }
        public string formatDisplay(List<Player> scps)
        {
            string display = string.Empty;
            bool horizontalDisplay = true;
            bool alignBottom = false;

            display = display + "<size=" + Plugin.Instance.Config.textSize.ToString() + "%>";
           // display = display + "<align=" + align + ">"; // left>";
            //display = display + "<align=center>";
            if (horizontalDisplay)
            {
                if (alignBottom)
                {
                    display = display + new string('\n', 28);
                }
                display = display +  new string('\t', 1);
            }
            else
            {
                   display = display + new string('\n', 29 - scps.Count);// "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
            }
            string nameline = "";
            string hpapline = "";
            foreach (Player pl in scps)
            {

                int hp = rnd.Next(989, 1024); //(int)Math.Ceiling(pl.Health);
                int ap = rnd.Next(989, 1024);//(int)Math.Ceiling(pl.HumeShield);

                string lencheck = Regex.Replace(nameline, @"\<.*?\>", "");

                string namestuff = "<mspace=0.6em><color=red>" + "SCP-" + pl.Role.Type.ToString().Replace("Scp", string.Empty) + "</color></mspace>";

                string hpstuff = "<mspace=0.6em><color=green>" + hp.ToString() + "HP</color> " + "<color=#5a97fa>" + ap.ToString() + "AP</color></mspace>";


             //   string com = RepeatLinq(" ", lencheck.Length) +  namestuff + hpstuff + "\r";

                nameline = nameline + namestuff + RepeatLinq("   ", 7);
                hpapline = hpapline + hpstuff + "   " ;

                

                /*string cool = Plugin.Instance.Config.SCPStatString;
                if (!horizontalDisplay)
                {
                    cool = new string('\t', 1) + cool;
                }
                //cool = cool.Replace("{NAME}", string.Empty); // pl.DisplayNickname);
                cool = cool.Replace("{SCP}", pl.Role.Type.ToString().Replace("Scp", string.Empty));
                cool = cool.Replace("{HEALTH}", Math.Ceiling(pl.Health).ToString());
                cool = cool.Replace("{HUME}", Math.Ceiling(pl.HumeShield).ToString());
                if (horizontalDisplay)
                {
                    display = display + "<color=red>[</color>" + cool + "<color=red>]</color> ";
                }
                else
                {
                    display = display + cool + "\n";
                }*/
            }
            //display = display + "</align>";
            display = display + nameline + "\n";
            display = display + hpapline;
           // display = display + "</align>";
            display = display + "</size>";
            if ( horizontalDisplay && !alignBottom)
            {
                display = display+new string('\n', 33);
            }
            //Log.Debug(display);
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
                    pl.ShowHint(displayText);
                }
                yield return Timing.WaitForSeconds(Plugin.Instance.Config.DisplayRefreshRate);
            }
        }
    }
}