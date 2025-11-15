public struct FilterInfo
{
	public int pos
	{
		get;
	}

	public string[] data
	{
		get;
	}

	public FilterInfo(int pos, string[] data)
	{
		this.pos = pos;
		this.data = data;
	}

	public string PosString()
	{
		return $"{pos}";
	}
}