<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projectBrandType_query.aspx.cs" Inherits="sd_order_sys.files.projectBrandType_query" %>


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
            url: '../struts/projectBrandType_query.ashx?action=query&projectId=<%=projectId %>',
            //queryParams:{},  
            loadMsg: '数据加载中请稍后……',
            pagination: true,
            rownumbers: true,
            pageSize:50,
            title: '<%=projectName%>品类设置',
            columns: [[
                { field: 'ck', checkbox: true, align: 'center' },
                { field: 'projectId', title: '项目ID', align: 'center' },
                { field: 'id', title: '品类ID', align: 'center' },
                { field: 'brandTypeName', title: '品类名称', align: 'center' },
                { field: 'brandTypeOrder', title: '品类顺序', align: 'center' },
                { field: 'brandTypeImg', title: '品类图片', align: 'center' },
                { field: 'brandTypeBackColor', title: '品类背景色', align: 'center' },
                { field: 'isShow', title: '是否显示', align: 'center', formatter: formatString },
                { field: 'createTime', title: '创建时间', align: 'center' },
                { field: 'lastChangeTime', title: '最后修改时间', align: 'center' }
            ]], toolbar: [{
                text: '添加',
                iconCls: 'icon-add',
                handler: function () {
                    window.location = "editProjectType.aspx?projectId=<%=projectId %>&projectName=<%=projectName%>"
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
                text: '编辑项目品牌',
                iconCls: 'icon-add',
                handler: function () {
                    SetBrand();
                }
            }, {
                text: '数据库同步',
                iconCls: 'icon-redo',
                handler: function () {
                    $.ajax({
                        url: '/struts/projectBrandType_query.ashx?action=redo&id=<%=projectId %>',
                        success: function (data) {
                            var comment = $.parseJSON(data);
                            if (comment != "suc") {
                                $.messager.alert("提示", "操作失败，请联系管理员");
                            } else {
                                $.messager.alert("提示", "您同步成功");
                                $('#persontable').datagrid('reload');
                            }
                        },
                        error: function () {
                            $.messager.alert("提示", "网络错误，请联系管理员");
                        }
                    });
                }
            }, {
                text: '刷新',
                iconCls: 'icon-edit',
                handler: function () {
                    reloaddate();
                }
            }]
        });
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
            //$("#btName").attr("value", selectedRow["brandTypeName"]);
            //$("#btOrder").attr("value", selectedRow["brandTypeOrder"]);
            //$("#btImg").attr("value", selectedRow["brandTypeImg"]);
            //$("#btBgcolor").attr("value", selectedRow["brandTypeBackColor"]);
            //$("#btIsshow").attr("value", selectedRow["isShow"]);
            //$("#hid").attr("value", selectedRow["id"]);
            //$('#dlg').dialog('open');
            window.location = "editProjectType.aspx?projectId=<%=projectId %>&projectName=<%=projectName%>&id=" + selectedRow["id"];
            //window.location = "replylist.aspx?id=" + selectedRow["id"];
    } else {
        $.messager.alert('提示', '请选中一条记录');
    }
}
function DelRecord() {
    $.messager.confirm('确认', '是否确认删除所选记录', function (row) {
        if (row) {
            var selectedRow = $('#persontable').datagrid('getSelections'); //获取选中行
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
                    url: '/struts/projectBrandType_query.ashx?action=del&id=' + num,
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
    });
}

function SetBrand() {
    var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
    if (selectedRow) {
        top.topManager.openPage({
            id: 'xmppsz',
            href: '/files/project_query.aspx?projectid=<% =projectId %>&projectName=<% =projectName %>' + '&projectBtId=' + selectedRow["id"] + '&projectBtName=' + selectedRow["brandTypeName"],
            title: '项目品牌设置'
        });
    } else {
        $.messager.alert('提示', '请选中一条记录');
    }
}
function formatString(val, row, index) {
    if (val == 0)
        return "否";
    else
        return "是";
}
</script>
<body>
    <form id="personform" runat="server">
        <table width="100%" border="0">
            <tr>
                <td align="left">
                    <ul>
                        <li>
                            <img src="../images/ico07.gif" /></li>
                        <li>
                            <select id="selectwhere" class="combo">

                                <option value="brandTypeName">品类名称</option>
                            </select>
                        </li>
                        <li>
                            <input id="txtwhere" type="text" />
                        </li>
                        <%--                        <li>
                            <select id="lywhere" style="display: none;" runat="server" name="lywhere">
                            </select>
                        </li>--%>
                        <li><a href="javascript:void(0);" onclick="searchbtn()">
                            <img src="../images/queren.jpg" border="0" /></a>
                            <a href="javascript:void(0);" onclick="reloaddate()">
                                <img src="../images/clear.jpg" border="0" /></a>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
        <table id="persontable">
            <tr>
                <th field="id" width="100" hidden="true">序号</th>
            </tr>
        </table>
        <div id="dlg" class="easyui-dialog" style="width: 600px; height: auto; padding: 10px 20px;" closed="true" buttons="#dlg-buttons" title="项目品类信息">
            <div class="ftitle">
            </div>
            <div class="fitem">
                品类名称：<input id="btName" type="text" name="btName" /><p></p>
            </div>
            <div class="fitem">
                品类顺序：<input id="btOrder" type="text" name="btOrder" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /><p></p>
            </div>
            <div class="fitem">
                品类图片：<input id="btImg" type="text" name="btImg" /><p></p>
            </div>
            <div class="fitem">
                品类背色：<input id="btBgcolor" type="text" name="btBgcolor" /><p></p>
            </div>
            <div class="fitem">
                是否显示： 
                <select id="btIsShow" name="btIsShow" class="combo">
                    <option value="1">是</option>
                    <option value="0">否</option>
                </select><p></p>
            </div>
            <div class="fitem">
                <input id="hid" type="hidden" name="hid" />
                <p></p>
            </div>
            <input type="hidden" name="hidnum" id="hidnum" />
            <input type="hidden" name="hidaccount" id="hidaccount" />
            <input type="hidden" name="projectId" id="projectId" value="<%=projectId %>" />
        </div>
        <div id="dlg-buttons">
            <a href="javascript:void(0)" class="easyui-linkbutton" onclick="ValidateForm()" iconcls="icon-save">保存</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" onclick="javascript:$('#dlg').dialog('close')" iconcls="icon-cancel">取消</a>
        </div>
    </form>
    <script>
        function searchbtn() {
            var where = $("#txtwhere").val();
            var cul = $('#selectwhere').val();
            $('#persontable').datagrid('options').url = "/struts/projectBrandType_query.ashx?action=query&cul=" + cul + "&where=" + encodeURI(where)+"&projectId="+<%=projectId %>;
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
                $.ajax({
                    type: "POST",
                    url: "/struts/projectBrandType_query.ashx?action=add",
                    data: $('#personform').serialize(),
                    datatype: "json",
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
            }
        }
    </script>
</body>
</html>
