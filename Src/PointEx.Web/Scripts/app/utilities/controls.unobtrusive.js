
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

        $.each($('.file', context), function (i, item) {
            $(item).on('filecleared', function () {
                $('#OriginalFileWasRemoved').val(true);
            });
        });

        $('.list-group-sortable').sortable({ placeholderClass: 'list-group-item' });

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
                language: "es-AR",
                clearBtn: true
            }).on('changeDate', function (ev) {
                $(this).datepicker('hide');

                if ($(this).find('input[type="text"]').valid) {
                    $(this).find('input[type="text"]').valid();
                }
            });
        });

        //Keep open dropdowns
        $('.keep-open').on('click', '[type=checkbox],label:has([type=checkbox])', function (e) {
            e.stopPropagation();
        });

        $('.keep-open').on(
        {
            "click": function (e) {
                var target = $(e.target);
                if (target.is(":submit") || target.parent().is(":submit")) {
                    return true;
                }
                //else if (target.is(":reset") || target.parent().is(":reset")) {
                //    return true;
                //}
                return false;
            }
        });

        $('button[type="reset"]').on(
        {
            "click": function (e) {
                var $parentForm = $(this).closest('form');
                $('.select2-control', $parentForm).each(function () {
                    $(this).select2("val", "");
                });
                $('select,input', $parentForm).val("");
                $('input[type=checkbox]', $parentForm).attr('checked', false);
            }
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