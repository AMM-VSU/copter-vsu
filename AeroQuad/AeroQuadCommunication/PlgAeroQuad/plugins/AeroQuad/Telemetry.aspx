<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Telemetry.aspx.cs" Inherits="Scada.Web.Plugins.AeroQuad.WFrmTelemetry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>AeroQuad - Rapid SCADA</title>

    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="css/aeroquad.css" rel="stylesheet" />

    <script src="js/jquery-1.12.1.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/aeroquad.js"></script>
</head>
<body role="document">
    <form runat="server">
    <nav class="navbar navbar-inverse navbar-fixed-top">
      <div class="container">
        <div class="navbar-header">
          <a class="navbar-brand" href="#">AeroQuad - Rapid SCADA</a>
        </div>
      </div>
    </nav>

    <div class="container" role="main">
        <div>
            <asp:Button 
                ID="btnStartReading" runat="server" Text="Start Reading" CssClass="btn btn-primary" /><asp:Button 
                ID="btnRecordOn" runat="server" Text="Record On" CssClass="btn btn-success"/><asp:Button 
                ID="btnRecordOff" runat="server" Text="Record Off" CssClass="btn btn-warning" />
        </div>
        <div class="tag">
            <span>Connection</span><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Record</span><asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Received messages</span><asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Failed messages</span><asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Messages per second</span><asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Roll Gyro Rate</span><asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Pitch Gyro Rate</span><asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Yaw Gyro Rate</span><asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Accel X Axis</span><asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Accel Y Axis</span><asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Accel Z Axis</span><asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Mag Raw Value X Axis</span><asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Mag Raw Value Y Axis</span><asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
        </div>
        <div class="tag">
            <span>Mag Raw Value Z Axis</span><asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
        </div>
        <div id="divStatus">
        </div>
    </div>
    </form>
</body>
</html>
