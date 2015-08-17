var card = function() {

    var updateExpirationDate = function() {
        var birthDate = $('#BirthDate').val();

        console.log(new Date(birthDate));
    }
    return {
        updateExpirationDate: updateExpirationDate
    }
}();