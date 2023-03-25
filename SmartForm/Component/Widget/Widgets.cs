
using Microsoft.AspNetCore.Html;

namespace SmartForm
{
    public abstract class Widget
    {
        public string Name { get; }
        public WidgetDataSource DataSource { get; set; }
        public string Title { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int RowStart { get; set; }
        public int RowEnd { get; set; }
        public int ColumnStart { get; set; }
        public int ColumnEnd { get; set; }
        public bool EnableDefaultErrorView { get; set; } = true;
        public string ErrorContent { get; set; } = "Error";
        public string ErrorHtmlString { get; set; }
        public string Id { get; set; } = "dashboard-widget-" + (new Random()).Next(10000, 99999);
        protected string ScriptString = "";
        protected Widget(string name = "")
        {
            Name = name;
            DataSource = new WidgetDataSource();
            ErrorHtmlString = $@"<div class=""dashboard-error-container dashboard-item-container dashboard-center-align-item""><p>{ErrorContent}</p></div>";
            ScriptString = @"<script>
                    function SkeletonShow($widget) {
                            $widget.find('.dashboard-skeleton-circle-holder').addClass('dashboard-skeleton-circle');
                            $widget.find('.dashboard-skeleton-line-holder').addClass('dashboard-skeleton-line');
                            $widget.find('.dashboard-skeleton-circle-holder').children().addClass('dashboard-opacity-0');
                            $widget.find('.dashboard-skeleton-line-holder').children().addClass('dashboard-opacity-0');
                    }
                    function SkeletonHide($widget) {
                            $widget.find('.dashboard-skeleton-circle-holder').children().removeClass('dashboard-opacity-0');
                            $widget.find('.dashboard-skeleton-line-holder').children().removeClass('dashboard-opacity-0');
                            $widget.find('.dashboard-skeleton-circle-holder').removeClass('dashboard-skeleton-circle');
                            $widget.find('.dashboard-skeleton-line-holder').removeClass('dashboard-skeleton-line');
                    }
                </script>";
        }

        public abstract string Build();
    }

    public class WidgetDataSource
    {
        public WidgetDataSource(string webApi = "", string method = "Get")
        {
            WebApi = webApi;
            Method = method;
        }

        public string WebApi { get; set; }
        public string Method { get; set; }
    }

    public class StatListData
    {
        public int Count { get; set; }
        public string Unit { get; set; } = "مورد";
        public string Title { get; set; }
        public string Color { get; set; }
        public string BackgroundColor { get; set; }
        public string Link { get; set; }
        public string OnClickFunction { get; set; }
    }

    public class DataStat : Widget
    {
        public DataStat(string name) : base(name)
        {

        }
        public class Info
        {
            public string ValueFieldName { get; set; }
            public string ValueField { get; set; }
            public string ColorFieldName { get; set; }
            public string ColorField { get; set; }
            public string BackgroundColorFieldName { get; set; }
            public string BackgroundColorField { get; set; }
        }
        private Info? InfoData { get; set; }
        public DataStat SetInfo(Action<Info> action)
        {
            InfoData = new Info();
            action(InfoData);
            return this;
        }


        private string ImageValue { get; set; }

        private string ImageFieldName { get; set; }

        public DataStat ImageField(string valueNameField)
        {
            ImageFieldName = valueNameField;

            return this;
        }
        public DataStat Image(string value)
        {
            ImageValue = value;

            return this;
        }

        private string DataValue { get; set; }

        private string DataFieldName { get; set; }

        public DataStat DataField(string valueFieldName)
        {
            DataFieldName = valueFieldName;
            return this;
        }

        public DataStat Data(string value)
        {
            DataValue = value;
            return this;
        }

        private string UnitValue { get; set; }

        private string UnitFieldName { get; set; }

        public DataStat UnitField(string valueFieldName)
        {
            UnitFieldName = valueFieldName;
            return this;
        }

        public DataStat Unit(string value)
        {
            UnitValue = value;
            return this;
        }

        public override string Build()
        {
            var htmlString = $@"<div class=""dashboard-widget"" Id=""{Id}"" style=""grid-column-start:{ColumnStart};grid-column-end:{ColumnEnd};grid-row-start:{RowStart};grid-row-end:{RowEnd};position:relative;"">
                                <div class=""dashboard-item-container dashboard-center-align-item"">
                                    <div class=""dashboard-skeleton-circle-holder dashboard-detail-image {(string.IsNullOrEmpty(ImageValue) ? "dashboard-display-none" : string.Empty)}""><img src=""{ImageValue}""></div>
                                    <div><p class=""dashboard-skeleton-line-holder dashboard-item-title"">{Title}</p>                                        
                                        <p class=""dashboard-skeleton-line-holder dashboard-item-content""><span class=""dashboard-item-content-text"">{DataValue}</span>
                                        {(!string.IsNullOrEmpty(UnitValue) ? $@"<span class=""dashboard-ActionSheetItem-Content-Unit"">{UnitValue}</span>" : string.Empty)}
                                        {(InfoData is not null ? ($@"<span class=""dashboard-skeleton-line-holder dashboard-item-threshold"" style=""{(!string.IsNullOrEmpty(InfoData?.ColorField) ? String.Format("'color:{0};'", InfoData.ColorField) : string.Empty)} {(!string.IsNullOrEmpty(InfoData?.BackgroundColorField) ? String.Format("'background:{0};'", InfoData.BackgroundColorField) : string.Empty)}"">{InfoData?.ValueField}</span>") : String.Empty)}            
                                        </p>
                                    </div>
                                </div>
                            </div>";

            //Check Other Properties
            ScriptString += string.IsNullOrEmpty(DataSource.WebApi) ? string.Empty :
                @"<script>
                   function LodaDashboardStatData($widget){
                       SkeletonShow($widget);
                        $.ajax({
                                    url: '" + DataSource.WebApi + @"',
                                    method: '" + DataSource.Method + @"',
                                    async: true,
                                    success: function (data) {
                                          var contentData = " + (string.IsNullOrEmpty(DataFieldName) ? "data " : "data." + DataFieldName) + @";
                                          if($.isNumeric(contentData)){
                                              contentData = parseInt(contentData).toLocaleString('en-us');
                                          }
                                          $widget.find('.dashboard-item-content .dashboard-item-content-text').text(contentData);
                                          if(" + string.IsNullOrEmpty(ImageFieldName).ToString().ToLower() + @") { $widget.find('.dashboard-detail-image').attr('src', " + (string.IsNullOrEmpty(ImageFieldName) ? String.Empty : "data." + ImageFieldName) + @");}
                                          $widget.find('.dashboard-ActionSheetItem-Content-Unit').text(" + (string.IsNullOrEmpty(UnitFieldName) ? String.Empty : "data." + UnitFieldName) + @");
                                          $widget.find('.dashboard-item-threshold').text(" + (string.IsNullOrEmpty(InfoData?.ValueFieldName) ? String.Empty : "data." + InfoData?.ValueFieldName) + @");
                                          if ($widget.find('.dashboard-item-threshold').text() == '') { $widget.find('.dashboard-item-threshold').addClass('dashboard-display-none'); }
                                          $widget.find('.dashboard-item-threshold').css('color', " + (InfoData is null || string.IsNullOrEmpty(InfoData?.ColorFieldName) ? String.Empty : "data." + InfoData.ColorFieldName) + @");
                                          $widget.find('.dashboard-item-threshold').css('background', " + (InfoData is null || string.IsNullOrEmpty(InfoData?.BackgroundColorFieldName) ? String.Empty : "data." + InfoData.BackgroundColorFieldName) + @");
                                          SkeletonHide($widget);
                                    },
                                    error: function (dataError) {   
                                        if(" + EnableDefaultErrorView.ToString().ToLower() + @"){
                                              $widget.append(" + string.Format("'{0}'", ErrorHtmlString) + @");
                                        }else{
                                              $widget.find('.dashboard-item-content .dashboard-item-content-text').text('-');
                                              $widget.find('.dashboard-item-threshold').addClass('dashboard-display-none');
                                        }
                                        console.log('error occurred: #" + Id + @"',dataError);
                                        SkeletonHide($widget);
                                    }
                               });
                    }
                    LodaDashboardStatData($('#" + Id + @"'));
                </script>";

            return htmlString + ScriptString;
        }
    }

    public class Chart : Widget
    {
        public List<ChartData> ChartDataList { get; set; }
        public string ChartId { get; set; } = "dashboard-chart-" + (new Random()).Next(10000, 99999);
        public Chart(string name) : base(name)
        {

        }

        public override string Build()
        {
            var htmlString = $@"<div class=""dashboard-widget"" Id=""{Id}"" style=""grid-column-start:{ColumnStart};grid-column-end:{ColumnEnd};grid-row-start:{RowStart};grid-row-end:{RowEnd};position:relative;"">
                                    <div class=""row dashboard-item-container"">
                                        <p class=""dashboard-skeleton-line-holder dashboard-item-title"">{Title}</p>
                                        <div class=""dashboard-chart-container"">
                                            <canvas id=""{ChartId}""></canvas>
                                        </div>
                                    </div>
                                </div>
                            <script src=""/Chart/Chart.js/Chart.js""></script>
                            <script src=""/Chart/Chart.js/chartjs-plugin-datalabels.js""></script>";

            htmlString += @"<script>
                                const ctx = document.getElementById('" + ChartId + @"');
                                new Chart(ctx, {
                                  type: 'doughnut',
                                  data: {
                                labels: [" + string.Join(",", ChartDataList.Select(x => string.Format("'{0}'", x.Label)).ToArray()) + @"], 
                                datasets: [{
                                          data: [" + string.Join(",", ChartDataList.Select(x => x.Data).ToArray()) + @"],
                                          backgroundColor: [" + string.Join(",", ChartDataList.Select(x => string.Format("'{0}'", x.Color)).ToArray()) + @"],
                                          hoverOffset: 4
                                        }]
                                      },
                              options: {
                                  responsive: true,
                                  maintainAspectRatio: false,
                                    legend: {
                                    display: true,
                                    position: 'right',
                                    align: 'start',
                                    rtl: true,
                                    labels:{
                                        fontSize: 14,
                                        fontFamily : 'vazirfd',
                                        boxWidth: 27
                                    }
                              },
                              tooltips: {
                                titleFontFamily: 'vazirfd',
                                bodyFontFamily: 'vazirfd'
                              },
                              plugins: {
                                datalabels: {
                                  formatter: (value, dnct1) => {
                                    return value + '%';
                                  },
                                  color: '#fff',
                                  font: {
                                    family : 'vazirfd',
                                      },
                                  textAlign : 'center'
                                }
                              }
                              },
                            plugins: [ChartDataLabels]
                                });
                        </script>";
            return htmlString;
        }
    }
    public class ChartData
    {
        public int Data { get; set; }
        public string Label { get; set; }
        public string Color { get; set; }
    }

    public class Notice : Widget
    {
        private string ContentValue { get; set; }

        private string ContentFieldName { get; set; }

        public Notice ContentField(string valueFieldName)
        {
            ContentFieldName = valueFieldName;
            return this;
        }

        public Notice Content(string value)
        {
            ContentValue = value;
            return this;
        }
        private string LabelValue { get; set; }

        private string LabelFieldName { get; set; }

        public Notice LabelField(string valueFieldName)
        {
            LabelFieldName = valueFieldName;
            return this;
        }

        public Notice Label(string value)
        {
            LabelValue = value;
            return this;
        }
        
        private string LinkTitleValue { get; set; } = "مشاهده";

        private string LinkTitleFieldName { get; set; }

        public Notice LinkTitleField(string valueFieldName)
        {
            LinkTitleFieldName = valueFieldName;
            return this;
        }

        public Notice LinkTitle(string value)
        {
            LinkTitleValue = value;
            return this;
        }
        private string LinkValue { get; set; }

        private string LinkFieldName { get; set; }

        public Notice LinkField(string valueFieldName)
        {
            LinkFieldName = valueFieldName;
            return this;
        }

        public Notice Link(string value)
        {
            LinkValue = value;
            return this;
        }

        private string LinkOnClickFunctionValue { get; set; }

        private string LinkOnClickFunctionFieldName { get; set; }

        public Notice LinkOnClickFunctionField(string valueFieldName)
        {
            LinkOnClickFunctionFieldName = valueFieldName;
            return this;
        }

        public Notice LinkOnClickFunction(string value)
        {
            LinkOnClickFunctionValue = value;
            return this;
        }
        private bool CloseableValue { get; set; } = true;

        private string CloseableFieldName { get; set; }

        public Notice CloseableField(string valueFieldName)
        {
            CloseableFieldName = valueFieldName;
            return this;
        }

        public Notice Closeable(bool value)
        {
            CloseableValue = value;
            return this;
        }
        
        private string MainColorValue { get; set; }

        private string MainColorFieldName { get; set; }

        public Notice MainColorField(string valueFieldName)
        {
            MainColorFieldName = valueFieldName;
            return this;
        }

        public Notice MainColor(string value)
        {
            MainColorValue = value;
            return this;
        }
        
        private string BackgroundValue { get; set; }

        private string BackgroundFieldName { get; set; }

        public Notice BackgroundField(string valueFieldName)
        {
            BackgroundFieldName = valueFieldName;
            return this;
        }

        public Notice Background(string value)
        {
            BackgroundValue = value;
            return this;
        }

        public Notice(string name) : base(name)
        {
        }
        public override string Build()
        {
            var backgroundStyle = "";
            if (!string.IsNullOrEmpty(BackgroundValue) && !string.IsNullOrEmpty(MainColorValue))
            {
                backgroundStyle = $@"style=""background:{BackgroundValue}; border-color:{MainColorValue};""";
            }
            else if (!string.IsNullOrEmpty(BackgroundValue))
            {
                backgroundStyle = $@"style=""background:{BackgroundValue};""";
            }
            else if (!string.IsNullOrEmpty(MainColorValue))
            {
                backgroundStyle = $@"style=""border-color:{MainColorValue};""";
            }

            var linkFunction = "";
            if (!string.IsNullOrEmpty(LinkOnClickFunctionValue))
            {
                linkFunction = $@"<a onclick=""{LinkOnClickFunctionValue}"">{LinkTitleValue}</a>";
            }else if (!string.IsNullOrEmpty(LinkValue))
            {
                linkFunction = $@"<a target=""_blank"" href=""{LinkValue}"">{LinkTitleValue}</a>";
            }

            var htmlString = $@"<div class=""dashboard-widget"" Id=""{Id}"" style=""grid-column-start:{ColumnStart};grid-column-end:{ColumnEnd};grid-row-start:{RowStart};grid-row-end:{RowEnd};position:relative;"">
                                    <div class=""dashboard-alert-container dashboard-center-align-item"" {backgroundStyle}>
                                        <div class=""dashboard-alert-content"">
                                            <div class=""dashboard-skeleton-line-holder dashboard-alert-title"">
                                                <label {(string.IsNullOrEmpty(MainColorValue) ? string.Empty : $@"style=""color:{MainColorValue}""")}>{LabelValue}:</label>
                                                <span>{Title}</span>
                                            </div>
                                            <p class=""dashboard-skeleton-line-holder dashboard-alert-detail"">{ContentValue}</p>
                                        </div>
                                        <div class=""dashboard-alert-action"">
                                            <button {(CloseableValue ? string.Empty : "class='dashboard-display-none'")} onClick=""$('#{Id}').slideUp();""> ×</button>
                                            {linkFunction}                                            
                                        </div>
                                    </div>
                                </div>";
            ScriptString += string.IsNullOrEmpty(DataSource.WebApi) ? string.Empty :
               @"<script>
                   function LoadDashboardNoticeData(){
                       var $widget = $('#" + Id + @"');
                       SkeletonShow($widget);
                        $.ajax({
                                    url: '" + DataSource.WebApi + @"',
                                    method: '" + DataSource.Method + @"',
                                    async: true,
                                    success: function (data) {
                                          if (!data."+ CloseableFieldName + @"){ $widget.find('.dashboard-alert-action button').addClass('dashboard-display-none')  }
                                          else { $widget.find('.dashboard-alert-action button').removeClass('dashboard-display-none')  }
                                          if (data." + BackgroundFieldName + @" != null && data." + BackgroundFieldName + @" != '' ){ $widget.find('.dashboard-alert-container').css('background', data."+ BackgroundFieldName + @")  }
                                          $widget.find('.dashboard-alert-detail').text(data." + ContentFieldName + @");
                                          $widget.find('.dashboard-alert-action a').prop('href', data." + LinkFieldName + @");                                      
                                          $widget.find('.dashboard-alert-action a').text('href', data." + LinkTitleFieldName + @");                                      
                                          SkeletonHide($widget);
                                    },
                                    error: function (dataError) {        
                                        if(" + EnableDefaultErrorView.ToString().ToLower() + @"){
                                            $('#" + Id + @"').append(" + string.Format("'{0}'", ErrorHtmlString) + @");
                                        }
                                        console.log('error occurred: #" + Id + @"',dataError);
                                        SkeletonHide($widget);
                                    }
                               });
                    }
                    LoadDashboardNoticeData();
                </script>";

            return htmlString + ScriptString;
        }
    }

    public class StatList : Widget
    {
        private List<StatListData> StatItemListValue { get; set; }

        private string StatItemListFieldName { get; set; }

        public StatList StatItemListField(string valueFieldName)
        {
            StatItemListFieldName = valueFieldName;
            return this;
        }

        public StatList StatItemList(List<StatListData> value)
        {
            StatItemListValue = value;
            return this;
        }

        public StatList(string name) : base(name)
        {
        }
        public override string Build()
        {
            var statItemHtmlString = "";
            if (StatItemListValue != null)
            {
                StatItemListValue.ForEach(Item =>
                {
                    statItemHtmlString += @$"<li><div class=""dashboard-stat-item""><div class=""dashboard-stat-color""></div><div class=""dashboard-stat-data-container"">
                               <p class=""dashboard-stat-title"">{Item.Title}</p><span class=""dashboard-stat-unit-count"">{Item.Count}<span>{Item.Unit}</span></span><img src=""/Widgets/Images/next-arrow.svg"" alt=""arrow""></div></div></li>";
                }
                );

            }

            var htmlString = $@"<div class=""dashboard-widget"" Id=""{Id}"" style=""grid-column-start:{ColumnStart};grid-column-end:{ColumnEnd};grid-row-start:{RowStart};grid-row-end:{RowEnd};position:relative;"">
                                <div class=""dashboard-item-container"">
                                    <div class=""dashboard-item-header-container"">
                                    <p class=""dashboard-skeleton-line-holder dashboard-item-title"">{Title}</p> 
                                    <div class=""dashboard-header-action-buttons"">
                                    <button onclick=""LodaDashboardStatListData($('#{ Id }'), '{DataSource.WebApi}')""><img src=""/Widgets/images/refresh.svg""></button>                                   
                                    </div></div>
                                    <ul class=""dashboard-skeleton-line-holder dashboard-stat-list"">
                                        {statItemHtmlString}
                                    </ul></div></div>";

            ScriptString += string.IsNullOrEmpty(DataSource.WebApi) ? string.Empty :
                @"<script>
                   function LodaDashboardStatListData($widget, url){
                       SkeletonShow($widget);
                        $.ajax({
                                    url: url,
                                    method: '" + DataSource.Method + @"',
                                    async: true,
                                    success: function (data) {
                                          $widget.find('.dashboard-stat-list').empty();
                                          var result = " + (string.IsNullOrEmpty(StatItemListFieldName) ? "data " : "data." + StatItemListFieldName) + @";
                                          $.each(result, function(index, item) {
                                            var listItem = '<li><a class=""dashboard-stat-item"" ' + (item.OnClickFunction != null || item.OnClickFunction != '' ? ""onclick = "" + item.OnClickFunction  : ""href = '"" + item.Link) + '><div class=""'+ item.Color +' '+ item.BackgroundColor +' dashboard-stat-color""></div><div class=""dashboard-stat-data-container"">' +
                                                           '<p class=""dashboard-stat-title"">' + item.Title + '</p><span class=""dashboard-stat-unit-count"">' + item.Count + '<span>' + item.Unit + '</span></span><img src=""/Widgets/Images/next-arrow.svg"" alt=""arrow""></div></a></li>';
                                            $widget.find('.dashboard-stat-list').append(listItem);
                                           });
                                          SkeletonHide($widget);
                                    },
                                    error: function (dataError) {        
                                        if(" + EnableDefaultErrorView.ToString().ToLower() + @"){
                                            $widget.append(" + string.Format("'{0}'", ErrorHtmlString) + @");
                                        }
                                        console.log('error occurred: #" + Id + @"', dataError);
                                        SkeletonHide($widget);
                                    }
                               });
                    }
                    LodaDashboardStatListData($('#" + Id + @"'),'" + DataSource.WebApi + @"');
                </script>";

            return htmlString + ScriptString;
        }
    }
    public class Slider : Widget
    {
        public class SliderItem
        {
            public string Image { get; set; }
            public string Title { get; set; }
            public string Date { get; set; }
            public string Content { get; set; }
            public string Link { get; set; }
            public string LinkTitle { get; set; }
        }
        private List<SliderItem> SlidesListValue { get; set; }

        private string SlidesListFieldName { get; set; }

        public Slider SlidesListField(string valueFieldName)
        {
            SlidesListFieldName = valueFieldName;
            return this;
        }

        public Slider SlidesList(List<SliderItem> value)
        {
            SlidesListValue = value;
            return this;
        }
        public Slider(string name) : base(name)
        {
        }
        public override string Build()
        {
            var slidesItem = "";
            if (SlidesListValue != null)
            {
                SlidesListValue.ForEach(Item =>
                {
                    slidesItem += @$"<div class=""item dashboard-owl-carousel-item-container"">
                                    <div class=""dashboard-news-item-container"">
                                    <img class=""dashboard-news-item-img"" src=""{Item.Image}"">
                                    <div>
                                    <p class=""dashboard-news-item-title"">{Item.Title}</p><span class=""dashboard-news-item-date-time-container""><span><img class=""dashboard-icon"" src=""/Widgets/Images/time-icon.svg""></span><span>{Item.Date}</span></span>
                                    <p class=""dashboard-news-item-content"">{Item.Content}</p>
                                    <a href=""{Item.Link}"" target=""_blank"">{Item.LinkTitle}</a>
                                    </div>
                                    </div>
                                    </div>
                                </div>";
                }
                );

            }

            var htmlString = $@"<div class=""dashboard-widget"" Id=""{Id}"" style=""grid-column-start:{ColumnStart};grid-column-end:{ColumnEnd};grid-row-start:{RowStart};grid-row-end:{RowEnd};position:relative;"">
                                <div class=""dashboard-item-container"">
                                   <div class=""dashboard-skeleton-line-holder dashboard-item-header-container""><p class=""dashboard-skeleton-line-holder dashboard-item-title"">{Title}</p>
                                    <div class=""dashboard-header-action-buttons"">
                                    <button onclick=""$('#{Id}').find('.owl-carousel').trigger('prev.owl.carousel');""><img src=""/Widgets/images/prev-arrow.svg""></button>
                                    <button onclick=""$('#{Id}').find('.owl-carousel').trigger('next.owl.carousel');""><img src=""/Widgets/images/next-arrow.svg""></button>
                                    </div></div> 
                                    <div class=""dashboard-skeleton-line-holder owl-carousel owl-theme"">{slidesItem}</div></div></div>";
            ScriptString += @"<link rel=""stylesheet"" href=""/Widgets/owl.carousel.min.css""><link rel=""stylesheet"" href=""/Widgets/owl.theme.default.min.css""><script src=""/Widgets/owl.carousel.min.js""></script>";
            ScriptString += @"<script>
                                $(document).ready(function(){
                                 function LodaDashboardSlidesListData($widget){
                                   SkeletonShow($widget);
                                    $.ajax({
                                                url: '" + DataSource.WebApi + @"',
                                                method: '" + DataSource.Method + @"',
                                                async: false,
                                                success: function (data) {
                                                      var result = " + (string.IsNullOrEmpty(SlidesListFieldName) ? "data " : "data." + SlidesListFieldName) + @";
                                                      $.each(result, function(index, item) {
                                                        var listItem = '<div class=""item dashboard-owl-carousel-item-container""><div class=""dashboard-news-item-container"">'+
                                                                        '<img class=""dashboard-news-item-img"" src=""'+ item.Image +'"">' + 
                                                                        '<div><p class=""dashboard-news-item-title"">'+ item.Title +'</p><span class=""dashboard-news-item-date-time-container""><span><img class=""dashboard-icon"" src=""/Widgets/Images/time-icon.svg""></span><span>'+ item.Date +'</span></span>'+
                                                                        '<p class=""dashboard-news-item-content"">'+ item.Content +'</p><a href=""'+ item.Link +'"" target=""_blank"">'+ item.LinkTitle +'</a></div></div></div></div > ';
                                                        $widget.find('.owl-carousel').append(listItem);
                                                       });
                                                      SkeletonHide($widget);
                                                },
                                                error: function (dataError) {        
                                                    if(" + EnableDefaultErrorView.ToString().ToLower() + @"){
                                                        $widget.append(" + string.Format("'{0}'", ErrorHtmlString) + @");
                                                    }
                                                    console.log('error occurred: #" + Id + @"', dataError);
                                                    SkeletonHide($widget);
                                                }
                                           });
                                }
                            LodaDashboardSlidesListData($('#" + Id + @"'));

                            $('#" + Id + @"').find('.owl-carousel').owlCarousel({
                                rtl:true,
                                nav:false,
                                dots:true,
                                items:1,
                                loop:true,
                                autoplay:true,
                                autoplayTimeout:5000,
                                autoplayHoverPause:true
                                });
                            });
                            </script>";
            return  htmlString + ScriptString;
        }
    }

    public class Banner : Widget
    {
        private string ImageValue { get; set; }

        private string ImageFieldName { get; set; }

        public Banner ImageField(string valueFieldName)
        {
            ImageFieldName = valueFieldName;
            return this;
        }

        public Banner Image(string value)
        {
            ImageValue = value;
            return this;
        }

        private string SubTitleValue { get; set; }

        private string SubTitleFieldName { get; set; }

        public Banner SubTitleField(string valueFieldName)
        {
            SubTitleFieldName = valueFieldName;
            return this;
        }

        public Banner SubTitle(string value)
        {
            SubTitleValue = value;
            return this;
        }

        private string LinkTitleValue { get; set; } = "مشاهده";

        private string LinkTitleFieldName { get; set; }

        public Banner LinkTitleField(string valueFieldName)
        {
            LinkTitleFieldName = valueFieldName;
            return this;
        }

        public Banner LinkTitle(string value)
        {
            LinkTitleValue = value;
            return this;
        }

        private string LinkValue { get; set; }

        private string LinkFieldName { get; set; }

        public Banner LinkField(string valueFieldName)
        {
            LinkFieldName = valueFieldName;
            return this;
        }

        public Banner Link(string value)
        {
            LinkValue = value;
            return this;
        }
        
        private string OnClickFunctionValue { get; set; }

        private string OnClickFunctionFieldName { get; set; }

        public Banner OnClickFunctionField(string valueFieldName)
        {
            OnClickFunctionFieldName = valueFieldName;
            return this;
        }

        public Banner OnClickFunction(string value)
        {
            OnClickFunctionValue = value;
            return this;
        }

        private string MainColorValue { get; set; }

        private string MainColorFieldName { get; set; }

        public Banner MainColorField(string valueFieldName)
        {
            MainColorFieldName = valueFieldName;
            return this;
        }

        public Banner MainColor(string value)
        {
            MainColorValue = value;
            return this;
        }

        private string BackgroundValue { get; set; }

        private string BackgroundFieldName { get; set; }

        public Banner BackgroundField(string valueFieldName)
        {
            BackgroundFieldName = valueFieldName;
            return this;
        }

        public Banner Background(string value)
        {
            BackgroundValue = value;
            return this;
        }

        public Banner(string name) : base(name)
        {
        }
        public override string Build()
        {

            var htmlString = $@"<div class=""dashboard-widget"" Id=""{Id}"" style=""grid-column-start:{ColumnStart};grid-column-end:{ColumnEnd};grid-row-start:{RowStart};grid-row-end:{RowEnd};position:relative;"">
                                <div class=""dashboard-banner-container"" {(!string.IsNullOrEmpty(BackgroundValue) || !string.IsNullOrEmpty(MainColorValue) ? $@"style=""{(!string.IsNullOrEmpty(BackgroundValue)? "background:" + BackgroundValue + ";" : "")} {(!string.IsNullOrEmpty(MainColorValue) ? "color:" + MainColorValue + ";" : "")}""" : string.Empty)}> 
                                <div class=""dashboard-banner-detail"">
                                <p class=""dashboard-banner-title"">{Title}</p>
                                <p class=""dashboard-banner-subtitle"">{SubTitleValue}</p>
                                <div class=""dashboard-banner-button"">
                                <a {(string.IsNullOrEmpty(LinkValue) ? string.Empty: $@"href=""{LinkValue}""")} {(string.IsNullOrEmpty(OnClickFunctionValue) ? string.Empty: $@"onclick=""{OnClickFunctionValue}""")}>
                                       {LinkTitleValue}<img class=""icon"" src=""/images/svg/left-arrow.svg""/></a>
                                </div>
                                </div>
                                <img src=""{ImageValue}""/>
                                </div>
                                </div>";
            ScriptString += string.IsNullOrEmpty(DataSource.WebApi) ? string.Empty :
               @"<script>
                   function LoadDashboardBannerData(){
                       var $widget = $('#" + Id + @"');
                       SkeletonShow($widget);
                        $.ajax({
                                    url: '" + DataSource.WebApi + @"',
                                    method: '" + DataSource.Method + @"',
                                    async: true,
                                    success: function (data) {
                                                                           
                                          SkeletonHide($widget);
                                    },
                                    error: function (dataError) {        
                                        if(" + EnableDefaultErrorView.ToString().ToLower() + @"){
                                            $('#" + Id + @"').append(" + string.Format("'{0}'", ErrorHtmlString) + @");
                                        }
                                        console.log('error occurred: #" + Id + @"',dataError);
                                        SkeletonHide($widget);
                                    }
                               });
                    }
                    LoadDashboardBannerData();
                </script>";

            return htmlString + ScriptString;
        }
    }
}
