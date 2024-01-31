namespace DSUGrupp1.Models.DTO
{
    public class ChartDataDto
    {
        public ChartDataDto() 
        {
            labels = new List<string>() { "Ja", "Nej" };
            datasets = [new DatasetsDto()];
        }
        public List<string> labels { get; set; }
        public List<DatasetsDto> datasets { get; set; }

    }
}
