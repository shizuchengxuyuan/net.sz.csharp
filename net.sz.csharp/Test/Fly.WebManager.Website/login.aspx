<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Fly.WebManager.Website.login" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" href="css/login.css">
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <title>后台登陆</title>
</head>
<body>
    <form runat="server">

        <div id="login_center">
            <div id="login_area">
                <div id="login_form">
                    <div id="login_tip">
                        用户登录&nbsp;&nbsp;UserLogin
                    </div>
                    <div>
                        <input type="text" class="username" /></div>
                    <div>
                        <input type="text" class="pwd" /></div>
                    <div id="btn_area">
                        <asp:Button ID="sub_btn" runat="server" Text="登&nbsp;&nbsp;录" />&nbsp;&nbsp;
                        <asp:TextBox ID="tbyzm" TextMode="SingleLine" class="verify" runat="server"></asp:TextBox>
                        <img src="images/login/verify.png" alt="" width="80" height="40" />
                    </div>
                </div>
            </div>
        </div>
        <div id="login_bottom">
            版权所有 飞颖网络工作室
        </div>
    </form>
</body>
</html>
