

public static class AppString
{
    public static string Distance(float distance)
    {
        if (distance < 1000.0f)
            return string.Format("{0}m", distance);
        else
            return string.Format("{0:n}km", distance / 1000.0f);
    }
}
