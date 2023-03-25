var SmartForm = SmartForm || {};

SmartForm.ProgressStepIntroTabFunction = function ($element, submitBtnTitle, nextStepBtnTitle, stepNumber, stepCount) {
    var $progressStepContainer = $element.parents('.smart-form-progress-step-container');
    $progressStepContainer.find('.smart-form-process-step').removeClass('smart-form-active-step');
    $element.parent().addClass('smart-form-active-step');
    SmartForm.UpdateProgressIndicator($progressStepContainer, $element.attr("href").replace("#", ""), stepCount, stepNumber);
    if (stepNumber == 1) {
        $progressStepContainer.find('.smart-form-prev-step').addClass('dashboard-display-none');
        $progressStepContainer.find('.smart-form-next-step').find('.smart-form-button-title').text(nextStepBtnTitle);
    } else if (stepNumber == stepCount) {
        $progressStepContainer.find('.smart-form-next-step').find('.smart-form-button-title').text(submitBtnTitle);
        $progressStepContainer.find('.smart-form-prev-step').addClass('dashboard-display-none');
    } else {
        $progressStepContainer.find('.smart-form-prev-step').removeClass('dashboard-display-none');
        $progressStepContainer.find('.smart-form-next-step').find('.smart-form-button-title').text(nextStepBtnTitle);
    }
};

SmartForm.ProgressStepTabFunction = function ($element, submitBtnTitle, nextStepBtnTitle, stepNumber, stepCount) {
    var $progressStepContainer = $element.parents('.smart-form-progress-step-container');
    $progressStepContainer.find('.smart-form-process-step').removeClass('smart-form-active-step');
    $element.parent().addClass('smart-form-active-step');
    $progressStepContainer.find('.smart-form-current-step-number').parent('.smart-form-procress-step-count-container').removeClass('smart-form-visibility-hidden');
    $progressStepContainer.find('.smart-form-current-step-number').text(stepNumber)


    SmartForm.UpdateProgressIndicator($progressStepContainer, $element.attr("href").replace("#", ""), stepCount, stepNumber);

    if (stepNumber == 1) {
        $progressStepContainer.find('.smart-form-prev-step').addClass('dashboard-display-none');
        $progressStepContainer.find('.smart-form-next-step').find('.smart-form-button-title').text(nextStepBtnTitle);
    } else if (stepNumber == stepCount) {
        $progressStepContainer.find('.smart-form-next-step').find('.smart-form-button-title').text(submitBtnTitle);
        $progressStepContainer.find('.smart-form-prev-step').addClass('dashboard-display-none');
    } else {
        $progressStepContainer.find('.smart-form-prev-step').removeClass('dashboard-display-none');
        $progressStepContainer.find('.smart-form-next-step').find('.smart-form-button-title').text(nextStepBtnTitle);
    }
};

SmartForm.ProgressStepNextFunction = function ($element, submitBtnTitle, nextStepBtnTitle, stepCount, submitFunction) {
    var $progressStepContainer = $element.parents('.smart-form-progress-step-container');
    var $activeTab = $progressStepContainer.find('.tab-pane.active');
    if (parseInt($activeTab.attr('data-step-number')) == stepCount) {
        SmartForm.ProgressStepSubmitFunction($element, submitFunction);
        return;
    }
    $progressStepContainer.find('.smart-form-process-step').removeClass('smart-form-active-step');
    if ($activeTab.next('.tab-pane').attr('data-step-number') != undefined &&
        parseInt($activeTab.next('.tab-pane').attr('data-step-number')) != -1) {
        var nextStepNumber = parseInt($activeTab.next('.tab-pane').attr('data-step-number'));
        $progressStepContainer.find('.smart-form-current-step-number').parent('.smart-form-procress-step-count-container').removeClass('smart-form-visibility-hidden');
        $progressStepContainer.find('.smart-form-current-step-number').text(nextStepNumber)

        var targetStepNumber = parseInt($activeTab.next('.tab-pane').attr('data-step-number'));

        SmartForm.UpdateProgressIndicator($progressStepContainer, $activeTab.next('.tab-pane').attr("id"), stepCount, targetStepNumber);
    }
    $progressStepContainer.find('.smart-form-next-step').find('.smart-form-button-title').text(nextStepBtnTitle);
    var nextTab = $activeTab.next('.tab-pane').attr('id');
    $('[href=\'#' + nextTab + '\']').parent().removeClass('smart-form-active-step');
    $('[href=\'#' + nextTab + '\']').tab('show');
    if ($activeTab.next('.tab-pane').attr('data-step-number') != undefined &&
        parseInt($activeTab.next('.tab-pane').attr('data-step-number')) > 1) {
        $progressStepContainer.find('.smart-form-prev-step').removeClass('dashboard-display-none');
    }
    if (parseInt($activeTab.attr('data-step-number')) == stepCount - 1) {
        $progressStepContainer.find('.smart-form-next-step').find('.smart-form-button-title').text(submitBtnTitle);
    }
}

SmartForm.ProgressStepSubmitFunction = function ($element, submitFunction) {
    var data = [];
    var $mainComponent = $element.parents('.smart-form-progress-step-container');
    $mainComponent.find('.smart-form-component').each(function (index, item) {
        data.push($(item).data('value'));
    });
    var componentData = { "Required": $mainComponent.attr('data-required'), "Id": $mainComponent.attr('Id'), "Value": data };
    $mainComponent.data('value', componentData);
    submitFunction($mainComponent.data('value'), $mainComponent.closest('.smart-form-container').attr('Id'), $mainComponent.find('.smart-form-next-step'));
}

SmartForm.ProgressStepPrevFunction = function ($element, stepCount, nextStepBtnTitle) {
    var $progressStepContainer = $element.parents('.smart-form-progress-step-container');
    var $activeTab = $progressStepContainer.find('.tab-pane.active');
    $progressStepContainer.find('.smart-form-process-step').removeClass('smart-form-active-step');
    $progressStepContainer.find('.smart-form-current-step-number').parent('.smart-form-procress-step-count-container').removeClass('smart-form-visibility-hidden');
    $progressStepContainer.find('.smart-form-current-step-number').text(parseInt($activeTab.attr('data-step-number')) - 1);


    var prevTab = $activeTab.prevAll('.tab-pane:not(.tab-intro)').first().attr('id');
    var targetStepNumber = parseInt($activeTab.prevAll('.tab-pane:not(.tab-intro)').first().attr('data-step-number'));

    SmartForm.UpdateProgressIndicator($progressStepContainer, prevTab, stepCount, targetStepNumber);

    $('[href=\'#' + prevTab + '\']').parent().addClass('smart-form-active-step');
    $('[href=\'#' + prevTab + '\']').tab('show');
    $progressStepContainer.find('.smart-form-next-step').find('.smart-form-button-title').text(nextStepBtnTitle);
    if (parseInt($activeTab.attr('data-step-number')) - 1 == 1) {
        $progressStepContainer.find('.smart-form-prev-step').addClass('dashboard-display-none');
    }

}

var stepStatus = [];
SmartForm.UpdateProgressIndicator = function ($progressStepContainer, targetStepId, stepCount, targetStepNumber) {
    var $targetIndicator = $progressStepContainer.find('[href=\'#' + targetStepId + '\']');
    var progressStepPercentage = $targetIndicator.find('.smart-form-progress-indicator-container').data("progress-percentage");
    var sectionId = $targetIndicator.find('.smart-form-progress-indicator-container').data("sectionid");

    if (stepStatus.length == 0) {
        $progressStepContainer.find('.smart-form-progress-indicator-container').each(function (index, element) {
            var stepData = {
                stepId: $(element).parent().attr("href").replace("#", ""),
                sectionId: $(element).data("sectionid"),
                firstSectionStep: ($(element).data("first-section-step") != undefined ? $(element).data("first-section-step").toLowerCase() == "true" : false),
                progressPercentage: $(element).data("progress-percentage")
            };
            if ($(element).data("progress-percentage") != -1) {
                stepStatus.push(stepData);
            }
        });
    }

    stepStatus.forEach(function (item, index) {
        var $indicator = $progressStepContainer.find('[href=\'#' + item.stepId + '\']');

        if (targetStepNumber == stepCount) {
            $indicator.find('.smart-form-progress-indicator-container').removeClass('smart-form-progress-indicator-in-progress').addClass('smart-form-progress-indicator-done');
        } else if (index + 1 <= targetStepNumber) {
            if (item.sectionId != sectionId) {
                $indicator.find('.smart-form-progress-indicator-container').removeClass('smart-form-progress-indicator-in-progress').addClass('smart-form-progress-indicator-done');
            } else {
                $indicator.find('.smart-form-progress-indicator-container').removeClass("smart-form-progress-indicator-done").addClass('smart-form-progress-indicator-in-progress');
            }
        } else if (index + 1 > targetStepNumber && targetStepNumber != -1) {
            $indicator.find('.smart-form-progress-indicator-container').removeClass('smart-form-progress-indicator-in-progress smart-form-progress-indicator-done');
        }
    });

    if (progressStepPercentage != -1) {//&& $progressStepContainer.find('.smart-form-process').data("progress-indicator-enable") == true
        $progressStepContainer.find('.smart-form-process').removeClass("dashboard-display-none");
        $progressStepContainer.find('.smart-form-progress-step-progress-bar .progress-bar').attr("aria-valuenow", progressStepPercentage + "%");
        $progressStepContainer.find('.smart-form-progress-step-progress-bar .progress-bar').css("width", progressStepPercentage + "%");
    }
}

SmartForm.SingleSelectFunction = function (element, mainComponent) {
    if ($(element).is(':checked')) {
        var group = 'input:checkbox[name="' + $(element).attr('name') + '"]';
        $(mainComponent).find(group).prop('checked', false);
        $(mainComponent).find(group).parent().removeClass('smart-form-multi-choice-item-checked');
        $(element).prop('checked', true);
        $(element).parent().addClass('smart-form-multi-choice-item-checked');
    } else {
        $(element).prop('checked', false);
        $(element).parent().removeClass('smart-form-multi-choice-item-checked');
    }

    var data = { "Required": $(mainComponent).attr('data-required'), "Id": $(mainComponent).attr('Id'), "Value": $(mainComponent).find('input:checked').val() != undefined ? $(mainComponent).find('input:checked').val() : '' };
    $(mainComponent).data('value', data);
}
SmartForm.MultiSelectFunction = function (element, mainComponent) {
    var checkedItems = [];
    if ($(element).is(':checked')) {
        $(element).parent().addClass('smart-form-multi-choice-item-checked');
    } else {
        $(element).parent().removeClass('smart-form-multi-choice-item-checked');
    }

    $(mainComponent).find('input:checked').each(function (index, item) { checkedItems.push($(item).val()); });
    var data = { "Required": $(mainComponent).attr('data-required'), "Id": $(mainComponent).attr('Id'), "Value": checkedItems.length > 0 ? checkedItems.join(',') : '' };
    $(mainComponent).data('value', data);
}
SmartForm.TextAreaChangedFunction = function (element) {
    $(element).unbind('blur');
    var $mainComponent = $(element).closest('.smart-form-component');
    var data = { "Required": $mainComponent.attr('data-required'), "Id": $mainComponent.attr('Id'), "Value": $(element).val() };
    $mainComponent.data('value', data);

}
SmartForm.RatingFunction = function (element) {
    var $mainComponent = $(element).closest('.smart-form-component');
    var data = { "Required": $mainComponent.attr('data-required'), "Id": $mainComponent.attr('Id'), "Value": $(element).val() };
    $mainComponent.data('value', data);
}


SmartForm.ClearForm = function ($component, clearAllInput = true) {
    $component.find('input').each(function (index, element) {
        $(element).prop('checked', false);
        $(element).parent().removeClass('smart-form-multi-choice-item-checked');
    });
    $component.find('textarea').val('');

    if (clearAllInput) {
        $component.find('.smart-form-component').each(function (index, item) {
            var data = $(item).data('value');
            if (data != undefined) {
                data.Value = '';
                $(item).data('value', data);
            }
        });
        $component.find('.smart-form-progress-step-first-step button').click();
    } else {
        var data = $component.data('value');
        if (data != undefined) {
            data.Value = '';
            $component.data('value', data);
        }
    }
}
SmartForm.DisplayProgressStepLoader = function ($element, visibility) {
    if (visibility) {
        $element.find('.smart-form-loader').removeClass('dashboard-display-none');
        $element.find('.smart-form-loader').parents('.smart-form-progress-step-footer').find('button').attr('disabled', true);
    } else {
        $element.find('.smart-form-loader').addClass('dashboard-display-none');
        $element.find('.smart-form-loader').parents('.smart-form-progress-step-footer').find('button').attr('disabled', false);
    }
}
