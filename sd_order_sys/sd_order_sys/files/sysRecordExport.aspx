<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sysRecordExport.aspx.cs" Inherits="sd_order_sys.files.sysRecordExport" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="panel panel-default">
        <div class="panel-heading">
            系统品牌上传面板</div>
        <div class="panel-body">
            <form id="form1" runat="server" class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">品类文件：</label>
                    <div class="col-sm-10">
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" CssClass="col-sm-6 control-label"></asp:Label>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <asp:Button ID="btnExport" runat="server" Text="上传" CssClass="btn btn-primary" OnClick="btnExport_Click" />
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
