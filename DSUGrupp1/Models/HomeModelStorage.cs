using DSUGrupp1.Models.DTO;
using DSUGrupp1.Models.ViewModels;

namespace DSUGrupp1.Models
{
    public static class HomeModelStorage
    {
        private static HomeViewModel _viewModel;
        private static DisplayAgeStatisticsViewModel _ageStatistics;



        public static HomeViewModel ViewModel 
        {
            get 
            { 
                return _viewModel;
            } 
            set
            {
                _viewModel = value;
            }


        }

        public static DisplayAgeStatisticsViewModel AgeStatistics
        {
            get
            {
                return _ageStatistics;
            }
            set
            {
                _ageStatistics = value;
            }
        }


    }
}
