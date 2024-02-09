﻿using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models
{
    public class Patient
    {
        public string DeSoCode { get; set; }
        public string DeSoName { get; set; }
        public string Gender { get; set; }
        public int YearOfBirth { get; set; }
        public List<Vaccination> Vaccinations { get; set; }

        public Patient(PatientInformationDto patientData, DoseTypeDto doseData) 
        { 
            Gender = patientData.Gender;
            YearOfBirth = int.Parse(patientData.YearOfBirth);
            Vaccinations = new List<Vaccination>();

            foreach (var v in patientData.Vaccinations)
            {
                BatchDto batch = GetVaccineName(doseData, v.BatchNumber);
                var vaccination = new Vaccination()
                {
                    VaccinationDate = DateTime.Parse(v.DateOfVaccination),
                    BatchNumber = v.BatchNumber,
                    VaccineName = batch.VaccineName,
                    VaccineManufacturer = batch.Manufacturer,
                    DoseNumber = v.DoseNumber,
                    VaccinationSiteId = v.VaccinationCentral.SiteId,
                    VaccinationSiteName = v.VaccinationCentral.SiteName,
                };
                Vaccinations.Add(vaccination);
            }
        }
    
        public BatchDto GetVaccineName(DoseTypeDto doseData, string batchNumber)
        {
            var batch = doseData.Batches.FirstOrDefault(b => b.BatchNumber == batchNumber);
            return batch;
        }
    }
}
