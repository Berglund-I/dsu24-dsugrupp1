using DSUGrupp1.Models.DTO;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DSUGrupp1.Infastructure
{
    public static class LinqQueryRepository
    {
        /// <summary>
        /// Gets a list of all patients of a specific gender
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public static List<PatientInformationDto> GetPatientsByGender(List<PatientInformationDto> patients, string gender)
        {
            List<PatientInformationDto> result = patients
            .Where(patient => patient.Gender == gender)
            .ToList();

            return result;
        }
        /// <summary>
        /// Gets a list of all patients that has gotten vaccinated with a specifik batch
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="batchNumber"></param>
        /// <returns></returns>
        public static List<PatientInformationDto> GetPatientsByBatchNumber(List<PatientInformationDto> patients, string batchNumber)
        {    
            List<PatientInformationDto> result = patients
            .Where(patient => patient.Vaccinations.Any(b => b.BatchNumber == batchNumber))
            .ToList();

            return result;
        }
        /// <summary>
        /// Gets a list of all patients that has gotten the corresponding number of doses
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="doseNumber"></param>
        /// <returns></returns>
        public static List<PatientInformationDto> GetPatientsByDoseNumber(List<PatientInformationDto> patients, int doseNumber)
        {
            List<PatientInformationDto> result = patients
            .Where(patient => patient.Vaccinations.Any(d => d.DoseNumber == doseNumber))
            .ToList();

            return result;
        }
        /// <summary>
        /// Gets a list of all patients within the age span
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="lowestAge"></param>
        /// <param name="highestAge"></param>
        /// <returns></returns>
        public static List<PatientInformationDto> GetPatientsByAge(List<PatientInformationDto> patients, int lowestAge, int highestAge)
        {
            DateTime dateTime = DateTime.Now;
            int currentYear = dateTime.Year;

            List<PatientInformationDto> result = patients
            .Where(patient => currentYear - int.Parse(patient.YearOfBirth) >= 30 && 
            currentYear - int.Parse(patient.YearOfBirth) <= 50)
            .ToList();

            return result;
        }
        /// <summary>
        /// Gets a list of all patients that has been vaccinated at a specific vaccination site
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static List<PatientInformationDto> GetPatientsByVaccinationCentral(List<PatientInformationDto> patients, int siteId)
        {
            List<PatientInformationDto> result = patients
            .Where(patient => patient.Vaccinations.Any(s => s.VaccinationCentral.SiteId== siteId))
            .ToList();

            return result;
        }

        public static List<PatientInformationDto> GetPatientsByDates(List<PatientInformationDto> patients)
        {
            //patient.Vaccinations.dateOfVaccination bör vara DateTime, inte string
            DateTime dateOne = DateTime.Parse("2020-09-14");
            DateTime dateTwo = DateTime.Parse("2021-09-14");

            List<PatientInformationDto> result = patients
            .Where(patient => patient.Vaccinations.Any(d => DateTime.Parse(d.DateOfVaccination) >= dateOne && 
            DateTime.Parse(d.DateOfVaccination) <= dateTwo))
            .ToList();

            return result;
        }
    }
}
