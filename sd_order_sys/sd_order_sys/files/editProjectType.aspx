<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editProjectType.aspx.cs" Inherits="sd_order_sys.files.editProjectType" %>
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
            项目品类信息
        </div>
        <div class="panel-body">
            <form id="form1" runat="server" class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">品类名称：</label>
                    <div class="col-sm-10">
                          <input id="txtName" type="text" name="txtName" class="form-control" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">品类logo：</label>
                    <div class="col-sm-10">
                        <asp:FileUpload ID="txtlogo" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">品类顺序：</label>
                    <div class="col-sm-10">
                       <input id="btOrder" type="text" name="btOrder" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server" class="form-control"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">所属项目：</label>
                    <div class="col-sm-10">
                        <asp:Label ID="Label1" runat="server" Text="Label" CssClass="form-control"></asp:Label><asp:Label ID="Label2" runat="server" Text="Label" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" CssClass="col-sm-6 control-label"></asp:Label>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <asp:Button ID="btnExport" runat="server" Text=" 保 存 " CssClass="btn btn-primary" OnClick="btnExport_Click" />
                    </div>
                </div>
                <input id="hidpro" type="hidden" runat="server" />
            </form>
        </div>
    </div>
</body>
</html>
