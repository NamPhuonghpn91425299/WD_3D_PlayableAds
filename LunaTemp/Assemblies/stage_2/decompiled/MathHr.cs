public static class MathHr
{
	public static float Remap(float main, float minIn, float maxIn, float minOut, float maxOut)
	{
		if (maxIn - minIn == 0f)
		{
			return (maxOut + minOut) / 2f;
		}
		return minOut + (main - minIn) * (maxOut - minOut) / (maxIn - minIn);
	}
}
