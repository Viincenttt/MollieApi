namespace Mollie.Api.Models.Settlement {
	public static class SettlementStatus {
		public const string Open = nameof(Open);
		public const string Pending = nameof(Pending);
		public const string PaidOut = nameof(PaidOut);
		public const string Failed = nameof(Failed);
	}
}