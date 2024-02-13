using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models
{
    public class Resident
    {
        public string DeSoCode { get; set; }
        public string Gender {  get; set; }

        public Resident(DataItem p)
        {
            if(p.Key[2] == "1")
            {
                Gender = "Male";
            }
            else
            {
                Gender = "Female";
            }
            DeSoCode = p.Key[0];
        }
    }
}
