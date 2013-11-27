<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MYDIGIPASS.COM._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MYDIGIPASS.COM</title>
    <script type="text/javascript" src="https://static.mydigipass.com/en/dp_connect.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a class="dpplus-connect" data-style="large" data-help = "true" data-client-id="{client-id}" data-redirect-uri="{redirect-uri}" data-origin="{origin}" href="#">Connect with MYDIGIPASS.COM</a>
        <br />
        <br />
        

        <asp:Button id="_btnConnectedUsers" runat="server" Text="ConnectedUsers" 
            Enabled="false" onclick="_btnConnectedUsers_Click" />
        <br />
        <br />
        <asp:Button id="_btnDisconnectUser" runat="server" 
            Text="Disconnect from MYDP.com" Enabled="false" 
            onclick="_btnDisconnectUser_Click"/>
        <br />
        <br />
        <asp:Label ID="_lblAction" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
