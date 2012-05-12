jQuery(function($) {

    /* Login scripts */
    var loginButton = 'nsn_login',
        loginForm = 'login_form',
        loginPasswdInput = 'nsn_passwd';

    NSN.$id(loginForm).validate({
        rules: {
            nsnId: { required: true, minlength: 1 }, // yêu cầu độ dài chuỗi nhập > 1 kí tự
            nsnPasswd: { required: true, minlength: 1 }
        },
        messages: {
            nsnId: { required: '', minlength: '' }, // message thông báo khi nhập không đúng yêu cầu
            nsnPasswd: { required: '', minlength: '' }
        },
        invalidHandler: function(form, validator) { // xử lý khi form nhập vào không hợp lệ
            NSN.shakeContainer(loginForm);
        },
        submitHandler: function(form) {
            var dlgLoading = NSN.callJqDlg(glbDefaultDlgId,
                '<h3><img src="/static/themes/hikari/images/loading1.gif" /> Signing in...</h3>',
                {
                    buttons: {}
                });
            dlgLoading.dialog('open');
            NSN.ajaxSubmit(form)
                .success(function(loginResponse) {
                    if (loginResponse.Status == 1)
                        document.location = NSN.url('/');
                    else {
                        dlgLoading.dialog('destroy');
                        NSN.callJqDlg(glbDefaultDlgId, loginResponse.Message, {
                            width: 500,
                            draggable: true,
                            buttons: {
                                'Close': function() {
                                    jQuery(this).dialog('destroy').remove();
                                    NSN.resetForm(loginForm);
                                    NSN.$id(loginForm).submit();
                                }
                            }
                        }).dialog('open');
                    }
                })
                .error(function(data) { // xử lý khi đường truyền bị lỗi, biến data chứa nội dung thông báo lỗi
                });
            NSN._log('__#' + form.id + '__ form is submited.');
        }
    });
    NSN.$id(loginButton).click(function() {
        NSN._log('__#' + this.id + '__ button is clicked.');
        NSN.$id(loginForm).submit();
        return false;
    });

    /* Register scripts */
    var regForm = 'reg',
        regButton = 'signup';

    NSN.$id(regForm).validate({
        rules: {
            firstname: { required: true, minlength: 1, maxlength: 30 },
            lastname: { required: true, minlength: 1, maxlength: 30 },
            reg_email: { required: true, email: true },
            reg_password: { required: true, minlength: 6 },
            confirm_password: { required: true, equalTo: '#reg_password' },
            gender: { gender: true },
            birthday_year: { birthdayBox: true },
            birthday_month: { birthdayBox: true },
            birthday_day: { birthdayBox: true }
        },
        messages: {
            firstname: { required: '', minlength: '', maxlength: '' },
            lastname: { required: '', minlength: '', maxlength: '' },
            reg_email: { required: '', email: '' },
            reg_password: { required: '', minlength: '' },
            confirm_password: { required: '', equalTo: '' },
            gender: { gender: '' },
            birthday_year: { birthdayBox: '' },
            birthday_month: { birthdayBox: '' },
            birthday_day: { birthdayBox: '' }
        },
        submitHandler: function(form) {
            NSN.callJqDlg(glbDefaultDlgId,
                    '<strong>Are you sure you want to join NSN?</strong>',
                    buildJqConfirmDlgOpts(NSN.$id(regButton).parents('form'), 'Join NSN!')
                ).dialog('open');
        }
    });

    NSN.$id(regButton).click(function() {
        NSN._log('__#' + this.id + '__ button is clicked.');
        NSN.$id(regForm).submit();
        return false;
    });
});