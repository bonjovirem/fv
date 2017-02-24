<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editProject.aspx.cs" Inherits="sd_order_sys.files.editProject" %>
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
            项目基本信息
        </div>
        <div class="panel-body">
            <form id="form1" runat="server" class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">项目名称：</label>
                    <div class="col-sm-10">
                          <input id="txtName" type="text" name="txtName" class="form-control" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">项目logo：</label>
                    <div class="col-sm-10">
                        <asp:FileUpload ID="txtlogo" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">项目描述：</label>
                    <div class="col-sm-10">
                        <textarea id="txtdesc" cols="50" rows="3" class="form-control" runat="server"></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">项目video：</label>
                    <div class="col-sm-10">
                        <asp:FileUpload ID="txtvideo" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">项目城市：</label>
                    <div class="col-sm-10">
                        <input id="txtcity" type="text" name="txtcity" class="form-control" runat="server"/>
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


