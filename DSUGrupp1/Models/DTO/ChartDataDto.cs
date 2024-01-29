namespace DSUGrupp1.Models.DTO
{
    public class ChartDataDto
    {
        public ChartDataDto() 
        {
            Labels = new List<string>() { "Ja", "Nej" };
            Datasets = [new DatasetsDto()];
        }
        public List<string> Labels { get; set; }
        public List<DatasetsDto> Datasets { get; set; }

    }
}
