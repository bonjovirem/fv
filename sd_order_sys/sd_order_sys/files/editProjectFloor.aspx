<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editProjectFloor.aspx.cs" Inherits="sd_order_sys.files.editProjectFloor" %>

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
            项目：  <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>楼层信息
        </div>
      
        <div class="panel-body">
            <form id="form1" runat="server" class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">楼层：</label>
                    <div class="col-sm-10">
                        <input id="floorLevel" type="text" name="floorLevel" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server" class="form-control"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">图片（仅限png格式）：</label>
                    <div class="col-sm-10">
                        <asp:FileUpload ID="floorImg" runat="server" CssClass="form-control" />
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


