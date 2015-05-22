
var controls = function () {

    var parse = function (container) {

        var context = null;

        if (container && container instanceof jQuery) {
            context = container;
        } else {
            context = $(container);
        }

        $.each($('.select2-control', context), function (i, item) {
            var placeholder = $(item).attr('placeholder');
            var options = {
                placeholder: placeholder
            }
            $(item).select2(options);
        });

        // $.each($('select[data-searchable]', context), function (i, item) {
        //    var options = { allowClear: true };
        //    if ($(item).attr("multiple")) {
        //        options.multiple = true;
        //    }
        //    $(item).select2(options);
        //});

        $.each($('input.autonumeric-control', context), function (i, item) {
            $(item).autoNumeric('init');
        });

        ////Parse timepicker.
        // $.each($('div.bootstrap-timepicker input[type="text"]', context), function (i, item) {
        //    $(item).timepicker({ showMeridian: false })
        //        .on('changeTime.timepicker', function (e) {
        //            if ($(this).valid) {
        //                $(this).valid();
        //            }
        //        });
        //});

        //Parse datepicker.
        $.each($('div.bootstrap-datepicker', context), function (i, item) {
            $(item).datepicker({
                autoclose: true,
                todayHighlight: true,
                language: "es-AR"
            }).on('changeDate', function (ev) {
                $(this).datepicker('hide');

                if ($(this).find('input[type="text"]').valid) {
                    $(this).find('input[type="text"]').valid();
                }
            });
        });

        ////Parse auto-submit-input.
        // $.each($('input.auto-submit-input', context), function (i, item) {
        //    var $input = $(item);
        //    $input.keyup(function (e) {
        //        if (e.which != 37 && e.which != 38 & e.which != 39 & e.which != 40 & e.which != 13) {
        //            $(this).closest('form').submit();
        //        }
        //    });
        //});

        ////Parse autocomplete.
        // $.each($('form', context), function (i, item) {
        //    $(this).attr('autocomplete', 'off');
        //});

        ////Parse typeahead.
        // $.each($('input.typeahead', context), function (i, item) {
        //    var $input = $(item);
        //    var typeaheadRemoteUrl = $input.data('url') + '?q=%QUERY';
        //    var typeaheadData = new Bloodhound({
        //        datumTokenizer: function (d) {
        //            return Bloodhound.tokenizers.whitespace(d.value);
        //        },
        //        queryTokenizer: Bloodhound.tokenizers.whitespace,
        //        remote: {
        //            url: typeaheadRemoteUrl,
        //            rateLimitWait: 250,
        //            ajax: { cache: false }
        //        }
        //    });

        //    typeaheadData.initialize();

        //    $input.typeahead({
        //        autoselect: true,
        //        highlight: true,
        //        hint: true,
        //        minLength: 1
        //    }, {
        //        source: typeaheadData.ttAdapter(),
        //        name: 'TypeaheadDataset',
        //        displayKey: $input.data('display-key')
        //    }).on('typeahead:selected', function (event, data) {
        //        $input.data('selected-value', data[$input.data('value-key')]);
        //    });
        //});

    };

    parse($(document));

    return {
        parse: parse
    };
}();

var validation = function () {

    var parse = function (containerSelector) {

        var container = $(document);

        if (containerSelector) {
            container = $(containerSelector);;
        }

        var form = $("form", container)
            .removeData("validator")
            .removeData("unobtrusiveValidation");

        if (form.length > 0) {
            $.validator.unobtrusive.parse(form);
        }
    };

    parse();

    return {
        parse: parse
    };
}();

