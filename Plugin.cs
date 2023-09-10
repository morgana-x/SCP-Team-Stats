using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.API.Enums;
using PlayerRoles;
using Exiled.API.Features.Roles;
using PluginAPI.Roles;
using UnityEngine;
using Exiled.API.Extensions;
using MEC;
using SCPTeamStatsv2;

namespace SCPTeamStatsv2
{
    public sealed class Plugin : Plugin<Config>
    {
        public override string Author => "morgana";

        public override string Name => "SCP Team Stats";

        public override string Prefix => Name;

        public static Plugin Instance;

        private EventHandlers _handlers;

  

        public override void OnEnabled()
        {
            Instance = this;

            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();

            Instance = null;

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _handlers = new EventHandlers();
            Exiled.Events.Handlers.Server.RoundStarted += _handlers.RoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += _handlers.RoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound += _handlers.RoundRestarting;
            Exiled.Events.Handlers.Server.WaitingForPlayers += _handlers.WaitingForPlayers;
            if (Exiled.API.Features.Round.IsStarted && !Exiled.API.Features.Round.IsEnded) 
            {
                _handlers.StartDisplay();
            }
        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= _handlers.RoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= _handlers.RoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound -= _handlers.RoundRestarting;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= _handlers.WaitingForPlayers;
            _handlers.KillDisplay(); // Just Incase
            _handlers = null;
        }
    }
}