using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace K4DamageInfo;

public sealed partial class Plugin
{
	private HookResult OnPlayerSpawn(EventPlayerSpawn @event)
	{
		var player = @event.UserIdPlayer;
		if (!IsValidPlayer(player))
			return HookResult.Continue;

		GetPlayerData(player.Slot).IsDataShown = false;
		return HookResult.Continue;
	}

	private HookResult OnPlayerDeath(EventPlayerDeath @event)
	{
		var gameRules = Core.EntitySystem.GetGameRules();
		if (gameRules == null || gameRules.WarmupPeriod || !_config.AllowDeathPrint)
			return HookResult.Continue;

		var victim = @event.UserIdPlayer;
		if (!IsValidPlayer(victim) || victim.IsFakeClient)
			return HookResult.Continue;

		var attackerSlot = @event.Attacker;
		var attacker = attackerSlot >= 0 ? Core.PlayerManager.GetPlayer(attackerSlot) : null;

		var data = GetPlayerData(victim.Slot);
		data.VictimKiller = (attacker != null && IsValidPlayer(attacker)) ? attacker.Slot : -1;

		DisplayDamageInfo(victim);

		if (_config.NoRoundsMode)
			data.DamageInfo.Clear();

		return HookResult.Continue;
	}

	private HookResult OnPlayerHurt(EventPlayerHurt @event)
	{
		var victim = @event.UserIdPlayer;
		var attackerSlot = @event.Attacker;
		var attacker = attackerSlot >= 0 ? Core.PlayerManager.GetPlayer(attackerSlot) : null;

		if (!IsValidPlayer(victim) || !IsValidPlayer(attacker))
			return HookResult.Continue;

		var victimTeam = victim.Controller?.Team ?? Team.None;
		var attackerTeam = attacker!.Controller?.Team ?? Team.None;

		if (victimTeam == attackerTeam && !_config.ShowFriendlyFire)
			return HookResult.Continue;

		int dmgHealth = @event.DmgHealth;
		int dmgArmor = @event.DmgArmor;
		var hitgroup = (HitGroup_t)@event.HitGroup;

		if (!attacker.IsFakeClient && (victimTeam != attackerTeam || _config.FFAMode))
			ShowLiveDamageInfo(attacker, victim, dmgHealth, dmgArmor, hitgroup);

		TrackDamage(victim, attacker, dmgHealth);

		return HookResult.Continue;
	}

	private HookResult OnRoundStart(EventRoundStart @event)
	{
		_playerData.Clear();
		return HookResult.Continue;
	}

	private HookResult OnRoundEnd(EventRoundEnd @event)
	{
		if (!_config.RoundEndSummary)
			return HookResult.Continue;

		foreach (var player in Core.PlayerManager.GetAllPlayers())
		{
			if (!IsValidPlayer(player) || player.IsFakeClient)
				continue;

			var team = player.Controller?.Team ?? Team.None;
			if (team <= Team.Spectator)
				continue;

			DisplayDamageInfo(player);
		}

		_playerData.Clear();
		return HookResult.Continue;
	}

	private void TrackDamage(IPlayer victim, IPlayer attacker, int dmgHealth)
	{
		var gameRules = Core.EntitySystem.GetGameRules();
		if (gameRules == null || gameRules.WarmupPeriod)
			return;

		// Track if round end summary OR death print is enabled (NoRoundsMode uses death print)
		if (!_config.RoundEndSummary && !_config.AllowDeathPrint)
			return;

		var victimData = GetPlayerData(victim.Slot);
		var attackerData = GetPlayerData(attacker.Slot);

		if (!victimData.DamageInfo.TakenDamage.TryGetValue(attacker.Slot, out var takenDamage))
		{
			takenDamage = new DamageInfo();
			victimData.DamageInfo.TakenDamage[attacker.Slot] = takenDamage;
		}

		if (!attackerData.DamageInfo.GivenDamage.TryGetValue(victim.Slot, out var givenDamage))
		{
			givenDamage = new DamageInfo();
			attackerData.DamageInfo.GivenDamage[victim.Slot] = givenDamage;
		}

		takenDamage.TotalDamage += dmgHealth;
		takenDamage.Hits++;
		givenDamage.TotalDamage += dmgHealth;
		givenDamage.Hits++;
	}
}
