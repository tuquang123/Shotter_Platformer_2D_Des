public class KeyChainData {
	public string userId { get; set; }
	public string uuid { get; set; }

	public override string ToString ()
	{
		return string.Format ("[KeyChainData: userId={0}, uuid={1}]", userId, uuid);
	}
}
