using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartForm
{
    public class SmartFormHost<TModel>
    {
        public static IHtmlHelper<TModel> Helper { get; set; }

        public SmartFormHost(IHtmlHelper<TModel> htmlHelper)
        {
            Helper = htmlHelper;
        }

        public DashboardBuilder<TModel> Dashboard(int GridRowCount, int GridColCount)
        {
            var dashboardBuilder = new DashboardBuilder<TModel>();
            dashboardBuilder.GridRowCount = GridRowCount;
            dashboardBuilder.GridColCount = GridColCount;
            return dashboardBuilder;
        }

        public FormBuilder<TModel> Form()
        {
            return new FormBuilder<TModel>();
        }
    }
}
