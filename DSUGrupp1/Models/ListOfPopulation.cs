namespace DSUGrupp1.Models
{
    public class ListOfPopulation
    {
        private static List<Resident> _listOfResidents;

        public static List<Resident> ListOfResidents
        {
            get { return _listOfResidents; }
            set { _listOfResidents = value; }
        }
    }
}
