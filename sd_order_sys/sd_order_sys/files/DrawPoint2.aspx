﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DrawPoint2.aspx.cs" Inherits="sd_order_sys.files.DrawPoint2" %>

<!DOCTYPE html>
<html>
<head>
    <title>关键点设置</title>
    <script type="text/javascript" src="../js/jquery-1.8.2.min.js"></script>
      <link rel="stylesheet" type="text/css" href="../plugins/themes/metro/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../plugins/themes/icon.css" />
    <script type="text/javascript" src="../plugins/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../plugins/locale/easyui-lang-zh_CN.js"></script>
    <style>
        #div {
            width: 500px;
            height: 0px;
            border-color: #FF0000;
            border-width: 0.5em;
            background-color: #CCCCCC;
        }

        pt {
            background: #CC3300;
            border-top: 6px solid #FFFFCC;
            border-left: 6px solid #FF3300;
            border-bottom: 6px solid #FFFFCC;
            overflow: hidden;
            float: left;
        }
    </style>
    <script type="text/javascript">
        function Point(x, y) {
            this.x = x;
            this.y = y;
        };
    </script>
    <script language="javascript" type="text/javascript">
        var arrPoints = new Array();
        var l = arrPoints.length;
        function drawPt2() {
            var x = event.offsetX;
            var y = event.offsetY;
            var sb = "";
            if (l == 0) {
                sb = "机";
            } else if (l == 1) {
                sb = "梯";
            } else {
                return;
            }
            var div = drawDot(x, y, "red", 30, 1,sb);
            //document.body.div.innerHTML += div;
            document.getElementById('div').innerHTML += div;
            var txtArea = $("#txtArea").val();
            if (txtArea == '') {
                $("#txtArea").val(txtArea + '(' + x + ',' + y + ')');
            } else {
                $("#txtArea").val(txtArea + ';(' + x + ',' + y + ')');
            }

        }
        function drawDot(x, y, color, size, index,sb) {
            var p = new Point(x, y);
            arrPoints.push(p);
            l = arrPoints.length;
            //新建一个div
            var div = "<div id='dlist" + l + "' style='position:absolute; border:0;left:" + (x - 15) + "px; top:" + (y - 15) +
            "px; background-size: cover;cursor:pointer;background-image:url(../images/1234.png)" + ";width:" + size + "px;height:" + size + "px;line-height:30px;text-align:center;'" + " " + "><b>" + sb + "</b></div>";
            return div;
        }
        function reset() {
            document.getElementById('div').innerHTML = '';
            arrPoints = new Array();
            l = arrPoints.length;
        }

        function AddArea() {
            $.ajax({
                type: "POST",
                url: "/struts/Drawpoint.ashx?action=add",
                data: $('#personform').serialize(),
                datatype: "json",
                success: function (data) {
                    var comment = $.parseJSON(data);
                    if (comment != "suc") {
                        $.messager.alert(comment.msg);
                    } else {
                        $.messager.confirm('确认', '操作成功，是否关闭当前页？', function (row) {
                            if (row) {
                                window.close();
                            }
                        });
                        //window.close();
                    }
                },
                //调用出错执行的函数
                error: function () {
                    $.messager.alert("提示", "网络错误，请联系管理员");
                }
            });
        }
    </script>

</head>
<body border='0' style="margin: 0px; padding: 0px;">
    <form id="personform">
        <div id="ds" style="background-image: url('../projectImg/2/f1.jpg'); background-size: cover; width: 1300px; height: 824px;">
            <canvas id="ldsun"  width="1300" height="824"  onmousedown="drawPt2();"></canvas>
        </div>
        <div id='div' onmousedown="drawPt2();"></div>
       <label id="typeShow" name="typeShow"></label>
            <input type="hidden" name="hidFloorId" id="hidFloorId" value="<%=hidFloorId %>" />
        <p>
            <input onclick="reset();" value="绘制client和最近电梯点" type="button" />
            <input id="txtClient" name="txtArea" value="" type="hidden" />
            <input onclick="AddClient();" value="确认绘制" type="button" />
        </p>
         <p>
            <input onclick="reset();" value="绘制client" type="button" />
            <input id="txtClient" name="txtArea" value="" type="hidden" />
            <input onclick="AddClient();" value="确认绘制" type="button" />
        </p>
    
    </form>
</body>
</html>
