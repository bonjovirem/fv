<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DrawPoint.aspx.cs" Inherits="sd_order_sys.files.DrawPoint" %>
<!DOCTYPE html>
<html>
<head>
    <title>关键点设置</title>
    <style>
        #div {
            width: 500px;
            height: 1px;
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
            var div = drawDot(x, y, "red", 30, 1);
            //document.body.div.innerHTML += div;
            document.getElementById('div').innerHTML += div;
            var txtArea = $("#txtArea").val();
            $("#txtArea").val(txtArea+';('+x+','+y+')');

        }
        function drawDot(x, y, color, size, index) {
            var p = new Point(x, y);
            arrPoints.push(p);
            l = arrPoints.length;
            //新建一个div
            var div = "<div id='dlist"+l+"' style='position:absolute; border:0;left:" + (x - 15) + "px; top:" + (y - 15) +
            "px; background-size: cover;cursor:pointer;background-image:url(../images/1234.png)" + ";width:" + size + "px;height:" + size + "px;line-height:30px;text-align:center;'" + "onclick='alert();'" + "><b>" + l + "</b></div>";
            return div;
        }
        function reset() {
            document.getElementById('div').innerHTML ='';
            arrPoints = new Array();
            l = arrPoints.length;
        }

    </script>

</head>
<body border='0' style="margin:0px;padding:0px;">
    <div id="ds" style="background-image:url('../projectImg/2/f1.jpg');background-size: cover;width:800px;height:600px;">
        <canvas id="ldsun" width="800" height="600" onmousedown="drawPt2();"></canvas>
    </div>
    <div id='div' onmousedown="drawPt2();"></div>
    <input onclick="drawLine();" value="连成线" type="button" />
    <input onclick="drawBallSet();" value="动画" type="button" /><input onclick="smalls();" value="变小" type="button" />

    <p>
        <input onclick="reset();" value="绘制区域" type="button" />
        <input id="txtArea" value="" type="text" />
        <input onclick="setArea();" value="确认绘制" type="button" />
    </p>
    <p></p>
    <p></p>
    <p></p>
</body>
</html>
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
            document.getElementById('dlist' + (i + 1)).style.left =( parseFloat(ax.replace('px', ''))-15) / 2 + "px";
            document.getElementById('dlist' + (i + 1)).style.top = (parseFloat(ay.replace('px', ''))-15) / 2 + "px";
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
        context.clearRect(0, 0, 800, 600);
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
                context.clearRect(0, 0, 800, 600);
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