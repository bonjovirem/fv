﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="project_query.aspx.cs" Inherits="sd_order_sys.files.project_query" %>

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
        .select {
        }
        .select1 {
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
            url: '../struts/project.ashx?action=query',
            //queryParams:{},  
            loadMsg: '数据加载中请稍后……',
            pagination: true,
            rownumbers: true,
            columns: [[
                { field: 'ck', checkbox: true, align: 'center' },
                { field: 'brandName', title: '品牌名称', align: 'center' },
                { field: 'brandImg', title: '品牌图片', align: 'center', formatter: formatUploadFile },
                { field: 'brandDesc', title: '品牌描述', align: 'center' },
                { field: 'brandVideo', title: '品牌视频', align: 'center' },
                { field: 'brandLogo', title: '品牌图标', align: 'center' },
                { field: 'brandOrder', title: '品类顺序', align: 'center' },
                { field: 'brandTypeName', title: '所属品类名称', align: 'center' },
                { field: 'isShow', title: '是否展示', align: 'center', formatter: formatString },
                { field: 'isStar', title: '是否标星', align: 'center', formatter: formatString },
                { field: 'isShowWay', title: '是否显示路线', align: 'center', formatter: formatString },
                { field: 'fvUrl', title: '全景地址', align: 'center' },
                { field: 'createTime', title: '创建时间', align: 'center' },
                { field: 'lastChangeTime', title: '最后修改时间', align: 'center' }
            ]], toolbar: [{
                text: '添加',
                iconCls: 'icon-add',
                handler: function () {
                    if ($("#hidname").val() == "") {
                        $.messager.alert('提示', '无任何品类信息，请先编辑品类');
                        return;
                    }
                    $("#dlg").dialog().parent().appendTo("#personform");
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
    function formatString(val, row, index) {
        if (val == 0)
            return "否";
        else
            return "是";
    }
    function formatUrl(val, row, index) {
        if (val) {
            return '<a href="' + val + '" title="查看全景" target="_blank">查看全景</a>';
        }
        else
            return "";
    }
    //function formateTime(val, row, index) {
    //    if (val) {
    //        var dateTimeJsonStr = val;
    //        var msecStr = dateTimeJsonStr.toString().replace(/\/Date\(([-]?\d+)\)\//gi, "$1");
    //        var msesInt = Number(msecStr);
    //        var dt = new Date(msesInt);
    //        return dt.toLocaleString();
    //    }
    //    else {
    //        return "";
    //    }
    //}
    //function formatOper(val, row, index) {
    //    return '<a href="javascript:void(0);", onclick="rowMark(' + index + ')">查看</a>';
    //};
    function rowMark(index) {
        $('#persontable').datagrid('selectRow', index);
        var selectedRow = $('#persontable').datagrid('getSelected');  //获取选中行
        if (selectedRow) {
            $("#dlg").dialog().parent().appendTo("#personform");
            $("#txtName").attr("value", selectedRow["brandName"]);
            // $("#txtImg").attr("value", selectedRow["brandImg"]);
            $("#txtdsc").attr("value", selectedRow["brandDesc"]);
            $("#txtvideo").attr("value", selectedRow["brandVideo"]);
            $("#txtlogo").attr("value", selectedRow["brandLogo"]);
            $("#brandOrder").attr("value", selectedRow["brandOrder"]);
            alert(selectedRow["brandTypeName"]);
            $("#brandTypeName").find("option[text='" + selectedRow["brandTypeName"] + "']").attr("selected", true);
            $("#isShow").find("option[value='" + selectedRow["isShow"] + "']").attr("selected", true);
            $("#isStar").find("option[value='" + selectedRow["isStar"] + "']").attr("selected", true);
            $("#isShowWay").find("option[value='" + selectedRow["isShowWay"] + "']").attr("selected", true);
            $("#hid").attr("value", selectedRow["id"]);
            $('#dlg').dialog('open');
            //window.location = "replylist.aspx?id=" + selectedRow["id"];
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
                        url: '/struts/project.ashx?action=rRecord&id=' + num,
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

                                <option value="brandName">品类名称</option>
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
                品牌名称：<input id="txtName" type="text" name="txtName" /><p></p>
            </div>
            <div class="fitem">
                品牌图片：<input id="txtImg" type="text" name="txtImg" onchange="document.getElementById('hidimg').value=this.value" /><input id="hidimg" type="text" name="hidimg" /><p></p>
            </div>
            <div class="fitem">
                品牌描述：<input id="txtdsc" type="text" name="txtdsc" /><p></p>
            </div>
            <div class="fitem">
                品牌视频：<input id="txtvideo" type="text" name="txtvideo" /><p></p>
            </div>
            <div class="fitem">
                品牌logo：<input id="txtlogo" type="text" name="txtlogo" /><p></p>
            </div>
            <div class="fitem">
                顺序：<input id="brandOrder" type="text" name="brandOrder" /><p></p>
            </div>
            <div class="fitem">
                所属品类：<select id="brandTypeName" runat="server" onchange="getText()" name="brandTypeName">
                    <option></option>

                </select><input id="hidname" type="hidden" name="hidname" runat="server" /><p></p>
            </div>
            <div class="fitem">
                是否展示：<select id="isShow" name="isShow">
                    <option value="0">否</option>
                    <option value="1">是</option>
                </select><p></p>
            </div>
            <div class="fitem">
                是否标星：<select id="isStar" name="isStar">
                    <option value="0">否</option>
                    <option value="1">是</option>
                </select><p></p>
            </div>
            <div class="fitem">
                是否展示：<select id="isShowWay" name="isShowWay">
                    <option value="0">否</option>
                    <option value="1">是</option>
                </select><p></p>
            </div>
            <div class="fitem">
                全景地址：<input id="fvUrl" type="text" name="fvUrl" /><p></p>
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
            $('#persontable').datagrid('options').url = "/struts/project.ashx?action=query&cul=" + cul + "&where=" + encodeURI(where);
            $('#persontable').datagrid('load');
        }
        function reloaddate() {
            $("#txtwhere").attr("value", "");
            $('#persontable').datagrid('options').url = "/struts/project.ashx?action=query";
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
                    url: "/struts/project.ashx?action=opt",
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
        function getText() {
            var text = $("#brandTypeName").find("option:selected").text();
            $("#hidname").attr("value", text);
        }
    </script>
</body>
</html>