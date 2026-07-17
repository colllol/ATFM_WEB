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
    <style type="text/css">
        .login-loading-overlay { position:fixed; z-index:99999; inset:0; display:flex; visibility:hidden; align-items:center; justify-content:center; background:rgba(231,241,249,.72); opacity:0; pointer-events:none; -webkit-backdrop-filter:blur(4px); backdrop-filter:blur(4px); transition:opacity .18s ease,visibility .18s ease; }
        .login-loading-overlay.is-visible { visibility:visible; opacity:1; pointer-events:all; }
        .login-loading-box { min-width:160px; padding:24px 30px; border:1px solid rgba(51,122,183,.2); border-radius:15px; background:rgba(255,255,255,.96); text-align:center; box-shadow:0 18px 45px rgba(22,63,95,.2); }
        .login-loading-spinner { position:relative; width:50px; height:50px; margin:0 auto 14px; }
        .login-loading-spinner:before,.login-loading-spinner:after { content:""; position:absolute; inset:0; border:4px solid transparent; border-radius:50%; }
        .login-loading-spinner:before { border-top-color:#337ab7; border-right-color:#337ab7; animation:loginSpin .8s linear infinite; }
        .login-loading-spinner:after { inset:9px; border-bottom-color:#20b486; border-left-color:#20b486; animation:loginSpinReverse .65s linear infinite; }
        .login-loading-text { color:#31536e; font-size:13px; font-weight:700; }
        @keyframes loginSpin { to { transform:rotate(360deg); } } @keyframes loginSpinReverse { to { transform:rotate(-360deg); } }
    </style>
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
        <div id="loginLoading" class="login-loading-overlay" role="status" aria-live="polite" aria-hidden="true">
            <div class="login-loading-box"><div class="login-loading-spinner"></div><div class="login-loading-text">Đang xác thực tài khoản...</div></div>
        </div>
    </form>

    <script type="text/javascript">
        (function () {
            var password = document.getElementById('<%= txtPassWord.ClientID %>');
            var toggle = document.getElementById('togglePassword');
            var year = document.getElementById('currentYear');
            var loginButton = document.getElementById('<%= btLogon.ClientID %>');
            var loginLoading = document.getElementById('loginLoading');

            if (year) year.innerHTML = new Date().getFullYear();

            if (loginButton && loginLoading) {
                loginButton.addEventListener('click', function () {
                    loginLoading.classList.add('is-visible');
                    loginLoading.setAttribute('aria-hidden', 'false');
                });
            }
            window.addEventListener('pageshow', function () {
                if (!loginLoading) return;
                loginLoading.classList.remove('is-visible');
                loginLoading.setAttribute('aria-hidden', 'true');
            });

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
