(function () {

    //Fix multiple form submition first time
    $.ajaxSetup({
        async: false
    });

    $("#create-edit-benefit-form").submit(function () {

        var isAnyAdminUser = $('input[type="submit"]', this).data('isanyadminuser');

        var typeValue = $("#BenefitTypeId").val();
        if (typeValue == 1) {
            var percentage = $("#DiscountPercentage").val();
            var percentageCeilling = $("#DiscountPercentageCeiling").val();
                        
            if (!(percentage != '' && percentage > 0)) {
                $('form').validate();
                $("#DiscountPercentage").rules('add', {
                    required: true,
                    messages: {
                        required: 'El campo Porcentaje de Descuento debe ser un número.'
                    }
                });
            };

            if (!(percentageCeilling != '' && percentageCeilling > 0)) {
                $('form').validate();
                $("#DiscountPercentageCeiling").rules('add', {
                    required: true,
                    messages: {
                        required: 'El campo Tope Porcentaje de Descuento debe ser un número.'
                    }
                });                
            }           
        }

        if ($('#create-edit-benefit-form').valid()) {

            if (isAnyAdminUser === "True") {
                return true;
            }

            return confirm("El Beneficio debe ser revisado y aprobado por la administración. Durante dicho periodo el mismo no estará disponible públicamente en el sitio");
        }

        return false;
    });

    $("#BenefitTypeId").change(function () {
        var typeValue = $(this).val();
        if (typeValue != 1) {
            $("#DiscountPercentage").closest(".form-group").hide();
            $("#DiscountPercentageCeiling").closest(".form-group").hide();           
        }
        else {
            $("#DiscountPercentage").closest(".form-group").show();
            $("#DiscountPercentageCeiling").closest(".form-group").show();           
        }
    }).trigger("change");
})();
