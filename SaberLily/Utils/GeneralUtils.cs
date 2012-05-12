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

        public static string UserImage(string userImage, int gender)
        {
            if (userImage == null || userImage.Length == 0)
            {
                if (gender == 1)
                {
                    return "default_medium_male.gif";
                }
                else if (gender == 2)
                {
                    return "default_medium_female.gif";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return userImage;
            }
        }
    }
}
