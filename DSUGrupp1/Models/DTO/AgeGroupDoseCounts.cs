namespace DSUGrupp1.Models.DTO
{
    public class AgeGroupDoseCounts
    {

        public int FirstDoseCount { get; set; }
        public int SecondDoseCount { get; set; }
        public int BoosterDoseCount { get; set; }

        public string AgeGroup { get; set; }

        //skriv om denna 
        //public void Aggregate(AgeGroupDoseCounts other)
        //{
        //    this.FirstDoseCount += other.FirstDoseCount;
        //    this.SecondDoseCount += other.SecondDoseCount;
        //    this.BoosterDoseCount += other.BoosterDoseCount;
        //}

    }

}