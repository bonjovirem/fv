<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FloorForm.aspx.cs" Inherits="sd_order_sys.files.FloorForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <script type="text/javascript" src="../js/jquery-1.8.2.min.js"></script>
    <title></title>
</head>
<body>
    <form id="personform" runat="server" action="/struts/SetFloor.ashx?action=opt" enctype="multipart/form-data" method="post">
        <div id="dlg" class="easyui-dialog" style="width: 600px; height: auto; padding: 10px 20px;" closed="true" buttons="#dlg-buttons" title="系统品类信息">
            <div class="ftitle">
            </div>
            <div class="fitem">
                楼层：<input id="floorLevel" type="text" name="floorLevel" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /><p></p>
            </div>
            <div class="fitem">
                图片：<input id="floorImg" type="text" name="floorImg" /><p></p>
            </div>
            <div class="fitem">
                <input id="hid" type="hidden" name="hid" />
                <input id="hidProId" type="hidden" name="hidProId" value="" />
                <input id="hidProName" type="hidden" name="hidProName" value="" />
                <p></p>
            </div>
             <input id="File1" name="File1" type="file" />  
            <input type="hidden" name="hidnum" id="hidnum" />
            <input type="hidden" name="hidaccount" id="hidaccount" />
        </div>
        <div id="dlg-buttons">
            <a href="javascript:void(0)" class="easyui-linkbutton" onclick="ValidateForm()" iconcls="icon-save">保存</a>
             <input type="submit" name="hidnum" id="hidnum" value="保存1" />
            <a href="javascript:void(0)" class="easyui-linkbutton" onclick="javascript:$('#dlg').dialog('close')" iconcls="icon-cancel">取消</a>
        </div>
    </form>
</body>
</html>
    <script>
        function ValidateForm() {
            if ($("#txtName").val() == "") {
                $.messager.alert("提示", "品类名称不能为空");
                return;
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/struts/SetFloor.ashx?action=opt",
                    data: $('#personform').serialize(),
                    datatype: "json",
                    fileElementId: 'File1',
                    success: function (data) {
                        var comment = $.parseJSON(data);
                        if (comment != "suc") {
                            $.messager.alert(comment.msg);
                        } else {
                            $('#dlg').dialog('close');
                            $('#persontable').datagrid('load');
                        }
                    },
                    //调用出错执行的函数
                    error: function () {
                        $.messager.alert("提示", "网络错误，请联系管理员");
                    }
                });
                //$("#personform").aj({
                //    success: function (str) {
                //        if (str != null && str != "undefined") {
                //            if (str == "1") { alert("上传成功"); document.getElementById("img1").src = "images/logo.jpg?" + new Date();/*上传后刷新图片*/ }
                //            else if (str == "2") { alert("只能上传jpg格式的图片"); }
                //            else if (str == "3") { alert("图片不能大于1M"); }
                //            else if (str == "4") { alert("请选择要上传的文件"); }
                //            else { alert('操作失败！'); }
                //        }
                //        else alert('操作失败！');
                //    },
                //    error: function (error) { alert(error); },
                //    url: '/struts/SetFloor.ashx?action=opt', /*设置post提交到的页面*/
                //    type: "post", /*设置表单以post方法提交*/
                //    dataType: "text" /*设置返回值类型为文本*/
                //});
            }
        }
    </script>