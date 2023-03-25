
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartForm
{
    public class DashboardBuilder<TModel> : SmartFormBuilder<TModel>
    {
        public List<Widget> Widgets { get; set; }
        public int GridRowCount { get; set; }
        public int GridColCount { get; set; }
        private string _Name { get; set; } = "smart-dashboard-" + (new Random()).Next(10000, 99999);

        public DashboardBuilder()
        {
            Widgets = new List<Widget>();
        }
        public DashboardBuilder<TModel> Name(string name)
        {
            _Name = name;
            return this;
        }

        public DashboardBuilder<TModel> AddWidgets(List<Widget> widgets)
        {
            Widgets.AddRange(widgets);

            return this;
        }

        public override HtmlString Build()
        {
            var content = "";
            bool[,] array = new bool[GridRowCount, GridColCount];
            foreach (var widget in Widgets.Select((x, i) => new { Value = x, Index = i }))
            {
                DashboardGrid gridManager = new DashboardGrid();
                var r = gridManager.FillGrid(widget.Value, GridRowCount, GridColCount, array);
                widget.Value.RowStart = r.Item1.First() + 1;
                widget.Value.RowEnd = r.Item1.First() == r.Item1.Last() ? widget.Value.RowStart + 1 : r.Item1.Last() + 2;
                widget.Value.ColumnStart = r.Item2.First() + 1;
                widget.Value.ColumnEnd = r.Item2.First() == r.Item2.Last() ? widget.Value.ColumnStart + 1 : r.Item2.Last() + 2;
                content += widget.Value.Build();
            }
            var html = $@"<div class=""grid-container"" Id=""{_Name}"">{content}</div>";
            html += $@"
                    <script>
                        $(':root').css('--dashboard-grid-column-count',{GridColCount})
                        $(':root').css('--dashboard-grid-row-count',{GridRowCount})
                    </script>";

            return new HtmlString(html);
        }
    }

}
