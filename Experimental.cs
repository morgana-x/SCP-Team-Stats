using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using PlayerRoles;
using Hints;
using Exiled.API.Features.Core;

namespace SCPTeamStatsv2.Experiment
{
    public class Experimental
    {

        public string[][] frame = new string[100][];

        public List<Player> getTeamMates (Team team) 
        {
            List<Player> teamMates = new List<Player>();
            foreach (Player t in Player.List)
            {
                if (t.IsDead) 
                    continue;
                if (t.Role.Team != team)
                    continue;
                
                teamMates.Add(t);
            }
            return teamMates;
        }

        public Vector2 posToScreen(UnityEngine.Camera plCam, Vector3 pos)
        {
            return plCam.WorldToScreenPoint(pos);
        }
        public void newFrame()
        {
            float radius = 5f;
            int width = 20; //10
            frame = new string[width][];
            for (int i = 0; i < frame.Length; i++)
            {
                frame[i] = new string[width];
            }
  
            for (int y = 0; y < frame.Length; y++)
            {
                for (int x = 0; x < frame[y].Length; x++)
                {
                    float xatea = (x - (frame.Length/2));
                    float yatea = (y - (frame.Length / 2));
                    float radiussquare = (xatea * xatea) + (yatea * yatea);
                    double r = Math.Sqrt(radiussquare);
                    //Log.Debug(r);
                    if ( r > radius)
                    {
                        frame[y][x] = " ";
                    }
                    else if (r-radius <= 1)
                    {
                        frame[y][x] = "*";
                    }
                    else
                    {
                        frame[y][x] = "<color=black>*</color>";
                    }
                }
            }
        }
        public string concatFrame()
        {
            string finishedProduct = string.Empty;
            for (int y = 0; y < frame.Length; y++)
            {
                for (int x = 0; x < frame[y].Length; x++)
                {
                    finishedProduct = finishedProduct + frame[y][x];
                }
                finishedProduct = "<line-height=30%>" + finishedProduct + "\n";
            }
            return finishedProduct;
        }
        public void renderTeammates(Player player, List<Player> teamMates) 
        {
            if (player.IsNPC)
                return;
            newFrame();
           /* UnityEngine.Camera cam = player.GameObject.GetComponent<UnityEngine.Camera>();
            if (cam == null)
            {
                
                Log.Debug("Creating fake camera!");
                cam = player.GameObject.AddComponent<UnityEngine.Camera>();
                cam.enabled = false;
            }
            cam.transform.position = player.CameraTransform.position;
            cam.transform.rotation = player.CameraTransform.rotation;
            cam.transform.localScale = player.CameraTransform.localScale;
            cam.transform.forward= player.CameraTransform.forward;
            cam.transform.up= player.CameraTransform.up;
            cam.pixelRect = new Rect(0, 0, frame[0].Length, frame.Length);
            Log.Debug("Init fake cam for " + player.DisplayNickname);
            Log.Debug("New frame");*/
            foreach (Player t in teamMates)
            {
                if (t == player)
                {
                    continue;
                }
                if (t.Zone != player.Zone)
                    continue;
                Vector3 dist = (player.Position - t.Position);
                Vector2 screenPos = new Vector2(dist.x, dist.z); /// (new Vector2(frame[0].Length, frame.Length)) ;
                Vector2 frameSize = (new Vector2(frame.Length, frame[0].Length));
                screenPos = screenPos + (frameSize / 2f);
                float angle = 90 + (player.Transform.eulerAngles.z - t.Transform.eulerAngles.z);
                float cosang = Mathf.Cos(angle);
                float sinang = Mathf.Sin(angle);    
                float x1 = screenPos.x * cosang - screenPos.y * sinang;
                float y1 = screenPos.x * sinang - screenPos.y * cosang;
                screenPos.x = x1;
                screenPos.y = y1;
                if (screenPos.x > frame.Length)
                    screenPos.x = frame.Length-0.1f;
                if (screenPos.y > frame[0].Length)
                    screenPos.y = frame[0].Length-0.1f;
                if (screenPos.x < 0)
                    screenPos.x = 0;
                if (screenPos.y < 0)
                    screenPos.y = 0;

               // Log.Debug(screenPos);
                frame[(int)screenPos.x][(int)screenPos.y] = "<color=" + t.Role.Color.ToHex() +">" + "*" + "</color>";
            }
            string finishedFrame = concatFrame();
           // Log.Debug(finishedFrame);
            player.ShowHint(finishedFrame);
        }
        public void showTeammates(Team team)
        {
            List<Player> teamMates = getTeamMates(team);
            if (teamMates.Count <= 0 )
                return;
            foreach (Player t in teamMates) 
            {
                if (t.IsNPC)
                {
                    continue;
                }
                //Log.Debug("rendering teams for " + t.DisplayNickname);
                renderTeammates(t, teamMates);
            }
        }
      
    }
}
