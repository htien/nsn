namespace NewSocialNetwork.Website.Models
{
    public class CreateCountryModel : EditCountryModel
    {
    }

    public class EditCountryModel
    {
        public string CountryIso { get; set; }

        public string Name { get; set; }

        public short Order { get; set; }
    }
}