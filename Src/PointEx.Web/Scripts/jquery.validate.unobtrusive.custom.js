(function ($) {

    $.validator.addMethod("unique", function (value, element, params) {
        var prefix = params;
        var selector = jQuery.validator.format("[name!='{0}'][name$='{1}'][data-val-unique-uniquesufix='{1}']", element.name, prefix);
        var matches = new Array();
        $(selector, $(element).closest('form')).each(function (index, item) {
            if (value == $(item).val()) {
                matches.push(item);
            }
        });

        return matches.length == 0;
    });
    $.validator.unobtrusive.adapters.addSingleVal("unique", "uniquesufix");
    

    $.validator.unobtrusive.adapters.add(
        'notequalto', ['other'], function (options) {
            options.rules['notEqualTo'] = '#' + options.params.other;
            if (options.message) {
                options.messages['notEqualTo'] = options.message;
            }
        });

    $.validator.addMethod('notEqualTo', function (value, element, param) {
        return this.optional(element) || value != $(param).val();
    }, '');
    

    $.validator.unobtrusive.adapters.add(
        'notequaltovalue', ['value'], function (options) {
            options.rules['notEqualToValue'] = '#' + options.params.value;
            if (options.message) {
                options.messages['notEqualToValue'] = options.message;
            }
        });

    $.validator.addMethod('notEqualToValue', function (value, element, param) {
        return this.optional(element) || value != $(param).val();
    }, '');

    $.validator.addMethod("isdateafter", function (value, element, params) {
        var parts = element.name.split(".");
        var prefix = "";
        if (parts.length > 1)
            prefix = parts[0] + ".";
        var startdatevalue = $('input[name="' + prefix + params.propertytested + '"]').val();
        if (!value || !startdatevalue)
            return true;

        if (params.onlytime == "True") {
            return (params.allowequaldates) ? startdatevalue <= value : startdatevalue < value;
        }
        return (params.allowequaldates == "True") ? Globalize.parseDate(startdatevalue) <= Globalize.parseDate(value) : Globalize.parseDate(startdatevalue) < Globalize.parseDate(value);
    });

    $.validator.unobtrusive.adapters.add("isdateafter", ["propertytested", "allowequaldates", "onlytime"], function (options) {
        options.rules['isdateafter'] = {
            propertytested: options.params.propertytested,
            allowequaldates: options.params.allowequaldates,
            onlytime: options.params.onlytime
        };
        options.messages['isdateafter'] = options.message;
    });

    $.validator.addMethod('requiredif', function (value, element, parameters) {
        var id = '#' + parameters['dependentproperty'];
        // get the target value (as a string, 
        // as that's what actual value will be)
        var targetvalue = parameters['targetvalue'];
        targetvalue = (targetvalue == null ? '' : targetvalue).toString();

        var targetvaluearray = targetvalue.split('|');

        for (var i = 0; i < targetvaluearray.length; i++) {

            // get the actual value of the target control
            // note - this probably needs to cater for more 
            // control types, e.g. radios
            var control = $(id);
            var controltype = control.attr('type');
            var actualvalue =
                controltype === 'checkbox' ?
                    control.attr('checked') ? "true" : "false" :
                    control.val();

            // if the condition is true, reuse the existing 
            // required field validator functionality
            if (targetvaluearray[i] === actualvalue) {
                return $.validator.methods.required.call(this, value, element, parameters);
            }
        }

        return true;
    });

    $.validator.unobtrusive.adapters.add('requiredif', ['dependentproperty', 'targetvalue'], function (options) {
        options.rules['requiredif'] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['requiredif'] = options.message;
    });

})(jQuery)
