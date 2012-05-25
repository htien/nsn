namespace NewSocialNetwork.Domain
{
    public class NSNPrivacyMode
    {
        public const byte PUBLIC = 0;
        public const byte FRIENDS = 1;
        public const byte CUSTOM = 9;
        public const byte ONLYME = 10;

        private NSNPrivacyMode() { }
    }
}
