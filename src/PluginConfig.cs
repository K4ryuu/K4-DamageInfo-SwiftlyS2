namespace K4DamageInfo;

public sealed partial class Plugin
{
	/// <summary>
	/// Main configuration for K4-DamageInfo plugin
	/// </summary>
	public sealed class PluginConfig
	{
		/// <summary>Show damage summary at round end</summary>
		public bool RoundEndSummary { get; set; } = true;

		/// <summary>Allow damage summary to be shown on death</summary>
		public bool AllowDeathPrint { get; set; } = true;

		/// <summary>Show only killer in round end summary</summary>
		public bool ShowOnlyKiller { get; set; } = false;

		/// <summary>Show friendly fire damage in summary</summary>
		public bool ShowFriendlyFire { get; set; } = false;

		/// <summary>Show all damages from all players</summary>
		public bool ShowAllDamages { get; set; } = false;

		/// <summary>Show center damage info on hit</summary>
		public bool CenterDamageInfo { get; set; } = true;

		/// <summary>Show console damage info on hit</summary>
		public bool ConsoleDamageInfo { get; set; } = true;

		/// <summary>Free-for-all mode (show damage to teammates)</summary>
		public bool FFAMode { get; set; } = false;

		/// <summary>No rounds mode - clear damage on death instead of round end</summary>
		public bool NoRoundsMode { get; set; } = false;

		/// <summary>Timeout for center damage info display (seconds)</summary>
		public int CenterInfoTimeout { get; set; } = 3;
	}
}
