using DSUGrupp1.Models;
using DSUGrupp1.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DSUGrupp1.Infastructure
{
    public static class LinqQueryRepository
    {
        public static List<Patient> GetPatientsByDeSo(List<Patient> patients, string deSoCode)
        {
            return patients.Where(patient => patient.DeSoCode == deSoCode).ToList();
        }
        /// <summary>
        /// Gets a list of all patients of a specific gender
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public static List<Patient> GetPatientsByGender(List<Patient> patients, string gender)
        {
            List<Patient> result = patients
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
        public static List<Patient> GetPatientsByBatchNumber(List<Patient> patients, string batchNumber)
        {
            List<Patient> result = patients
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
        public static List<Patient> GetPatientsByDoseNumber(List<Patient> patients, int doseNumber)
        {
            List<Patient> result = patients
            .Where(patient => patient.Vaccinations.Any(d => d.DoseNumber == doseNumber))
            .ToList();

            return result;
        }
        public static List<Patient> GetPatientsWithBoosterDose(List<Patient> patients)
        {
            return patients
                .Where(patient => patient.Vaccinations.Count > 3)
                .ToList();
        }
        /// <summary>
        /// Gets a list of all patients within the age span
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="lowestAge"></param>
        /// <param name="highestAge"></param>
        /// <returns></returns>
        public static List<Patient> GetPatientsByAge(List<Patient> patients, int lowestAge, int highestAge)
        {
            List<Patient> result = patients
            .Where(patient => patient.AgeAtFirstVaccination >= lowestAge &&
             patient.AgeAtFirstVaccination <= highestAge)
            .ToList();

            return result;
        }
        /// <summary>
        /// Gets a list of all patients that has been vaccinated at a specific vaccination site
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static List<Patient> GetPatientsByVaccinationCentral(List<Patient> patients, int siteId)
        {
            List<Patient> result = patients
            .Where(patient => patient.Vaccinations.Any(s => s.VaccinationSiteId == siteId))
            .ToList();

            return result;
        }

        public static List<Patient> GetPatientsByDates(List<Patient> patients, DateTime startDate,DateTime endDate)
        {
            List<Patient> result = patients
            .Where(patient => patient.Vaccinations.Any(d => d.VaccinationDate >= startDate &&
            d.VaccinationDate <= endDate))
            .ToList();

            return result;
        }

        /// <summary>
        /// Sorts patients from parameters sent from javascript with linqquery, returns a list of filtered patients
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="patients"></param>
        /// <returns></returns>
        public static List<Patient> GetSortedPatients(FilterDto filter, List<Patient> patients)
        {
            var filteredPatients = patients
                .Where(p => string.IsNullOrEmpty(filter.DeSoCode) || p.DeSoCode == filter.DeSoCode)
                .Where(p => string.IsNullOrEmpty(filter.BatchNumber) || p.Vaccinations.Any(v => v.BatchNumber == filter.BatchNumber))
                .Where(p => string.IsNullOrEmpty(filter.Gender) || p.Gender == filter.Gender)
                .Where(p => filter.MinAge == 0 || p.AgeAtFirstVaccination >= filter.MinAge)
                .Where(p => filter.MaxAge == 0 || p.AgeAtFirstVaccination <= filter.MaxAge)
                .Where(p => filter.SiteId == 0 || p.Vaccinations.Any(v => v.VaccinationSiteId == filter.SiteId))
                .Where(p => filter.NumberOfDoses == 0 || p.Vaccinations.Any(v => v.DoseNumber == filter.NumberOfDoses))
                .Where(p => string.IsNullOrEmpty(filter.TypeOfVaccine) || p.Vaccinations.Any(v => v.VaccineName == filter.TypeOfVaccine))
                .Where(p => filter.StartDate == DateTime.MinValue || p.Vaccinations.Any(v => v.VaccinationDate >= filter.StartDate))
                .Where(p => filter.EndDate == DateTime.MinValue || p.Vaccinations.Any(v => v.VaccinationDate <= filter.EndDate)).ToList();
            return filteredPatients;
        }
        
        public static List<SelectListItem> GetDesoInformation(List<Patient> patients)
        {
            List<SelectListItem> uniqueDeSoCodesAndNames = patients
                .GroupBy(p => p.DeSoCode)
                .Select(g => new SelectListItem
                {
                    Value = g.Key, 
                    Text = g.First().DeSoName
                })
                .OrderBy(item => item.Text)
                .ToList();
            return uniqueDeSoCodesAndNames;
        }
        /// <summary>
        /// Sorts resident by specifik deSo and gender
        /// </summary>
        /// <param name="residents"></param>
        /// <param name="deSo"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static List<Resident> GetResidentsByGender(List<Resident> residents, string deSo, string gender)
        {
            List<Resident> sortedResidents = residents
            .Where(resident => resident.DeSoCode == deSo)
            .Where(resident => resident.Gender == gender)
            .ToList();
            return sortedResidents;
        }
      
        /// <summary>
        /// Method that calculates the total population in all desos and creates a list.
        /// </summary>
        /// <param name="residents"></param>
        /// <param name="deSo"></param>
        /// <returns></returns>
        public static List<Resident> GetResidentsByDeSo(List<Resident> residents, string deSo)
        {
            List<Resident> result = residents
            .Where(resident => resident.DeSoCode == deSo)
            .ToList();

            return result;
        }

        /// <summary>
        /// Method that creates a list of all deso codes.
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public static List<string> GetDesoList(List<Patient> patients)
        {
            List<string> desoList = patients.Select(d => d.DeSoCode).Distinct().ToList();
            return desoList;
        }

    }
}
