using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models
{
    public class Vaccination
    {
        public Vaccination() 
        {

        }

        public DateTime VaccinationDate { get; set; }
        public string BatchNumber { get; set; }
        public string VaccineName { get; set; }
        public string VaccineManufacturer { get; set; }
        public int DoseNumber { get; set; }
        public int VaccinationSiteId { get; set; }
        public string VaccinationSiteName { get; set; }

    }
}
