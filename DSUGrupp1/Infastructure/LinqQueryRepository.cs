﻿using DSUGrupp1.Models;
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
        /// <summary>
        /// Gets a list of all patients within the age span
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="lowestAge"></param>
        /// <param name="highestAge"></param>
        /// <returns></returns>
        public static List<Patient> GetPatientsByAge(List<Patient> patients, int lowestAge, int highestAge)
        {
            //DateTime dateTime = DateTime.Now;
            //int currentYear = dateTime.Year;

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
            //patient.Vaccinations.dateOfVaccination bör vara DateTime, inte string
            //DateTime dateOne = DateTime.Parse("2020-09-14");
            //DateTime dateTwo = DateTime.Parse("2021-09-14");

            List<Patient> result = patients
            .Where(patient => patient.Vaccinations.Any(d => d.VaccinationDate >= startDate &&
            d.VaccinationDate <= endDate))
            .ToList();

            return result;
        }
        public static List<Patient> GetSortedPatients(FilterDto filter, List<Patient> patients)
        {
            var filteredPatients = patients
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
    }
}
