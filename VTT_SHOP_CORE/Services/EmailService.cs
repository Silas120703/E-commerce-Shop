using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using VTT_SHOP_CORE.Interfaces;

namespace VTT_SHOP_CORE.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string verificationCode)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["Mail:From"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family:Arial, sans-serif; background-color:#f4f6f8; padding:30px;'>
                    <div style='max-width:600px; margin:auto; background:white; border-radius:10px; padding:25px; box-shadow:0 3px 10px rgba(0,0,0,0.1);'>
                        <div style='text-align:center;'>
                            <img src='https://i.imgur.com/sb7n9KQ.png' alt='VTT Shop' style='width:80px; margin-bottom:10px;'/>
                            <h2 style='color:#333; margin-bottom:5px;'>VTT Shop</h2>
                            <p style='color:#888; margin-top:0;'>Xác nhận tài khoản của bạn</p>
                        </div>

                        <hr style='border:none; border-top:2px solid #007bff; width:50px; margin:auto; margin-bottom:25px;'/>

                        <div style='color:#333; line-height:1.6; text-align:center;'>
                            <p>Bạn đang đăng ký tài khoản tại <b>VTT Shop</b>.</p>
                            <p>Mã xác thực của bạn là:</p>
                            <h1 style='color:#007bff; letter-spacing:3px; margin:20px 0;'>{verificationCode}</h1>
                            <p>Vui lòng nhập mã này vào trang xác thực để hoàn tất quá trình đăng ký.</p>
                            <p style='font-size:13px; color:#666;'>Mã sẽ hết hạn sau 2 phút.</p>
                        </div>

                        <div style='text-align:center; margin-top:30px;'>
                            <a href='https://vttshop.com' style='display:inline-block; background-color:#007bff; color:#fff; padding:10px 20px; text-decoration:none; border-radius:5px; font-weight:bold;'>Đến VTT Shop</a>
                        </div>

                        <hr style='border:none; border-top:1px solid #ddd; margin:30px 0;'/>
                        <p style='font-size:13px; color:#777; text-align:center;'>
                            Đây là email tự động, vui lòng không trả lời.<br/>
                            &copy; {DateTime.Now.Year} VTT Shop. All rights reserved.
                        </p>
                    </div>
                </div>"
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _config["Mail:SmtpServer"],
                int.Parse(_config["Mail:Port"]),
                SecureSocketOptions.StartTls
            );
            await smtp.AuthenticateAsync(_config["Mail:UserName"], _config["Mail:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailForgotPasswordAsync(string to, string subject, string verificationCode)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["Mail:From"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family:Arial, sans-serif; background-color:#f4f6f8; padding:30px;'>
                    <div style='max-width:600px; margin:auto; background:white; border-radius:10px; padding:25px; box-shadow:0 3px 10px rgba(0,0,0,0.1);'>
                        <div style='text-align:center;'>
                            <img src='https://i.imgur.com/sb7n9KQ.png' alt='VTT Shop' style='width:80px; margin-bottom:10px;'/>
                            <h2 style='color:#333; margin-bottom:5px;'>VTT Shop</h2>
                            <p style='color:#888; margin-top:0;'>Xác nhận tài khoản của bạn</p>
                        </div>

                        <hr style='border:none; border-top:2px solid #007bff; width:50px; margin:auto; margin-bottom:25px;'/>

                        <div style='color:#333; line-height:1.6; text-align:center;'>
                            <p>Bạn đang khôi phục tài khoản tại <b>VTT Shop</b>.</p>
                            <p>Mã xác thực của bạn là:</p>
                            <h1 style='color:#007bff; letter-spacing:3px; margin:20px 0;'>{verificationCode}</h1>
                            <p>Vui lòng nhập mã này vào trang xác thực để hoàn tất quá trình khôi phục tài khoản.</p>
                            <p style='font-size:13px; color:#666;'>Mã sẽ hết hạn sau 2 phút.</p>
                        </div>

                        <div style='text-align:center; margin-top:30px;'>
                            <a href='https://vttshop.com' style='display:inline-block; background-color:#007bff; color:#fff; padding:10px 20px; text-decoration:none; border-radius:5px; font-weight:bold;'>Đến VTT Shop</a>
                        </div>

                        <hr style='border:none; border-top:1px solid #ddd; margin:30px 0;'/>
                        <p style='font-size:13px; color:#777; text-align:center;'>
                            Đây là email tự động, vui lòng không trả lời.<br/>
                            &copy; {DateTime.Now.Year} VTT Shop. All rights reserved.
                        </p>
                    </div>
                </div>"
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _config["Mail:SmtpServer"],
                int.Parse(_config["Mail:Port"]),
                SecureSocketOptions.StartTls
            );
            await smtp.AuthenticateAsync(_config["Mail:UserName"], _config["Mail:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
