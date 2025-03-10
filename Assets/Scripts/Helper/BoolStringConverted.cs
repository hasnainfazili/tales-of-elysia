public class BoolStringConverted
{
    public static string BoolToString(bool value)
    {
        return value.ToString().ToLower();
    }

    public static bool StringToBool(string value, out bool result)
    {
        if (string.Equals(value, "true"))
        {
            result = true;
            return true;
        }
        else if (string.Equals(value, "false"))
        {
            result = false;
            return true;
        }
        else
        {
            result = false;
            return false;
        }
    }
}
