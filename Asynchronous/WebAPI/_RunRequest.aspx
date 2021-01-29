<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Run Request to WebAPI</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
</head>
<body>
    <h2>Requests to ASP.NET WebAPI</h2>
    <br /><br />
    <input id="myVar" value="click start" style="width: 500px" />
    <br /><br />
    <input id="myVar2" value="click start" style="width: 500px" />

    <br /><br />
    <button id="btnStart">Start</button>
</body>
</html>

<script>
    var asyncUrl = "http://localhost:51076/api/AsynchronousApi/1?time=2000";
    var syncUrl = "http://localhost:51076/api/SynchronousApi/2?time=2000";

    window.onload = function () {
        $("#btnStart").click(function () {
            console.log("running");
            $('#myVar').val("Fetching data");
            window.fetch(asyncUrl).then(function (response) {
                console.log("running 2");
                response.text().then(function (text) {
                    console.log(text);
                    $('#myVar').val(text);
                });
            });

            $('#myVar2').val("Fetching data");
            window.fetch(syncUrl).then(function (response) {
                response.text().then(function (text) {
                    $('#myVar2').val(text);
                });
            });
        });
    };
</script>