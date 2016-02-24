var receiveTimeout = 500; // таймаут запроса текущих данных, мс

// Обновить текущие данные по таймауту
function updateCurData() {
    setTimeout(function () { receiveCurData(); }, receiveTimeout);
}

// Принять текущие данные
function receiveCurData() {
    $(function () {
        $.ajax({
            type: "GET",
            url: "AeroQuadSvc.svc/GetCurData",
            dataType: "json",

            success: function (data) {
                if (data.d == "") {
                    var msg = "Empty data received. Data service error";
                    console.log(msg);
                    showError(msg);
                } else {
                    console.log("Data received successfully");
                    console.log(data.d);
                    var curData = $.parseJSON(data.d);
                    showCurData(curData);
                }
            },

            error: function (result) {
                var msg = "Error: " + result.status + " " + result.statusText;
                console.log(msg);
                showError(msg);
            },

            complete: function () {
                updateCurData();
            }
        });
    });
}

// Отправить команду
function sendCmd(action) {
    $(function () {
        $.ajax({
            type: "GET",
            url: "AeroQuadSvc.svc/" + action,
            dataType: "json",

            success: function (data) {
                console.log("Command '" + action + "' has been sent successfully");
            },

            error: function (result) {
                console.log("Error: " + result.status + " " + result.statusText);
                showError();
            }
        });
    });
}

// Отобразить текущие данные на веб-форме
function showCurData(curData) {
    var len = curData.length;
    var i = 0;

    $(".container .tag input:text").each(function () {
        var textBox = $(this);
        var val = i < len ? curData[i] : "";
        textBox.val(val);
        textBox.css("color", getColor(val));
        i++;
    });

    $("#divStatus").text("Received: " + (new Date().toLocaleTimeString()));
}

// Получить цвет для отображения значения
function getColor(val) {
    if (val == "Yes")
        return "green";
    else if (val == "No")
        return "red";
    else if (val == "On")
        return "green";
    else if (val == "Off")
        return "orange";
    else
        return "black";
}

// Вывести сообщение об ошибке
function showError(msg) {
    $("#divStatus").text(msg);
}

$(document).ready(function () {
    updateCurData();

    $("#btnStartReading").click(function (event) {
        event.preventDefault();
        sendCmd("StartReading");
    });

    $("#btnRecordOn").click(function (event) {
        event.preventDefault();
        sendCmd("RecordOn");
    });

    $("#btnRecordOff").click(function (event) {
        event.preventDefault();
        sendCmd("RecordOff");
    });
});