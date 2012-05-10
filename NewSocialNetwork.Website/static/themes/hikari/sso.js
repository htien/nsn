jQuery(function($) {

    /* Login scripts */
    var loginButton = 'nsn_login',
        loginForm = 'login_form';
    loginPasswdInput = 'nsn_passwd';

    // Gắn bộ validate cho loginForm
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
            NSN.callJqDlg(glbDefaultDlgId,
                '<h3><img src="/static/themes/hikari/images/loading1.gif" /> Signing in...</h3>',
                {
                    buttons: {}
                }).dialog('open');
            NSN.ajaxSubmit(form)
                .success(function(loginResponse) {
                    if (loginResponse.Status == 1)
                        document.location = NSN.url('/');
                    else {
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
});