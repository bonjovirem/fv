  
    var time;  //����һ��ȫ�ֱ���
    var n = 30 * 1000;  //ʱ������
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