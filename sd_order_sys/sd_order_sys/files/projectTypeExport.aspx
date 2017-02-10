<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projectTypeExport.aspx.cs" Inherits="sd_order_sys.files.projectTypeExport" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    项目名称：<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red"></asp:Label><p></p>
                    <asp:Button ID="btnExport" runat="server" Text="上传" CssClass="btn btn-primary" OnClick="btnExport_Click"/>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
