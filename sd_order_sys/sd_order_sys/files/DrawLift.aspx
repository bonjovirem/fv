<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DrawLift.aspx.cs" Inherits="sd_order_sys.files.DrawLift" %>

<!DOCTYPE html>
<html>
<head>
    <title>电梯点设置</title>
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
</head>
<body border='0' style="margin: 0px; padding: 0px;">
    <form id="personform">
        <div id="ds" style="background-image: url('../projectImg/<%=projectId %>/f<%=floorLevel %>.png'); background-size: cover; width: 1300px; height: 824px;">
            <canvas id="ldsun"  width="1300" height="824"  onmousedown="drawPt2();"></canvas>
        </div>
        <div id='div' onmousedown="drawPt2();"></div>
        <p>
            <input onclick="reClear();" value="清空" type="button" />
            <input id="clientPoint" name="clientPoint" value="" type="hidden" />
            <input id="btnsub" onclick="AddClient();" value="确认" type="button" />
            <input type="hidden" name="floorId" id="floorId" value="<%=floorId %>" />
            <input type="hidden" name="floorLevel" id="floorLevel" value="<%=floorLevel %>" />
            <input type="hidden" name="projectId" id="projectId" value="<%=projectId %>" />
            <input type="hidden" name="hid" id="hid" value="<%=clientId %>" />
        </p>
       
    
    </form>
</body>
</html>
    <script language="javascript" type="text/javascript">
        var arrPoints = new Array();
        var l = arrPoints.length;

        function drawPt3(x, y) {
            var div = drawDot(x, y, "red", 30, 1, 'C');
            //document.body.div.innerHTML += div;
            if (l > 1) {
                return;
            }
            document.getElementById('div').innerHTML += div;
            var txtArea = $("#clientPoint").val();
            if (txtArea == '') {
                $("#clientPoint").val(txtArea + '(' + x + ',' + y + ')');
            } else {
                $("#clientPoint").val(txtArea + ';(' + x + ',' + y + ')');
            }

        }
        function drawPt2() {
            var x = event.offsetX;
            var y = event.offsetY;
            drawPt3(x, y);
        }
        function drawDot(x, y, color, size, index, sb) {
            var p = new Point(x, y);
            arrPoints.push(p);
            l = arrPoints.length;
            //新建一个div
            var div = "<div id='dlist" + l + "' style='position:absolute; border:0;left:" + (x - 15) + "px; top:" + (y - 15) +
            "px; background-size: cover;cursor:pointer;background-image:url(../images/1234.png)" + ";width:" + size + "px;height:" + size + "px;line-height:30px;text-align:center;'" + "onclick='alert();'" + "><b>" + sb + "</b></div>";
            return div;
        }
        function reClear() {
            document.getElementById('div').innerHTML = '';
            $("#clientPoint").val('');
            arrPoints = new Array();
            l = arrPoints.length;
        }

        function AddClient() {
            $("#btnsub").attr("disabled", "disabled");
            $.ajax({
                type: "POST",
                url: "/struts/DrawLift.ashx?action=add",
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
        <% =strForShow %>
    </script>