function loadPanelDesc(brandId) {
    var Arrayfv = new Array();
    var ArrayDesc = new Array();
    var ArrayTele = new Array();
    var ArrayAddress = new Array();
    var ArrayLogo = new Array();
    var ArrayQrCode = new Array();
    //*fvString
    //*descString
    //*telephoneString
    //*addressString
    //*brandLogo
    //*brandQrCode
    document.getElementById('hidBrandId').value = brandId;
    document.getElementById('divfv').innerHTML = "<iframe id='iffv' width='100%' height='100%' data-label='quanjing' scrolling='no' frameborder='0' webkitallowfullscreen='' mozallowfullscreen='' allowfullscreen='' src='" + Arrayfv[brandId] + "'></iframe>";
    document.getElementById('divlogo').innerHTML = " <img src='" + ArrayLogo[brandId] + "' style='padding:3px;height:120px;'/>";
    document.getElementById('divDesc').innerHTML = ArrayDesc[brandId];
    document.getElementById('divTele').innerHTML = '电话：' + ArrayTele[brandId];
    document.getElementById('divAddress').innerHTML = '地址：' + ArrayAddress[brandId];
    document.getElementById('divImg').innerHTML = " <img src='" + ArrayQrCode[brandId] + "' style='padding:0px 10px;height:190px;'  />";
}