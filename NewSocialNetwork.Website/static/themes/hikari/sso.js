jQuery(function($) {
    var allowLog = glbDebug && !NSN.isIE();

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
        submitHandler: function(form) { // xử lý khi form nhập vào hợp lệ
            NSN.ajaxSubmit(form) // dùng ajax để submit nội dung form và gửi lên server
                .success(function(loginResponse) { // server sẽ trả về json, biến loginResponse chứa nội dung json trả về
                    if (loginResponse.Status == 1) // khi đăng nhập thành công
                        document.location = NSN.url('/');
                    else // khi đăng nhập thất bại, gọi ngay dialog thông báo
                        NSN.callJqDlg(glbDefaultDlgId, loginResponse.Message, {
                            buttons: {
                                'Close': function() { // tạo ra 1 nút button, ấn vào để tắt dialog
                                    jQuery(this).dialog('destroy');
                                }
                            }
                        }).dialog('open');
                })
                .error(function(data) { // xử lý khi đường truyền bị lỗi, biến data chứa nội dung thông báo lỗi
                });
            if (allowLog) { // log nội dung trên các trình duyệt thuộc webkit
                console.log('__#' + form.id + '__ form is submited.');
            }
        }
    });
    NSN.$id(loginButton).click(function() { // gắn sự kiện click cho nút Sign In
        if (allowLog) {
            console.log('__#' + this.id + '__ button is clicked.');
        }
        NSN.$id(loginForm).submit();
        return false;
    });
});