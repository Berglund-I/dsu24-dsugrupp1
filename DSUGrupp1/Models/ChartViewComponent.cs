using Microsoft.AspNetCore.Mvc;
using DSUGrupp1.Models.ViewModels;

namespace DSUGrupp1.Models
{
    public class ChartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(string id, ChartViewModel chartModel)
        {
            if (chartModel == null)
            {
                return Content("Chart data is not available.");
            }

            chartModel.Id = id;
            return View(chartModel);
        }
    }
}
