using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using SwiftlyS2.Shared.Translation;

namespace K4DamageInfo;

public sealed partial class Plugin
{
	private void ShowLiveDamageInfo(IPlayer attacker, IPlayer victim, int dmgHealth, int dmgArmor, HitGroup_t hitgroup)
	{
		var localizer = Core.Translation.GetPlayerLocalizer(attacker);
		string hitgroupName = GetHitgroupName(localizer, hitgroup);

		if (_config.ConsoleDamageInfo)
		{
			attacker.SendConsole(localizer["phrases.console.normal", victim.Controller?.PlayerName ?? "Unknown", dmgHealth, dmgArmor, hitgroupName]);

			if (!victim.IsFakeClient)
			{
				var victimLocalizer = Core.Translation.GetPlayerLocalizer(victim);
				victim.SendConsole(victimLocalizer["phrases.console.inverse", attacker.Controller?.PlayerName ?? "Unknown", dmgHealth, dmgArmor, GetHitgroupName(victimLocalizer, hitgroup)]);
			}
		}

		if (_config.CenterDamageInfo)
		{
			var attackerData = GetPlayerData(attacker.Slot);
			if (!attackerData.RecentDamages.TryGetValue(victim.Slot, out var recentDamage))
			{
				recentDamage = new RecentDamage();
				attackerData.RecentDamages[victim.Slot] = recentDamage;
			}

			recentDamage.AddDamage(dmgHealth);
			attacker.SendCenterHTML(localizer["phrases.center.html", hitgroupName, dmgArmor, recentDamage.TotalDamage], _config.CenterInfoTimeout * 1000);
		}
	}

	private void DisplayDamageInfo(IPlayer player)
	{
		if (player.IsFakeClient)
			return;

		var data = GetPlayerData(player.Slot);
		if (data.IsDataShown)
			return;

		var localizer = Core.Translation.GetPlayerLocalizer(player);

		if (_config.ShowAllDamages)
			DisplayAllPlayersDamage(player, localizer);
		else
			DisplayPlayerDamageInfo(player, data.DamageInfo, localizer);
	}

	private void DisplayAllPlayersDamage(IPlayer player, ILocalizer localizer)
	{
		var data = GetPlayerData(player.Slot);
		if (data.DamageInfo.GivenDamage.Count == 0 && data.DamageInfo.TakenDamage.Count == 0)
			return;

		data.IsDataShown = true;
		player.SendChat(localizer["phrases.summary.startline"]);

		// Collect all player slots we interacted with
		var allSlots = new HashSet<int>(data.DamageInfo.GivenDamage.Keys);
		allSlots.UnionWith(data.DamageInfo.TakenDamage.Keys);

		foreach (int otherSlot in allSlots)
		{
			data.DamageInfo.GivenDamage.TryGetValue(otherSlot, out var given);
			data.DamageInfo.TakenDamage.TryGetValue(otherSlot, out var taken);
			PrintDataLine(player, localizer, otherSlot, given ?? new DamageInfo(), taken ?? new DamageInfo());
		}

		player.SendChat(localizer["phrases.summary.endline"]);
	}

	private void DisplayPlayerDamageInfo(IPlayer player, PlayerDamageInfo info, ILocalizer localizer)
	{
		if (info.GivenDamage.Count == 0 && info.TakenDamage.Count == 0)
			return;

		var data = GetPlayerData(player.Slot);
		data.IsDataShown = true;

		bool headerPrinted = false;
		var processedSlots = new HashSet<int>();

		foreach (var entry in info.GivenDamage)
		{
			int otherSlot = entry.Key;
			if (_config.ShowOnlyKiller && data.VictimKiller != otherSlot)
				continue;

			if (!headerPrinted)
			{
				player.SendChat(localizer["phrases.summary.startline"]);
				headerPrinted = true;
			}

			info.TakenDamage.TryGetValue(otherSlot, out var taken);
			processedSlots.Add(otherSlot);
			PrintDataLine(player, localizer, otherSlot, entry.Value, taken ?? new DamageInfo());
		}

		foreach (var entry in info.TakenDamage)
		{
			int otherSlot = entry.Key;
			if (_config.ShowOnlyKiller && data.VictimKiller != otherSlot)
				continue;

			if (processedSlots.Contains(otherSlot))
				continue;

			if (!headerPrinted)
			{
				player.SendChat(localizer["phrases.summary.startline"]);
				headerPrinted = true;
			}

			PrintDataLine(player, localizer, otherSlot, new DamageInfo(), entry.Value);
		}

		if (headerPrinted)
			player.SendChat(localizer["phrases.summary.endline"]);
	}

	private static void PrintDataLine(IPlayer player, ILocalizer localizer, int otherSlot, DamageInfo given, DamageInfo taken)
	{
		var other = Core.PlayerManager.GetPlayer(otherSlot);
		string otherName = (other != null && other.IsValid) ? (other.Controller?.PlayerName ?? "Unknown") : "Unknown";
		int otherHealth = other?.PlayerPawn?.Health ?? 0;
		string healthString = otherHealth > 0 ? $"{otherHealth}HP" : localizer["phrases.dead"];

		player.SendChat(localizer["phrases.summary.dataline",
			given.TotalDamage, given.Hits, taken.TotalDamage, taken.Hits, otherName, healthString]);
	}

	private static string GetHitgroupName(ILocalizer localizer, HitGroup_t hitgroup)
	{
		string key = hitgroup switch
		{
			HitGroup_t.HITGROUP_GENERIC => "phrases.hitgroup.body",
			HitGroup_t.HITGROUP_HEAD => "phrases.hitgroup.head",
			HitGroup_t.HITGROUP_CHEST => "phrases.hitgroup.chest",
			HitGroup_t.HITGROUP_STOMACH => "phrases.hitgroup.stomach",
			HitGroup_t.HITGROUP_LEFTARM => "phrases.hitgroup.leftarm",
			HitGroup_t.HITGROUP_RIGHTARM => "phrases.hitgroup.rightarm",
			HitGroup_t.HITGROUP_LEFTLEG => "phrases.hitgroup.leftleg",
			HitGroup_t.HITGROUP_RIGHTLEG => "phrases.hitgroup.rightleg",
			HitGroup_t.HITGROUP_NECK => "phrases.hitgroup.neck",
			HitGroup_t.HITGROUP_GEAR => "phrases.hitgroup.gear",
			_ => "phrases.hitgroup.unknown"
		};
		return localizer[key];
	}
}
