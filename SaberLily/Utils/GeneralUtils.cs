namespace SaberLily.Utils
{
    public class GeneralUtils
    {
        public static string Gender(int gender)
        {
            switch (gender)
            {
                case 1: return "Male";
                case 2: return "Female";
                default: return "Unspecified";
            }
        }
    }
}
