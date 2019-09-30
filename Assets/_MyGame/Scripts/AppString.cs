

public static class AppString
{
    public static string Distance(float distance)
    {
        if (distance < 1000.0f)
            return string.Format("{0:0.0}m", distance);
        else
            return string.Format("{0:n1}km", distance / 1000.0f);
    }
    public static string Hit(int hit)
    {
        return string.Format("{0}hit", hit);
    }
    public static string GameTime(float time)
    {
        return string.Format("{0:0.0}s", time);
    }
}
