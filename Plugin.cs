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
namespace SCP_Team_Stats
{
    public sealed class Plugin : Plugin<Config>
    {
        public override string Author => "morgana";

        public override string Name => "SCP Team Stats";

        public override string Prefix => Name;

        public static Plugin Instance;

        private EventHandlers _handlers;

        public CoroutineHandle displayHandle;

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
            Log.Debug("Starting coroutine");
            displayHandle = Timing.RunCoroutine(_handlers.refreshDisplay());
        }

        private void UnregisterEvents()
        {
            Timing.KillCoroutines(displayHandle);
            _handlers = null;
            Log.Debug("Killed coroutine");
        }
    }
}