namespace DSUGrupp1.Models.DTO
{
    public class FilterDto
    {
        public string BatchNumber { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int SiteId { get; set; }
        public int NumberOfDoses { get; set; }
        public string TypeOfVaccine { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
     
    }
}
