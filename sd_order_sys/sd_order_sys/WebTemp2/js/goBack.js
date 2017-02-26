  
    var time;  //设置一个全局变量
    var n = 30 * 1000;  //时间设置
  time = setTimeout(function () {
        location.href = "index.html";
    }, n);
    window.top.document.onmousemove = function () {
        clearTimeout(time);
        time = setTimeout(function () {
            location.href = "index.html";
        }, n);
    };
    window.top.document.onkeydown = function () {
        {
            clearTimeout(time);
            time = setTimeout(function () {
                location.href = "index.html";
            }, n);
        };
    }