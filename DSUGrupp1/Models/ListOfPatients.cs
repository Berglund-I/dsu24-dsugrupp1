namespace DSUGrupp1.Models
{
    public class ListOfPatients
    {
        private static List<Patient> _patientList;

        public static List<Patient> PatientList 
        {
            get{ return _patientList; } set { _patientList = value; }
        }
    }
}
