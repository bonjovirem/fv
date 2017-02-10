<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cityProject_query.aspx.cs" Inherits="sd_order_sys.files.cityProject_query" %>

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
            url: '../struts/city.ashx?action=query',
            //queryParams:{},  
            loadMsg: '数据加载中请稍后……',
            pagination: true,
            rownumbers: true,
            columns: [[
                { field: 'ck', checkbox: true, align: 'center' },
                { field: 'projectName', title: '项目名称', align: 'center' },
                { field: 'projectLogo', title: '项目logo', align: 'center', formatter: formatUploadFile },
                { field: 'projectDesc', title: '项目描述', align: 'center' },
                { field: 'projectImg', title: '项目图片', align: 'center' },
                { field: 'projectVideo', title: '项目video', align: 'center' },
                { field: 'projectCity', title: '项目城市', align: 'center' },
                { field: 'projectFirstShow', title: '项目video', align: 'center' },
                { field: 'createTime', title: '创建时间', align: 'center' },
                { field: 'lastChangeTime', title: '最后修改时间', align: 'center' }
            ]], toolbar: [{
                text: '添加',
                iconCls: 'icon-add',
                handler: function () {
                    $("#dlg").dialog().parent().appendTo("#personform");
                    $("#txtName").attr("value", '');
                    $("#txtImg").attr("value", '');
                    $("#txtdsc").attr("value", '');
                    $("#txtvideo").attr("value", '');
                    $("#txtlogo").attr("value", '');
                    $("#txtcity").attr("value", '');
                    $("#txtfirst").attr("value", '');
                    $("#hid").attr("value", '');
                    $('#dlg').dialog('open');
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
                text: '编辑楼层',
                iconCls: 'icon-add',
                handler: function () {
                    SetFloor();
                }
            }, {
                text: '编辑项目品类',
                iconCls: 'icon-add',
                handler: function () {
                    SetBrandType();
                }
            }, {
                text: '导入项目品牌',
                iconCls: 'icon-redo',
                handler: function () {
                    SetBrand();
                }
            }, {
                text: '导入项目品类',
                iconCls: 'icon-print',
                handler: function () {
                    ExportBrand();
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
            $("#dlg").dialog().parent().appendTo("#personform");
            $("#txtName").attr("value", selectedRow["projectName"]);
            $("#txtImg").attr("value", selectedRow["projectImg"]);
            $("#txtdsc").attr("value", selectedRow["projectDesc"]);
            $("#txtvideo").attr("value", selectedRow["projectVideo"]);
            $("#txtlogo").attr("value", selectedRow["projectLogo"]);
            $("#txtcity").attr("value", selectedRow["projectCity"]);
            $("#txtfirst").attr("value", selectedRow["projectFirstShow"]);
            $("#hid").attr("value", selectedRow["id"]);
            $('#dlg').dialog('open');
            //window.location = "replylist.aspx?id=" + selectedRow["id"];
        } else {
            $.messager.alert('提示', '请选中一条记录');
        }
    }
    function DelRecord() {
        $.messager.confirm('确认', '是否确认删除所选记录', function(row) {
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
                        url: '/struts/city.ashx?action=rRecord&id=' + num,
                        success: function(data) {
                            var comment = $.parseJSON(data);
                            if (comment != "suc") {
                                $.messager.alert("提示", "操作失败，请联系管理员");
                            } else {
                                $.messager.alert("提示", "您删除成功");
                                $('#persontable').datagrid('reload');
                            }
                        },
                        error: function() {
                            $.messager.alert("提示", "网络错误，请联系管理员");
                        }
                    });
                }
            }
        });
    }

    function SetFloor() {
       // $('#persontable').datagrid('selectRow', index);
        var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
        if (selectedRow) {
            //$("#hid").attr("value", selectedRow["id"]);
            //$.messager.alert('提示', '1');
            top.topManager.openPage({
                id: 'sbs',
                href:  '/files/setFloor.aspx?projectid=' + selectedRow["id"] + "&projectName=" + selectedRow["projectName"],
                title: '楼层设置'
            });
            //window.location = 'setFloor.aspx?projectid=' + selectedRow["id"] + "&projectName=" + selectedRow["projectName"];
            //$.messager.alert('提示', '2');
        } else {
            $.messager.alert('提示', '请选中一条记录');
        }
    }

    function SetBrandType() {
        var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
        if (selectedRow) {
            top.topManager.openPage({
                id: 'xmplsz',
                href: '/files/projectBrandType_query.aspx?projectId=' + selectedRow["id"] + "&projectName=" + selectedRow["projectName"],
                title: '项目品类设置'
            });
        } else {
            $.messager.alert('提示', '请选中一条记录');
        }
    }
    function SetBrand() {
        var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
        if (selectedRow) {
            top.topManager.openPage({
                id: 'xmppsz',
                href: '/files/projectExport.aspx?projectId=' + selectedRow["id"] + "&projectName=" + selectedRow["projectName"],
                title: '项目品牌设置'
            });
        } else {
            $.messager.alert('提示', '请选中一条记录');
        }
    }
    function ExportBrand() {
        var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
        if (selectedRow) {
            top.topManager.openPage({
                id: 'xmppdr',
                href: '/files/projectTypeExport.aspx?projectId=' + selectedRow["id"] + "&projectName=" + selectedRow["projectName"],
                title: '项目品类设置'
            });
        } else {
            $.messager.alert('提示', '请选中一条记录');
        }
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

                                <option value="projectName">品类名称</option>
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
        <div id="dlg" class="easyui-dialog" style="width: 600px; height: auto; padding: 10px 20px;" closed="true" buttons="#dlg-buttons" title="系统品类信息">
            <div class="ftitle">
            </div>
            <div class="fitem">
                项目名称：<input id="txtName" type="text" name="txtName" /><p></p>
            </div>
            <div class="fitem">
                项目图片：<input id="txtImg" type="text" name="txtImg" /><p></p>
            </div>
            <div class="fitem">
                项目描述：<input id="txtdsc" type="text" name="txtdsc" /><p></p>
            </div>
            <div class="fitem">
                项目视频：<input id="txtvideo" type="text" name="txtvideo" /><p></p>
            </div>
            <div class="fitem">
                项目logo：<input id="txtlogo" type="text" name="txtlogo" /><p></p>
            </div>
            <div class="fitem">
                项目城市：<input id="txtcity" type="text" name="txtcity" /><p></p>
            </div>
            <div class="fitem">
                项目首页：<input id="txtfirst" type="text" name="txtfirst" /><p></p>
            </div>
            <div class="fitem">
                <input id="hid" type="hidden" name="hid" />
                <p></p>
            </div>
            <input type="hidden" name="hidnum" id="hidnum" />
            <input type="hidden" name="hidaccount" id="hidaccount" />
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
            $('#persontable').datagrid('options').url = "/struts/city.ashx?action=query&cul=" + cul + "&where=" + encodeURI(where);
            $('#persontable').datagrid('load');
        }
        function reloaddate() {
            $("#txtwhere").attr("value", "");
            $('#persontable').datagrid('options').url = "/struts/city.ashx?action=query";
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
                    url: "/struts/city.ashx?action=opt",
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
