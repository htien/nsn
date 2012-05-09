jQuery(function($) {
    var allowLog = glbDebug && !NSN.isIE();

    /* Login scripts */
    var loginButton = 'nsn_login',
        loginForm = 'login_form';
    loginPasswdInput = 'nsn_passwd';
    NSN.$id(loginForm).validate({
        rules: {
            nsnId: { required: true, minlength: 1 },
            nsnPasswd: { required: true, minlength: 1 }
        },
        messages: {
            nsnId: { required: '', minlength: '' },
            nsnPasswd: { required: '', minlength: '' }
        },
        invalidHandler: function(form, validator) {
            NSN.shakeContainer(loginForm);
        },
        submitHandler: function(form) {
            NSN.ajaxSubmit(form)
                .success(function(loginResponse) {
                    if (loginResponse.Status == 1)
                        document.location = NSN.url('/');
                    else
                        NSN.callJqDlg(glbDefaultDlgId, loginResponse.Message, {
                            buttons: {
                                'Close': function() {
                                    jQuery(this).dialog('destroy');
                                }
                            }
                        }).dialog('open');
                })
                .error(function(data) {
                });
            if (allowLog) {
                console.log('__#' + form.id + '__ form is submited.');
            }
        }
    });
    NSN.$id(loginButton).click(function() {
        if (allowLog) {
            console.log('__#' + this.id + '__ button is clicked.');
        }
        NSN.$id(loginForm).submit();
        return false;
    });
});