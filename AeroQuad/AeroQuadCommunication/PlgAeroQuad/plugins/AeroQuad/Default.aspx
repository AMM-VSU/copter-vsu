<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Scada.Web.plugins.AeroQuad.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server">
    <div>
        <asp:Button 
            ID="btnStartReading" runat="server" Text="Start Reading" /><asp:Button 
            ID="btnRecordOn" runat="server" Text="Record On" /><asp:Button 
            ID="btnRecordOff" runat="server" Text="Record Off" />
    </div>
    <div>
        <span>Connection</span><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Record</span><asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Received messages</span><asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Failed messages</span><asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Messages per second</span><asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Roll Gyro Rate</span><asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Pitch Gyro Rate</span><asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Yaw Gyro Rate</span><asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Accel X Axis</span><asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Accel Y Axis</span><asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Accel Z Axis</span><asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Mag Raw Value X Axis</span><asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Mag Raw Value Y Axis</span><asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>Mag Raw Value Z Axis</span><asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
    </div>
    </form>
</body>
</html>
