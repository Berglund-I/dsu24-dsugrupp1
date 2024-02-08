using DSUGrupp1.Models.DTO;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DSUGrupp1.Infastructure
{
    public class LinqQueryRepository
    {
        public List<PatientInformationDto> Sorted { get; set; }
        public LinqQueryRepository(List<PatientInformationDto> patients) 
        {
            Sorted = GetPatientsByAge(patients);
                //GetPatientsByDoseNumber(patients);
                //GetPatientsByGenderAndBatchNumber(patients);
                //GetPatientsByGender(patients);
        }
        public List<PatientInformationDto> GetPatientsByGender(List<PatientInformationDto> patients)
        {
            //IQueryable<PatientInformationDto> query = 
            //    from patient in patients
            //    where patient.Gender == "Male"
            //    select patient;

            //return query.ToList();

            List<PatientInformationDto> result = patients
            .Where(patient => patient.Gender == "Male")
            .ToList();

            return result;
        }

        public List<PatientInformationDto> GetPatientsByGenderAndBatchNumber(List<PatientInformationDto> patients)
        {    
            List<PatientInformationDto> result = patients
            .Where(patient => patient.Gender == "Male" && patient.Vaccinations.Any(v => v.BatchNumber == "ER1741"))
            .ToList();
            return result;
        }

        public List<PatientInformationDto> GetPatientsByDoseNumber(List<PatientInformationDto> patients)
        {
            List<PatientInformationDto> result = patients
            .Where(patient => patient.Vaccinations.Any(d => d.DoseNumber == 2))
            .ToList();
            return result;
        }

        public List<PatientInformationDto> GetPatientsByAge(List<PatientInformationDto> patients)
        {
            int lowestAge = 30;
            int highestAge = 50;
            DateTime dateTime = DateTime.Now;
            int currentYear = dateTime.Year;

            List<PatientInformationDto> result = patients
            .Where(patient => currentYear - int.Parse(patient.YearOfBirth) >= 30 && 
            currentYear - int.Parse(patient.YearOfBirth) <= 50)
            .ToList();
            return result;
        }
    }
}
