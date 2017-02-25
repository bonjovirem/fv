<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editProjectBrand.aspx.cs" Inherits="sd_order_sys.files.editProjectBrand" %>

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
            系统品牌信息
        </div>
        <div class="panel-body">
            <form id="form1" runat="server" class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">品牌名称：</label>
                    <div class="col-sm-10">
                        <input id="txtName" type="text" name="txtName" class="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">品牌logo：</label>
                    <div class="col-sm-10">
                        <asp:FileUpload ID="txtlogo" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">品牌描述：</label>
                    <div class="col-sm-10">
                        <textarea id="txtdesc" cols="50" rows="3" class="form-control" runat="server"></textarea>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">品牌video：</label>
                    <div class="col-sm-10">
                        <asp:FileUpload ID="txtvideo" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">品牌顺序：</label>
                    <div class="col-sm-10">
                        <input id="brandOrder" type="text" name="brandOrder" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server" class="form-control" value="0"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">所属品类：</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddltype" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">所属项目：</label>
                    <div class="col-sm-10">
                        <asp:Label ID="lblpro" runat="server" Text="" CssClass="form-control"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">是否展示：</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddlisShow" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">是否标星：</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddlisStar" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">全景地址：</label>
                    <div class="col-sm-10">
                        <input id="fvUrl" type="text" name="fvUrl" class="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">所在楼层：</label>
                    <div class="col-sm-10">
                        <input id="Text1" type="text" name="brandOrder" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server" class="form-control"/><p></p>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">商家电话：</label>
                    <div class="col-sm-10">
                        <input id="telephone" type="text" name="telephone" class="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">商家地址：</label>
                    <div class="col-sm-10">
                        <input id="address" type="text" name="address" class="form-control" runat="server" />
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

