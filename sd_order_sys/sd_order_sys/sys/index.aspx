<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="sd_order_sys.sys.index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/assets/css/dpl-min.css" rel="stylesheet" type="text/css" />
    <link href="/assets/css/bui-min.css" rel="stylesheet" type="text/css" />
    <link href="/assets/css/main-min.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="dl-title">
                山东全景导览后台管理系统
            </div>
            <div class="dl-log">欢迎您，<span class="dl-log-user"><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></span><a href="/quit.aspx" title="退出系统" class="dl-log-quit">[退出]</a></div>

        </div>
        <div class="content">
            <div class="dl-main-nav">
                <ul id="J_Nav" class="nav-list ks-clear">
                    <li class="nav-item dl-selected">
                        <div class="nav-item-inner nav-home">首&nbsp;页</div>
                    </li>
                </ul>
            </div>
            <ul id="J_NavContent" class="dl-tab-conten">
            </ul>
        </div>
        <script type="text/javascript" src="/js/jquery-1.8.2.min.js"></script>
        <script type="text/javascript" src="/assets/js/bui.js"></script>
        <script type="text/javascript" src="/assets/js/config.js"></script>

        <script>
            BUI.use('common/main', function () {
                var config = <%=msg%>
                new PageUtil.MainPage({
                    modulesConfig: config
                });
            });
        </script>
    </form>
</body>
</html>


