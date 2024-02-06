using DSUGrupp1.Models.ViewModels;

namespace DSUGrupp1.Models
{
    public static class HomeModelStorage
    {
        private static HomeViewModel _viewModel;

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
        
        
    }
}
