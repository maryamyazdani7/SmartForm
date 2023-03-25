
namespace SmartForm
{
    public abstract class Component
    {
        public string Name { get; }
        public FormsDataSource DataSource { get; set; }
        public string ComponentType { get; protected set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool Required { get; set; } = false;
        public bool EnableDefaultErrorView { get; set; } = true;
        public string ErrorContent { get; set; } = "Error";
        public string ErrorHtmlString { get; set; }

        private string _Id;

        public string Id
        {
            get { return _Id; }
            set { _Id = (string.IsNullOrEmpty(value) ? "smart-forms-component-" + (new Random()).Next(10000, 99999) : value); }
        }
        //public string Id { get; set; } = "smart-forms-component-" + (new Random()).Next(10000, 99999);
        protected string ScriptString = "";
        protected Component(string name = "")
        {
            Name = name;
            DataSource = new FormsDataSource();
            ErrorHtmlString = $@"<div class=""dashboard-error-container dashboard-item-container dashboard-center-align-item""><p>{ErrorContent}</p></div>";
            //ScriptString = @"<script>
            //        function SkeletonShow($Forms) {
            //                $Forms.find('.dashboard-skeleton-circle-holder').addClass('dashboard-skeleton-circle');
            //                $Forms.find('.dashboard-skeleton-line-holder').addClass('dashboard-skeleton-line');
            //                $Forms.find('.dashboard-skeleton-circle-holder').children().addClass('dashboard-opacity-0');
            //                $Forms.find('.dashboard-skeleton-line-holder').children().addClass('dashboard-opacity-0');
            //        }
            //        function SkeletonHide($Forms) {
            //                $Forms.find('.dashboard-skeleton-circle-holder').children().removeClass('dashboard-opacity-0');
            //                $Forms.find('.dashboard-skeleton-line-holder').children().removeClass('dashboard-opacity-0');
            //                $Forms.find('.dashboard-skeleton-circle-holder').removeClass('dashboard-skeleton-circle');
            //                $Forms.find('.dashboard-skeleton-line-holder').removeClass('dashboard-skeleton-line');
            //        }
            //    </script>";
        }

        public abstract string Build();
    }

    public class FormsDataSource
    {
        public FormsDataSource(string webApi = "", string method = "Get")
        {
            WebApi = webApi;
            Method = method;
        }

        public string WebApi { get; set; }
        public string Method { get; set; }
    }
    public class MultiChoiceItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class MultiChoice : Component
    {
        public MultiChoice(string name = "") : base(name)
        {
            ComponentType = "MultiChoice";

        }
        private string _GroupName; 

        public string GroupName   
        {
            get { return _GroupName; }   
            set { _GroupName = (string.IsNullOrEmpty(value) ? "smart-forms-choice-" + (new Random()).Next(10000, 99999): value); }  
        }
      //  public string GroupName { get; set; } = "smart-forms-choice-" + (new Random()).Next(10000, 99999);
        public bool MultiSelectEnable { get; set; } = false;
        public bool DisplayMultiChoiceInTwoColumns { get; set; } = false;
        private List<MultiChoiceItem> ItemsList = new List<MultiChoiceItem>();

        private string MultiChoiceItemsFieldName { get; set; }

        public MultiChoice MultiChoiceItemsField(string valueNameField)
        {
            MultiChoiceItemsFieldName = valueNameField;

            return this;
        }
        public MultiChoice Items(List<MultiChoiceItem> valueList)
        {
            ItemsList = valueList;

            return this;
        }

        public override string Build()
        {
            var inputItems = "";
            ItemsList.ForEach(item =>
            {
                inputItems += $@"<div {(DisplayMultiChoiceInTwoColumns ? "class=\"smart-form-multi-choice-item-col-2\"" : string.Empty)}><label class=""smart-form-multi-choice-item""><input type=""checkbox"" class=""{ (MultiSelectEnable ? string.Empty : "input-single-select")}"" value=""{ item.Value}"" name=""{GroupName}"" onclick=""{(MultiSelectEnable ? "SmartForm.MultiSelectFunction(this, $('#" + Id + "'))" : "SmartForm.SingleSelectFunction(this, $('#" + Id + "'))")}""/>{item.Text}<span class=""smart-form-multi-choice-checkbox-check-mark"">✓</span></label></div>";
            });
            var htmlString = $@"<div class=""smart-form-component"" Id=""{Id}"" data-required=""{Required.ToString().ToLower()}""  data-Value='{{ ""Required"": ""{Required.ToString().ToLower()}"", ""Id"": ""{Id}"", ""Value"": """" }}'><p class=""smart-form-item-title"">{(Required ? "<span class='smart-form-item-required'>*</span>" : string.Empty)}{Title}</p><p class=""smart-form-item-sub-title"">{SubTitle}</p><div {(DisplayMultiChoiceInTwoColumns ? "class=\"smart-form-multi-choice-item-row\"" : string.Empty)}>{inputItems}</div></div>";

            ScriptString += string.IsNullOrEmpty(DataSource.WebApi) ? string.Empty :
                @"<script>;
                </script>";

            return htmlString + ScriptString;
        }
    }
    public class TextAreaInput : Component
    {
        public TextAreaInput(string name = "") : base(name)
        {
            ComponentType = "TextAreaInput";
        }
        private string TextAreaTitleFieldName { get; set; }
        private string TextAreaTitleFieldValue { get; set; }

        public TextAreaInput TextAreaTitleField(string valueNameField)
        {
            TextAreaTitleFieldName = valueNameField;

            return this;
        }
        public TextAreaInput TextAreaTitle(string value)
        {
            TextAreaTitleFieldValue = value;

            return this;
        }
        private string TextAreaRowFieldName { get; set; }
        private int TextAreaRowFieldValue { get; set; } = 4;

        public TextAreaInput TextAreaRowField(string valueNameField)
        {
            TextAreaRowFieldName = valueNameField;

            return this;
        }
        public TextAreaInput TextAreaRow(int value)
        {
            TextAreaRowFieldValue = value;

            return this;
        }
        public override string Build()
        {
            var htmlString = $@"<div class=""smart-form-component"" Id=""{Id}"" data-required=""{Required.ToString().ToLower()}""  data-Value='{{ ""Required"": ""{Required.ToString().ToLower()}"", ""Id"": ""{Id}"", ""Value"": """" }}'><p class=""smart-form-item-title"">{(Required ? "<span class='smart-form-item-required'>*</span>" : string.Empty)}{Title}</p><p class=""smart-form-item-sub-title"">{SubTitle}</p><div>
                                <p class=""smart-form-textarea-title"">{TextAreaTitleFieldValue}</p>
                                  <textarea class=""smart-form-textarea"" rows=""{TextAreaRowFieldValue}"" oninput=""SmartForm.TextAreaChangedFunction($(this))""></textarea>
                                                                </div></div>";

            ScriptString += string.IsNullOrEmpty(DataSource.WebApi) ? string.Empty :
                @"<script>;
                </script>";

            return htmlString + ScriptString;
        }
    }
    public class RatingInput : Component
    {
        public class RateItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
        public RatingInput(string name = "") : base(name)
        {
            ComponentType = "RatingInput";
        }
        private string RateMaxTitleFieldName { get; set; }
        private string RateMaxTitleFieldValue { get; set; } = "رضایت کامل";

        public RatingInput RateMaxTitleField(string valueNameField)
        {
            RateMaxTitleFieldName = valueNameField;

            return this;
        }
        public RatingInput RateMaxTitle(string value)
        {
            RateMaxTitleFieldValue = value;

            return this;
        }
        private string RateMinTitleFieldName { get; set; }
        private string RateMinTitleFieldValue { get; set; } = "رضایت ندارم";

        public RatingInput RateMinTitleField(string valueNameField)
        {
            RateMinTitleFieldName = valueNameField;

            return this;
        }
        public RatingInput RateMinTitle(string value)
        {
            RateMinTitleFieldValue = value;

            return this;
        }

        private string _GroupName;

        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = (string.IsNullOrEmpty(value) ? "smart-forms-rate-" + (new Random()).Next(10000, 99999) : value); }
        }
        // public string GroupName { get; set; } = "smart-forms-rate-" + (new Random()).Next(10000, 99999);
        List<RateItem> ItemsList = new List<RateItem>();

        private string RatingItemsFieldName { get; set; }

        public RatingInput RatingItemsField(string valueNameField)
        {
            RatingItemsFieldName = valueNameField;

            return this;
        }
        public RatingInput Items(List<RateItem> valueList)
        {
            ItemsList = valueList;

            return this;
        }
        public override string Build()
        {
            var inputItems = "";
            foreach (var (item, index) in ItemsList.Select((v, i) => (v, i)))
            {
                inputItems = string.Concat($@"<input type=""radio"" id=""{(GroupName + "-" + index)}"" name=""{GroupName}"" value=""{item.Value}"" onclick=""SmartForm.RatingFunction($(this))""/><label for=""{(GroupName + "-" + index)}"">{item.Text}</label>", inputItems);
            };

            var htmlString = $@"<div class=""smart-form-component"" Id=""{Id}"" data-required=""{Required.ToString().ToLower()}"" data-Value='{{ ""Required"": ""{Required.ToString().ToLower()}"", ""Id"": ""{Id}"", ""Value"": """" }}'><p class=""smart-form-item-title"">{(Required ? "<span class='smart-form-item-required'>*</span>" : string.Empty)}{Title}</p><p class=""smart-form-item-sub-title"">{SubTitle}</p>
                                <div class=""smart-form-rate-container""><div class=""smart-form-rate"">{inputItems}</div><div class=""min-rate-title""><span>|</span><p>{RateMinTitleFieldValue}</p></div><div class=""max-rate-title""><span>|</span><p>{RateMaxTitleFieldValue}</p></div></div></div>";

            ScriptString += string.IsNullOrEmpty(DataSource.WebApi) ? string.Empty :
                @"<script>;
                </script>";

            return htmlString + ScriptString;
        }
    }
    public class BannerInput : Component
    {
        public BannerInput(string name = "") : base(name)
        {
            ComponentType = "Banner";
        }
        private string ImageFieldName { get; set; }
        private string ImageFieldValue { get; set; }

        public BannerInput ImageField(string valueNameField)
        {
            ImageFieldName = valueNameField;

            return this;
        }
        public BannerInput Image(string value)
        {
            ImageFieldValue = value;

            return this;
        }
        public override string Build()
        {

            var htmlString = $@"<div class=""smart-form-component-none-value smart-form-center-items-containe smart-form-banner-container"" Id=""{Id}"">
                                <p class=""smart-form-item-title smart-form-banner-title"">{Title}</p>
                                <p class=""smart-form-item-sub-title smart-form-banner-subtitle"">{SubTitle}</p>
                                {(string.IsNullOrEmpty(ImageFieldValue) ? string.Empty : $@"<img class=""smart-form-image"" src=""{ImageFieldValue}""/>")}                                
                                </div>";


            ScriptString += string.IsNullOrEmpty(DataSource.WebApi) ? string.Empty :
                @"<script>;
                </script>";

            return htmlString + ScriptString;
        }
    }
    public class Section
    {
        public Section()
        {
            Steps = new List<Step>();
        }

        private string _SectionId;

        public string SectionId
        {
            get { return _SectionId; }
            set { _SectionId = (string.IsNullOrEmpty(value) ? "smart-forms-progress-steps-section" + (new Random()).Next(10000, 99999) : value); }
        }
       // public string StepId { get; set; } = "smart-forms-progress-steps-step" + (new Random()).Next(10000, 99999);
        public string Title { get; set; }
        public List<Step> Steps { get; set; }
    }
    public class Step
    {
        public Step()
        {
            StepContent = new List<Component>();
        }

        private string _StepId;

        public string StepId
        {
            get { return _StepId; }
            set {
                _StepId = (string.IsNullOrEmpty(value) ? "smart-forms-progress-steps-step" + (new Random()).Next(10000, 99999) : value); 
            }
        }
       // public string StepId { get; set; } = "smart-forms-progress-steps-step" + (new Random()).Next(10000, 99999);
        public string Title { get; set; }
        public bool IsIntro { get; set; } = false;
        public List<Component> StepContent { get; set; }
    }
    public class ProgressStep : Component
    {
        public ProgressStep()
        {
            SectionsList = new List<Section>();
        }

        public bool ProgressStepNavEnable { get; set; } = true;
        public string StepUnit { get; set; }
        public string NextStepButtonTitle { get; set; }
        public string PrevStepButtonTitle { get; set; }
        public string SubmitButtonTitle { get; set; }
        public string IntroButtonTitle { get; set; }
        public string SubmitButtonFunction { get; set; }

        //  private List<Step> StepsList = new List<Step>();
        private List<Section> SectionsList = new List<Section>();

        private string SectionsFieldName { get; set; }

        public ProgressStep SectionsField(string valueNameField)
        {
            SectionsFieldName = valueNameField;

            return this;
        }
        public ProgressStep Sections(List<Section> valueList)
        {
            SectionsList = valueList;

            return this;
        }

        public ProgressStep(string name = "") : base(name)
        {
            ComponentType = "ProgressStep";
        }

        public override string Build()
        {
            var stepNav = "";
            var stepContainer = "";
            var stepCount = 0;
            var stepsTotalCount = SectionsList.SelectMany(x => x.Steps).Where(x => !x.IsIntro).ToList().Count();
            var isFirstStepIntro = false;
            var itemIndex = 0;
            var sectionsStepsCount = 0;
            var sectionsEachIncreament = 100 / SectionsList.Count;
            var stepProgressPercentage = 0.00;

            foreach (var (section, index) in SectionsList.Select((v, i) => (v, i)))
            {
                var sectionProgressPercentage =  ((index + 1) * 100) / (SectionsList.Count());
                sectionsStepsCount += section.Steps.Where(x => !x.IsIntro).Count();
                var sectionStepCount = 0;
                var increament = (double)sectionsEachIncreament / (double)section.Steps.Where(x => !x.IsIntro).Count();

                foreach (var (step, stepIndex) in section.Steps.Select((v, i) => (v, i)))
                {
                    if (itemIndex == 0 && step.IsIntro) { isFirstStepIntro = true; }
                    if (step.IsIntro)
                    {
                        stepNav += $@"<div class=""smart-form-process-step  dashboard-display-none {(itemIndex == 0 ? "smart-form-active-step" : string.Empty)}"">
                              <button type=""button"" data-toggle=""tab"" href=""#{step.StepId}"" onclick=""SmartForm.ProgressStepIntroTabFunction($(this), '{SubmitButtonTitle}', '{(itemIndex == 0  ? IntroButtonTitle : NextStepButtonTitle)}',{itemIndex + 1}, {SectionsList.SelectMany(x => x.Steps).ToList().Count})"">
                              <div class=""smart-form-progress-indicator-container"" data-sectionid=""{section.SectionId}"" data-first-section-step='""false""' data-progress-percentage=""-1""><span class=""smart-form-progress-indicator-number"">{index + 1}</span><span class=""smart-form-progress-indicator-title"">{section.Title}</span></div></button></div>";
                        var currentStepComponents = "";
                        step.StepContent.ForEach(stepContent => { currentStepComponents += stepContent.Build(); });
                        stepContainer += $@"<div id=""{step.StepId}"" data-step-number=""-1"" class=""smart-form-tab-pane tab-pane tab-intro fade {(itemIndex == 0 ? "active in" : string.Empty)} "">{currentStepComponents}</div>";

                    }
                    else
                    {
                        sectionStepCount++;
                        stepCount++;
                        //var stepProgressPercentage = (double)stepCount * (double)sectionProgressPercentage / (double)sectionsStepsCount;
                        stepProgressPercentage += (double)increament;
                        stepNav += $@"<div class=""smart-form-process-step {(sectionStepCount == 1 || stepCount == stepsTotalCount ? string.Empty : " dashboard-display-none")} {(itemIndex == 0 ? "smart-form-active-step smart-form-progress-step-first-step" : (stepCount == 1 ? "smart-form-progress-step-first-step" : (stepCount == stepsTotalCount ? "smart-form-progress-step-last-step" : string.Empty)))}"">
                              <button type=""button"" data-toggle=""tab"" href=""#{step.StepId}"" onclick=""SmartForm.ProgressStepTabFunction($(this), '{SubmitButtonTitle}', '{NextStepButtonTitle}',{stepCount}, {stepsTotalCount})"">
                              <div class=""smart-form-progress-indicator-container {(stepCount == 1 ? "smart-form-progress-indicator-in-progress" : string.Empty)}"" data-sectionid=""{section.SectionId}"" data-first-section-step=""{sectionStepCount == 1}"" data-progress-percentage=""{stepProgressPercentage}"">
                              <span class=""smart-form-progress-indicator-number"">{(stepCount == stepsTotalCount ? "✓" : (index + 1))}</span><span class=""smart-form-progress-indicator-title"">{(stepCount == stepsTotalCount ? string.Empty : section.Title)}</span></div></button></div>";
                        var currentStepComponents = "";
                        step.StepContent.ForEach(stepContent => { currentStepComponents += stepContent.Build(); });
                        stepContainer += $@"<div id=""{step.StepId}"" data-step-number=""{stepCount}"" class=""smart-form-tab-pane tab-pane fade {(itemIndex == 0 ? "active in" : string.Empty)} "">{currentStepComponents}</div>";
                    }

                    itemIndex++;

                }                

            };

            var htmlString = $@"<div class=""smart-form-component smart-form-progress-step-container"" id=""{Id}"" data-required=""{Required.ToString().ToLower()}""  data-Value='{{ ""Required"": ""{Required.ToString().ToLower()}"", ""Id"": ""{Id}"", ""Value"": """" }}'>
                    <div class=""smart-form-process {(ProgressStepNavEnable && !isFirstStepIntro ? string.Empty : "dashboard-display-none")}"" data-progress-indicator-enable=""{ProgressStepNavEnable.ToString().ToLower()}"">
                        <div class=""smart-form-process-row nav nav-tabs"">
                        <div class=""progress smart-form-progress-step-progress-bar"">
                             <div class=""progress-bar"" role=""progressbar"" aria-valuenow=""{(stepsTotalCount > 0 ? 100 / stepsTotalCount : 0)}%"" aria-valuemin=""0"" aria-valuemax=""100"" style=""width: {(stepsTotalCount > 0 ? 100 / stepsTotalCount : 0)}%;""></div>
                        </div>
                           {stepNav}
                        </div>
                    </div>
                    <div class=""smart-form-process-step-tab-content tab-content"">{stepContainer}</div>
                    <div class=""smart-form-progress-step-footer"">
                        <div class=""smart-form-procress-step-count-container {(isFirstStepIntro ? "smart-form-visibility-hidden" : string.Empty)}"">
                            <span> {StepUnit} </span><span class=""smart-form-current-step-number"">1</span><span> از {stepsTotalCount}</span>
                        </div>
                       <button type=""button"" class=""smart-form-prev-step dashboard-display-none"" onclick=""SmartForm.ProgressStepPrevFunction($(this), {stepsTotalCount},'{NextStepButtonTitle}')"">
                            {PrevStepButtonTitle}
                        </button>
                        <button type=""button"" class=""smart-form-next-step""  onclick=""SmartForm.ProgressStepNextFunction($(this),'{SubmitButtonTitle}','{NextStepButtonTitle}', {stepsTotalCount}, {SubmitButtonFunction})"">
                             <div class=""smart-form-loader-container""><span class=""smart-form-loader dashboard-display-none""></span><span class=""smart-form-button-title"">{(isFirstStepIntro ? IntroButtonTitle: NextStepButtonTitle)}</span></div>
                        </button>
                    </div>
                </div>";

            ScriptString += string.IsNullOrEmpty(DataSource.WebApi) ? string.Empty :
                @"<script>;
                </script>";

            return htmlString + ScriptString;
        }

    }

}
