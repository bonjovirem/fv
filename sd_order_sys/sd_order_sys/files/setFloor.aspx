<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="setFloor.aspx.cs" Inherits="sd_order_sys.files.setFloor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../js/jquery-1.8.2.min.js"></script>
    <!-- 加载jquery-easyui -->
    <link rel="stylesheet" type="text/css" href="../plugins/themes/metro/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../plugins/themes/icon.css" />
    <script type="text/javascript" src="../plugins/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../plugins/locale/easyui-lang-zh_CN.js"></script>
    <style type="text/css">
        li {
            float: left;
            list-style: none;
            padding: 2px;
        }

        ul {
            padding: 2px;
            margin: 5px;
        }

        input {
            border: 1px solid #D3D3D3;
            border-radius: 5px;
            min-height: 18px;
            max-height: 22px;
            cursor: text;
            background-color: White;
            padding: 1px 5px;
            font-size: 12px;
            vertical-align: middle;
            width: 200px;
        }
    </style>
</head>
<script>
    //页面加载  
    $(document).ready(function () {
        loadGrid();
    });
    //加载表格datagrid  
    function loadGrid() {
        //加载数据  
        $('#persontable').datagrid({
            //fit:true,
            width: 'auto',
            height: 'auto',
            striped: false,
            singleSelect: false,
            url: '../struts/SetFloor.ashx?action=query&projectId=<%=proId%>',
            //queryParams:{},  
            loadMsg: '数据加载中请稍后……',
            pagination: true,
            rownumbers: true,
            columns: [[
                { field: 'ck', checkbox: true, align: 'center' },
                { field: 'id', title: 'floorId', align: 'center' },
                { field: 'floorLevel', title: '第几层', align: 'center' },
                { field: 'floorImg', title: '楼层图片', align: 'center' },
                { field: 'createTime', title: '创建时间', align: 'center' },
                { field: 'lastChangeTime', title: '最后修改时间', align: 'center' },
                { field: 'hasClient', title: '是否设置client', align: 'center', formatter: formatStringClient  }
            ]], toolbar: [{
                text: '添加',
                iconCls: 'icon-add',
                handler: function () {
                    window.location = "editProjectFloor.aspx?proId=<%=proId%>&projectName=<%=proName%>";
                }
            },
            {
                text: '编辑',
                iconCls: 'icon-edit',
                handler: function () {
                    rowMark();
                }
            }, {
                text: '删除',
                iconCls: 'icon-remove',
                handler: function () {
                    DelRecord();
                }
            }, {
                text: '编辑客户机/电梯点路径',
                iconCls: 'icon-add',
                handler: function () {
                    SetClientandLift();
                }
            }, {
                text: '刷新',
                iconCls: 'icon-add',
                handler: function () {
                    reloaddate();
                }
            }, {
                text: '生成html',
                iconCls: 'icon-add',
                handler: function () {
                    buildHTML();
                }
            }]
        });
    }
    function formatString(val, row, index) {
        if (val == 0)
            return "是";
        else
            return "否";
    }
    function formatStringClient(val, row, index) {
        if (val == 3)
            return "全设置";
        else if (val == 2)
            return "只client";
        else if (val == 1)
            return "只电梯";
        else
            return "未设置"
    }
    function formatUploadFile(val, row, index) {
        if (val) {
            return '<a href="' + val + '" title="下载文件" >下载文件</a>';
        }
        else
            return "";
    }

    function rowMark(index) {
        $('#persontable').datagrid('selectRow', index);
        var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
        if (selectedRow) {
            //$("#dlg").dialog().parent().appendTo("#personform");
            //$("#floorLevel").attr("value", selectedRow["floorLevel"]);
            ////$("#floorImg").attr("value", selectedRow["floorImg"]);
            //$("#hid").attr("value", selectedRow["id"]);
            //$('#dlg').dialog('open');
            window.location = "editProjectFloor.aspx?id=" + selectedRow["id"]+"&proId=<%=proId%>&projectName=<%=proName%>";
    } else {
        $.messager.alert('提示', '请选中一条记录');
    }
}
function DelRecord() {
    $.messager.confirm('确认', '是否确认删除所选记录', function (row) {
        if (row) {
            var selectedRow = $('#persontable').datagrid('getSelections');  //获取选中行
            if (selectedRow.length == 0) {
                $.messager.alert('提示', '请选中一条记录');
            } else {
                var num = "";
                for (var i = 0; i < selectedRow.length; i++) {
                    if (i == selectedRow.length - 1) {
                        num = num + selectedRow[i].id + "";
                    } else {
                        num = num + selectedRow[i].id + ",";
                    }
                }
                $.ajax({
                    url: '/struts/SetFloor.ashx?action=rRecord&id=' + num,
                    success: function (data) {
                        var comment = $.parseJSON(data);
                        if (comment != "suc") {
                            $.messager.alert("提示", "操作失败，请联系管理员");
                        } else {
                            $.messager.alert("提示", "您删除成功");
                            $('#persontable').datagrid('reload');
                        }
                    },
                    error: function () {
                        $.messager.alert("提示", "网络错误，请联系管理员");
                    }
                });
            }
        }
    })
}

function SetClientandLift(index) {
    $('#persontable').datagrid('selectRow', index);
    var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
    if (selectedRow) {
        //$("#hid").attr("value", selectedRow["id"]);
        window.open( 'DrawCtoLift.aspx?floorId=' + selectedRow["id"] + "&floorLevel=" + selectedRow["floorLevel"]+"&projectId="+<%=proId %>);
    } else {
        $.messager.alert('提示', '请选中一条记录');
    }
}
function buildHTML(index) {

    $('#persontable').datagrid('selectRow', index);
    var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
    $("#hid").attr("value", selectedRow["id"]);
    if (selectedRow) {
        $.ajax({
            url: '/struts/city.ashx?action=build&id=' +<%=proId %>+"&floorLevel="+selectedRow["floorLevel"],
            success: function (data) {
                var comment = $.parseJSON(data);
                if (comment != "suc") {
                    $.messager.alert("提示", "操作失败，请联系管理员");
                } else {
                    $.messager.alert("提示", "您操作成功");
                }
            },
            error: function () {
                $.messager.alert("提示", "网络错误，请联系管理员");
            }
        });
    } else {
        $.messager.alert('提示', '请选中一条记录');
    }
       
}
function SetLift(index) {

    $('#persontable').datagrid('selectRow', index);
    var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
    if (selectedRow) {
        //$("#hid").attr("value", selectedRow["id"]);
        window.open(  '/files/DrawLift.aspx?floorId=' + selectedRow["id"] + "&floorLevel=" + selectedRow["floorLevel"]+"&projectId="+<%=proId %>);
    } else {
        $.messager.alert('提示', '请选中一条记录');
    }
   <%-- var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
    if (selectedRow) {
        top.topManager.openPage({
            id: 'szlift',
            href: '/files/DrawLift.aspx?floorId=' + selectedRow["id"] + "&floorLevel=" + selectedRow["floorLevel"]+"&projectId="+<%=proId %>,
                title: '电梯点设置'
            });
        } else {
            $.messager.alert('提示', '请选中一条记录');
        }--%>
}
</script>
<body>
    <form id="personform" runat="server" action="/struts/SetFloor.ashx?action=opt">
        <input id="hidpro" type="hidden" name="hidpro" runat="server" />
        <table width="100%" border="0">
            <tr>
                <td align="left">
                    <ul>
                        <li>
                            <img src="../images/ico07.gif" /></li>
                        <li>
                            <span style="font-size: 16px;"><b><%=proName %>(<%=proId %>)</b></span>
                        </li>
                        <li></li>
                        <%--                        <li>
                            <select id="lywhere" style="display: none;" runat="server" name="lywhere">
                            </select>
                        </li>--%>
                        <li></li>
                    </ul>
                </td>
            </tr>
        </table>
        <table id="persontable">
            <tr>
                <th field="id" width="100" hidden="true">序号</th>
            </tr>
        </table>
        <%--<div id="dlg" class="easyui-dialog" style="width: 600px; height: auto; padding: 10px 20px;" closed="true" buttons="#dlg-buttons" title="系统品类信息">
            <div class="ftitle">
            </div>
            <div class="fitem">
                楼层：<input id="floorLevel" type="text" name="floorLevel" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /><p></p>
            </div>
            <div class="fitem">
                图片：<input id="floorImg" type="file" name="floorImg" /><p></p>
            </div>
            <div class="fitem">
                <input id="hid" type="hidden" name="hid" />
                <input id="hidProId" type="hidden" name="hidProId" value="<%=proId %>" />
                <input id="hidProName" type="hidden" name="hidProName" value="<%=proName %>" />
                <p></p>
            </div>
            <input type="hidden" name="hidnum" id="hidnum" />
            <input type="hidden" name="hidaccount" id="hidaccount" />
        </div>
      <div id="dlg-buttons">
            <a href="javascript:void(0)" class="easyui-linkbutton" onclick="ValidateForm()" iconcls="icon-save">保存</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" onclick="javascript:$('#dlg').dialog('close')" iconcls="icon-cancel">取消</a>
        </div>--%>
    </form>
    <script>
        function searchbtn() {
            var where = $("#txtwhere").val();
            var cul = $('#selectwhere').val();
            $('#persontable').datagrid('options').url = "/struts/SetFloor.ashx?action=query&cul=" + cul + "&where=" + encodeURI(where);
            $('#persontable').datagrid('load');
        }
        function reloaddate() {
            $('#persontable').datagrid('load');
        }
        function ValidateForm() {
            if ($("#txtName").val() == "") {
                $.messager.alert("提示", "品类名称不能为空");
                return;
            }
            else {
                $("#personform").submit();
                //$.ajax({
                //    type: "POST",
                //    url: "/struts/SetFloor.ashx?action=opt",
                //    data: $('#personform').serialize(),
                //    datatype: "json",
                //    fileElementId: 'floorImg',
                //    success: function (data) {
                //        var comment = $.parseJSON(data);
                //        if (comment != "suc") {
                //            $.messager.alert(comment.msg);
                //        } else {
                //            $('#dlg').dialog('close');
                //            $('#persontable').datagrid('load');
                //        }
                //    },
                //    //调用出错执行的函数
                //    error: function () {
                //        $.messager.alert("提示", "网络错误，请联系管理员");
                //    }
                //});
            }
        }
    </script>
</body>
</html>
