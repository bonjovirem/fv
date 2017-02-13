<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DrawLiftPath.aspx.cs" Inherits="sd_order_sys.files.DrawLiftPath" %>

<!DOCTYPE html>
<html>
<head>
    <title>设置电梯路径</title>
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
        <div id="ds" style="background-image: url('../projectImg/<% =projectId  %>/f<% =floorLevel  %>.png'); background-size: cover; width: 1300px; height: 824px;">
            <canvas id="ldsun" width="1300" height="824"  onmousedown="drawPt2();"></canvas>
        </div>
        <div id='div' onmousedown="drawPt2();"></div>
        <p>
            <%-- <select id="floorLevel" runat="server" onchange="getText()" name="floorLevel">
                    <option></option>
                </select>--%>
            <input onclick="reClear();" value="重新绘制" type="button" />
            <%--<input onclick="" value="连成线" type="button" />--%>
            <input onclick="drawLine(); drawBallSet();" value="动画" type="button" />
            <input id="txtArea" name="txtArea" value="" type="hidden" />
            <input id="btnsub" onclick="AddPath();" value="确认绘制" type="button" />
            <input type="hidden" name="hidFloorId" id="hidFloorId" runat="server" />
            <input type="hidden" name="projectId" id="projectId" value="<%=projectId %>" />
            <input type="hidden" name="projectBrandId" id="projectBrandId" value="<%=projectBrandId %>" />
            <input type="hidden" name="floorLevel" id="floorLevel" value="<%=floorLevel %>" />
            <input type="hidden" name="clientId" id="clientId" value="<%=clientId %>" />
        </p>


    </form>
</body>
</html>
<script language="javascript" type="text/javascript">
    var arrPoints = new Array();
    var l = arrPoints.length;

    function drawPt3(x, y) {
        var div = drawDot(x, y, "red", 30, 1);
        //document.body.div.innerHTML += div;
        document.getElementById('div').innerHTML += div;
        var txtArea = $("#txtArea").val();
        if (txtArea == '') {
            $("#txtArea").val(txtArea + '(' + x + ',' + y + ')');
        } else {
            $("#txtArea").val(txtArea + ';(' + x + ',' + y + ')');
        }
    }
    function drawPt2() {
        var x = event.offsetX;
        var y = event.offsetY;
        drawPt3(x, y);
    }
    function drawDot(x, y, color, size, index) {
        var p = new Point(x, y);
        arrPoints.push(p);
        l = arrPoints.length;
        //新建一个div
        var div = "<div id='dlist" + l + "' style='position:absolute; border:0;left:" + (x - 15) + "px; top:" + (y - 15) +
        "px; background-size: cover;cursor:pointer;background-image:url(../images/1234.png)" + ";width:" + size + "px;height:" + size + "px;line-height:30px;text-align:center;'" + "onclick='alert();'" + "><b>" + l + "</b></div>";
        return div;
    }
    function reClear() {
        document.getElementById('div').innerHTML = '';
        $("#txtArea").val('');
        arrPoints = new Array();
        l = arrPoints.length;
        clearInterval(interVal);
        context.clearRect(0, 0, 1300,824);
         <% =strForClient %>
    }

    function AddPath() {
        l = arrPoints.length;
        if (l<2) {
            $.messager.alert("提示", "一个点太少了，至少设置两个点！");
            return;
        }
        $("#btnsub").attr("disabled", "disabled");
        $.ajax({
            type: "POST",
            url: "/struts/DrawLiftPath.ashx?action=add",
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
    function getText() {
        var val = $("#floorLevel").find("option:selected").val();
        $("#hidFloorId").attr("value", val);
        reClear();
        $('#ds').css('background-image', 'url(../projectImg/<% =projectId  %>/f' + val + '.png)');
    }
    <% =strForShow %>
</script>
<script language="javascript" type="text/javascript">
    //document.getElementById('ldsun').style.display = 'none';
    var canvas = document.querySelector('canvas');
    context = canvas.getContext('2d');
    // w = canvas.width = window.innerWidth;
    // h = canvas.height = window.innerHeight;
    var x = y = 0;
    var maxX = 200,
         maxY = 200;
    function smalls() {

        document.getElementById('ds').style.width = '400px';
        document.getElementById('ds').style.height = '300px';
        var canvas = document.querySelector('canvas');
        canvas.width = 400;
        canvas.height = 300;
        var qx = canvas.width / 800;
        var qy = canvas.height / 600;
        for (var i = 0; i < arrPoints.length; i++) {
            arrPoints[i] = new Point(arrPoints[i].x * qx, arrPoints[i].y * qy);
            ax = document.getElementById('dlist' + (i + 1)).style.left;
            ay = document.getElementById('dlist' + (i + 1)).style.top;
            document.getElementById('dlist' + (i + 1)).style.left = (parseFloat(ax.replace('px', '')) - 15) / 2 + "px";
            document.getElementById('dlist' + (i + 1)).style.top = (parseFloat(ay.replace('px', '')) - 15) / 2 + "px";
        }

    }
    function drawLine() {
        context.strokeStyle = "red";
        context.fillStyle = "blue";
        context.lineWidth = 2;
        // document.getElementById('div').style.display = 'none';
        //  document.getElementById('ldsun').style.display = ' ';
        context.beginPath();
        for (var i = 0; i < arrPoints.length; i++) {
            if (i == 0) {
                context.moveTo(arrPoints[i].x, arrPoints[i].y);
            } else {
                context.lineTo(arrPoints[i].x, arrPoints[i].y);
            }
        }
        context.stroke();
        context.closePath();
        //context.moveTo(80, 80);
        //context.lineTo(20, 100);
        //context.lineTo(70, 100);
        //context.lineTo(70, 130);
        //context.stroke();
    }

    var p1 = new Point();
    var p2 = new Point();
    var lx = 0;
    var ly = 0;
    var stepX = 1;
    var stepY = 1;
    var indexS = 0;
    var interVal = 0.0;
    var faqus = 0;
    function drawBall() {
        faqus = 300 / arrPoints.length;
        context.clearRect(0, 0, 1300,824);
        drawLine();
        p1 = arrPoints[indexS];
        p2 = arrPoints[indexS + 1];
        stepX = (p2.x - p1.x) / faqus;
        stepY = (p2.y - p1.y) / faqus;
        lx = lx + stepX;
        ly = ly + stepY;
        context.strokeStyle = "blue";
        context.fillStyle = "blue";
        context.beginPath();
        context.arc(p1.x + lx, p1.y + ly, 12, 0, Math.PI * 2, 1); //x坐标，y坐标，半径，Math.PI是圆周率
        context.stroke();
        context.closePath();
        context.fill();
        if (Math.abs(p1.x + lx - p2.x) < Math.abs(stepX * 1.2) || Math.abs(p1.y + ly - p2.y) < Math.abs(1.2 * stepY)) { //判断当前线的位置，控制在区域内
            if (indexS >= arrPoints.length - 2) {
                context.clearRect(0, 0, 1300,824);
                drawLine();
                context.strokeStyle = "blue";
                context.fillStyle = "blue";
                context.beginPath();
                context.arc(p2.x, p2.y, 12, 0, Math.PI * 2, 1); //x坐标，y坐标，半径，Math.PI是圆周率
                context.stroke();
                context.closePath();
                context.fill();
                clearInterval(interVal);
            }
            indexS = indexS + 1;
            lx = 0;
            ly = 0;
            stepX = 1;
            stepY = 1;
        }
    }
    function drawBallSet() {
        indexS = 0;
        lx = 0;
        ly = 0;
        interVal = setInterval(drawBall, 10);
    }
    //context.moveTo(0, 0);
    //context.lineTo(20, 100);
    //context.lineTo(70, 100);
    //context.lineTo(70, 130);

    //method 1
    function draw() {
        if (x < maxX && y <= maxY) {
            context.lineTo(x += 10, y += 10);
            context.stroke();
        } else {
            clearInterval(interVal);
        }
    }
    //method 2
    function drawFrame() {
        interVal = window.requestAnimationFrame(drawFrame, canvas);
        if (x < maxX && y <= maxY) {
            context.lineTo(x += 10, y += 10);
            context.stroke();
        } else {
            window.cancelAnimationFrame(interVal);
        }
    }
    //context.moveTo(x, y);
</script>
