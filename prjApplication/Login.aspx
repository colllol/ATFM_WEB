<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="prjApplication.Login" EnableEventValidation="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="vi">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="Đăng nhập hệ thống quản lý luồng không lưu" />
    <title>Đăng nhập | VATM</title>

    <link rel="stylesheet" href="Style/assets/font-awesome/4.5.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Style/login-modern.css" />
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btLogon">
        <main class="login-page">
            <section class="brand-panel" aria-label="Giới thiệu VATM">
                <div class="brand-content">
                    <div class="brand-lockup">
                        <img src="Images/logo.png" alt="Logo VATM" class="brand-logo" />
                        <div class="brand-divider" aria-hidden="true"></div>
                        <div class="brand-name">
                            <strong>TỔNG CÔNG TY QUẢN LÝ BAY VIỆT NAM</strong>
                            <span>VIETNAM AIR TRAFFIC MANAGEMENT CORPORATION</span>
                        </div>
                    </div>

                    <div class="brand-message">
                        <span class="eyebrow">HỆ THỐNG NGHIỆP VỤ</span>
                        <h1>Air Traffic Flow<br />Management</h1>
                        <p>Quản lý luồng không lưu an toàn, hiệu quả và hiện đại.</p>
                    </div>
                </div>

                <div class="radar radar-one" aria-hidden="true"></div>
                <div class="radar radar-two" aria-hidden="true"></div>
                <div class="route-line" aria-hidden="true"></div>
            </section>

            <section class="login-panel">
                <div class="login-card">
                    <div class="mobile-brand">
                        <img src="Images/logo.png" alt="Logo VATM" />
                        <div>
                            <strong>VATM</strong>
                            <span>Vietnam Air Traffic Management Corporation</span>
                        </div>
                    </div>

                    <header class="login-header">
                        <span class="welcome-label">CHÀO MỪNG TRỞ LẠI</span>
                        <h2>Đăng nhập hệ thống</h2>
                        <p>Vui lòng nhập thông tin tài khoản để tiếp tục.</p>
                    </header>

                    <div class="form-group">
                        <label for="txtUserName">Tên đăng nhập</label>
                        <div class="input-wrap">
                            <i class="fa fa-user" aria-hidden="true"></i>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control"
                                placeholder="Nhập tên đăng nhập" autocomplete="username"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtPassWord">Mật khẩu</label>
                        <div class="input-wrap">
                            <i class="fa fa-lock" aria-hidden="true"></i>
                            <asp:TextBox ID="txtPassWord" runat="server" TextMode="Password" CssClass="form-control"
                                placeholder="Nhập mật khẩu" autocomplete="current-password"></asp:TextBox>
                            <button type="button" class="password-toggle" id="togglePassword"
                                aria-label="Hiện mật khẩu" aria-pressed="false">
                                <i class="fa fa-eye" aria-hidden="true"></i>
                            </button>
                        </div>
                    </div>

                    <div class="form-options">
                        <label class="remember-me">
                            <asp:CheckBox runat="server" ID="chkRemember" />
                            <span>Ghi nhớ đăng nhập</span>
                        </label>
                    </div>

                    <asp:LinkButton runat="server" ID="btLogon" CssClass="login-button"
                        OnClick="btLogon_Click">
                        <span>Đăng nhập</span>
                        <i class="fa fa-arrow-right" aria-hidden="true"></i>
                    </asp:LinkButton>

                    <p class="support-text">
                        <i class="fa fa-shield" aria-hidden="true"></i>
                        Hệ thống nội bộ dành cho người dùng được cấp quyền
                    </p>
                </div>

                <footer class="login-footer">
                    &copy; <span id="currentYear"></span> Vietnam Air Traffic Management Corporation
                </footer>
            </section>
        </main>
    </form>

    <script type="text/javascript">
        (function () {
            var password = document.getElementById('<%= txtPassWord.ClientID %>');
            var toggle = document.getElementById('togglePassword');
            var year = document.getElementById('currentYear');

            if (year) year.innerHTML = new Date().getFullYear();

            if (password && toggle) {
                toggle.onclick = function () {
                    var showPassword = password.type === 'password';
                    password.type = showPassword ? 'text' : 'password';
                    toggle.setAttribute('aria-pressed', showPassword ? 'true' : 'false');
                    toggle.setAttribute('aria-label', showPassword ? 'Ẩn mật khẩu' : 'Hiện mật khẩu');
                    toggle.firstElementChild.className = showPassword ? 'fa fa-eye-slash' : 'fa fa-eye';
                    password.focus();
                };
            }
        })();
    </script>
</body>
</html>
