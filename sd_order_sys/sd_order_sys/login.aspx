<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="sd_order_sys.login" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>系统登录</title>
    <link href="css/login.css" rel="stylesheet"  type="text/css" media="all" />
    <link href="css/demo.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src="js/jquery-1.8.2.min.js"></script>
    <script>
        $(function () {

            $(".i-text").focus(function () {
                $(this).addClass('h-light');
            });

            $(".i-text").focusout(function () {
                $(this).removeClass('h-light');
            });

            $("#username").focus(function () {
                var username = $(this).val();
                if (username == '输入账号') {
                    $(this).val('');
                }
            });

            $("#username").focusout(function () {
                var username = $(this).val();
                if (username == '') {
                    $(this).val('输入账号');
                }
            });


            $("#pwd").focus(function () {
                var username = $(this).val();
                if (username == '输入密码') {
                    $(this).val('');
                }
            });
        });




    </script>


</head>

<body>
    <form runat="server" id="form1">

        <div class="header">
            <h1 class="headerLogo"></h1>
        </div>

        <div class="banner">

            <div class="login-aside">
                <div id="o-box-up"></div>
                <div id="o-box-down" style="table-layout: fixed;">
                    <div class="error-box" id=""></div>
                        <div class="fm-item">
                            <label for="logonId" class="form-label">用户名：</label>
                            <input type="text" value="输入账号" maxlength="100" id="username" class="i-text" name="username">
                            <div class="ui-form-explain"></div>
                        </div>

                        <div class="fm-item">
                            <label for="logonId" class="form-label">密码：</label>
                            <input type="password" value="" maxlength="100" id="pwd" class="i-text" datatype="*6-16" nullmsg="请设置密码！" name="pwd">
                            <div class="ui-form-explain"></div>
                        </div>

<%--                        <div class="fm-item pos-r">
                            <label for="logonId" class="form-label">验证码</label>
                            <input type="text" value="输入验证码" maxlength="100" id="yzm" class="i-text yzm" nullmsg="请输入验证码！" name="yzm">
                            <div class="ui-form-explain">
                                <img src="validatecode.aspx" class="yzm-img" id="yzmimg"/>
                            </div>
                        </div>--%>

                        <div class="fm-item">
                            <label for="logonId" class="form-label"></label>
                            <input type="button" value="" tabindex="4" id="send-btn" class="btn-login" onclick="subform()">
                            <div class="ui-form-explain"></div>
                        </div>

                </div>

            </div>

            <div class="bd">
                <ul>
                    <li style="background: url(themes/theme-pic1.jpg) #CCE1F3 center 0 no-repeat;"></li>       
                </ul>
            </div>
        </div>
        
        <script>
            function subform() {
                if ($("#username").val() == "" || $("#username").val() == "输入账号") {
                    alert("请输入账号");
                }
                else if ($("#pwd").val() == "" || $("#pwd").val() == "输入密码") {
                    alert("请输入密码");
                }
                else if ($("#yzm").val() == "" || $("#yzm").val() == "输入验证码") {
                    alert("请输入验证码");
                } else {
                    $.ajax({
                        type: "POST",
                        url: "struts/login.ashx",
                        data: $('#form1').serialize(),
                        datatype: "json",
                        success: function (data) {
                            var comment = $.parseJSON(data);
                            if (comment.msg != "suc") {
                                alert(comment.msg);
                                $("#yzmimg").attr("src", "validatecode.aspx?id=" + Math.random());
                            } else {
                                window.location.href = comment.url;
                            }
                        },
                        //调用出错执行的函数
                        error: function () {
                            alert("网络错误，请联系管理员");
                        }
                    });
                }
            }
            $(document).keydown(function (event) {
                if (event.keyCode == 13) {
                    subform();
                }
            });
        </script>

        <div class="banner-shadow"></div>

        <div class="footer">
            <p class="yoo1">
               
            </p>
        </div>
    </form>
</body>
</html>