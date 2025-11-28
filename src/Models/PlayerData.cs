namespace K4DamageInfo;

public sealed partial class Plugin
{
	/// <summary>
	/// Stores damage tracking data for a single player
	/// </summary>
	private sealed class PlayerData
	{
		/// <summary>Whether damage info has been shown this round</summary>
		public bool IsDataShown { get; set; }

		/// <summary>Slot of the player who killed this player (-1 if none)</summary>
		public int VictimKiller { get; set; } = -1;

		/// <summary>Damage given/taken during this round</summary>
		public PlayerDamageInfo DamageInfo { get; } = new();

		/// <summary>Recent damage tracking for center display aggregation</summary>
		public Dictionary<int, RecentDamage> RecentDamages { get; } = [];

		/// <summary>Reset data for new round</summary>
		public void Reset()
		{
			IsDataShown = false;
			VictimKiller = -1;
			DamageInfo.Clear();
			RecentDamages.Clear();
		}
	}

	/// <summary>
	/// Tracks damage given and taken by a player
	/// </summary>
	private sealed class PlayerDamageInfo
	{
		/// <summary>Damage given to other players (key = victim slot)</summary>
		public Dictionary<int, DamageInfo> GivenDamage { get; } = [];

		/// <summary>Damage taken from other players (key = attacker slot)</summary>
		public Dictionary<int, DamageInfo> TakenDamage { get; } = [];

		public void Clear()
		{
			GivenDamage.Clear();
			TakenDamage.Clear();
		}
	}

	/// <summary>
	/// Single damage record
	/// </summary>
	private sealed class DamageInfo
	{
		public int TotalDamage { get; set; }
		public int Hits { get; set; }
	}

	/// <summary>
	/// Recent damage for center display aggregation
	/// </summary>
	private sealed class RecentDamage
	{
		private static readonly TimeSpan AggregationWindow = TimeSpan.FromSeconds(5);

		public int TotalDamage { get; private set; }
		public DateTime LastDamageTime { get; private set; }

		/// <summary>
		/// Add damage, aggregating if within time window
		/// </summary>
		public void AddDamage(int damage)
		{
			if (DateTime.Now - LastDamageTime <= AggregationWindow)
				TotalDamage += damage;
			else
				TotalDamage = damage;

			LastDamageTime = DateTime.Now;
		}
	}
}
