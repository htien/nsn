jQuery(function($) {

    /* Register scripts */
    var regForm = 'reg',
        regButton = 'signup';

    NSN.$id(regForm).validate({
        rules: {
            
        },
        messages: {
        },
        invalidHandler: function(form, validator) {
        },
        submitHandler: function(form) {
        }
    });

    NSN.$id(regButton).click(function() {
        NSN._log('__#' + this.id + '__ button is clicked.');
        NSN.$id(regForm).submit();
        return false;
    });

});