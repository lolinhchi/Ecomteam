function getUrl() {
    return window.location.pathname.split("/");
}
function isLogin() {
    var [domain, controller, method] = getUrl();
    if (!(controller == "Login" && method == "Index")) {
        return window.location.href = "/Login/Index";
    }
}
function PrintBarCode() {
    var data = new Array();
    $.each(this.ViewBag.Result, function (key, value) {
        var ar = new Array();
        $.each(value, function (k, v) {
            ar[v.Key] = v.Value;
        });
        data[key] = ar;
    });
    $.each(data, function (k, v) {
        JsBarcode("#barcode_" + v.ProductId, v.Barcode, {
            format: "EAN13"
        });
    });
}

function getParam(param) {
    var url = new URL(document.URL);
    return url.searchParams.get(param);
}
function noti(text, type = 'success') {
    $.notify(text, {
        className: type,
        position: 'bottom right'
    });
}

function Format(i) {
    return Intl.NumberFormat().format(i);
}
function openPopup({url = "",param = {w : 800, h : 800, top : 0, bottom : 0 }, isPopup = true }) {
    var pa = url.indexOf("?") == -1 ? "?" : "";
    window.open(url+pa +"&isPopup="+isPopup, "", "width=" + param.w + ",height=" + param.h + ",top=" + param.top + "bottom=" + param.bottom);
}

function sendMessage( IsSuccess ) {
    Message = IsSuccess ? "Thành Công":  "Failed";
    
    swal({
        title: 'THÔNG BÁO!',
        text: Message,
        type: IsSuccess == true ? 'success' : 'error',
        showCancelButton: false,
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Xác nhận!'
    }).then((result) => {
        window.location.reload();
    })
}
function callback(result) {
    window.close();
    window.opener.sendMessage(result);
}
function btnLogin() {
    var username = $("#inputEmail").val(),
        password = $("#inputPassword").val();
    if (!username || !password) {
        noti("Không được bỏ trống!", 'error');
        return;
    }
    $.post(
        '/login/CheckLogin',
        {
            Username: username,
            Password : password
        },
        function (result) {
            if (result.IsSuccess) {
                noti(result.Message);
                document.body.innerHTML = "Đang chuyển hướng trang, chờ trong giây lát!";
                window.location.href = '/Home/Index';
            }
            else {
                noti(result.Message, 'error');
            }
        }
    );
}

function btnLogout() {
    $.post(
        '/Login/Logout',
        function (result) {
            if (result.IsSuccess) {
                window.location.href = "/Login/Index";
            }
        }
    );
}


function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    if (googleUser === true) {

    } else {
        googleUser.disconnect();
    }
    $.post(
        '/Login/OAuth',
        {
            data: {
                data: {
                    Id: parseInt(profile.getId()),
                    Email: profile.getEmail(),
                    Name: profile.getName(),
                    Photo: profile.getImageUrl(),
                    Token: googleUser.Zi.access_token
                }
            }
        },
        function (result) {
            if (result.IsSuccess) {
                document.body.innerHTML = "Đang chuyển hướng, xin chờ trong giây lát!";
                window.location.href = '/Home/Index';
            }
            else {
                noti(result.Message, 'error');
            }
        }
    );
}
function SalePrintPDF() {
    var data = [];
    $.each(this.ViewBag.Result, function (key, row) {
        var ar = new Object();
        $.each(row, function (k, v) {
            if (v.Key == "SaleDate" || v.Key == "CustomerName" || v.Key == "EmployeeName" || v.Key == "Qty" || v.Key == "Total" || v.Key == "Note") {
                ar[v.Key] = v.Value;
            }
        });
        data.push(ar);
    });
    var json = JSON.stringify(data);
    console.log(json);
    printJS({
        printable: JSON.parse(json),
        properties: ['SaleDate', 'CustomerName', 'EmployeeName','Qty','Total','Note'],
        type: 'json'
    });
}

