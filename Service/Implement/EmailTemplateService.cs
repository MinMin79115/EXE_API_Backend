using System;
using System.Text;

namespace EXE_API_Backend.Services
{
    public class EmailTemplateService
    {
        /// <summary>
        /// Lấy nội dung email xác nhận đăng ký
        /// </summary>
        /// <param name="fullName">Tên đầy đủ của người dùng</param>
        /// <param name="userName">Tên đăng nhập của người dùng</param>
        /// <returns>Nội dung email dạng HTML</returns>
        public string GetRegistrationConfirmationTemplate(string fullName, string userName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h1>Chào mừng " + fullName + "!</h1>");
            sb.AppendLine("<p>Bạn đã đăng ký thành công tài khoản trên hệ thống EXE API Backend.</p>");
            sb.AppendLine("<p>Tên đăng nhập: " + userName + "</p>");
            return sb.ToString();
        }

        /// <summary>
        /// Lấy nội dung email đặt lại mật khẩu
        /// </summary>
        /// <param name="fullName">Tên đầy đủ của người dùng</param>
        /// <param name="resetLink">Link đặt lại mật khẩu</param>
        /// <returns>Nội dung email dạng HTML</returns>
        public string GetPasswordResetTemplate(string fullName, string resetLink)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h1>Đặt lại mật khẩu</h1>");
            sb.AppendLine("<p>Xin chào " + fullName + ",</p>");
            sb.AppendLine("<p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn trên hệ thống EXE API Backend.</p>");
            sb.AppendLine("<p>Vui lòng nhấp vào liên kết dưới đây để đặt lại mật khẩu:</p>");
            sb.AppendLine("<a href='" + resetLink + "'>Đặt lại mật khẩu</a>");
            sb.AppendLine("<p>Liên kết này sẽ hết hạn sau 24 giờ.</p>");
            sb.AppendLine("<p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>");
            return sb.ToString();
        }

        /// <summary>
        /// Lấy nội dung email thông báo chung
        /// </summary>
        /// <param name="fullName">Tên đầy đủ của người dùng</param>
        /// <param name="message">Nội dung thông báo</param>
        /// <returns>Nội dung email dạng HTML</returns>
        public string GetGeneralNotificationTemplate(string fullName, string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h1>Thông báo từ EXE API Backend</h1>");
            sb.AppendLine("<p>Xin chào " + fullName + ",</p>");
            sb.AppendLine("<p>" + message + "</p>");
            sb.AppendLine("<p>Trân trọng,</p>");
            sb.AppendLine("<p>Đội ngũ EXE API Backend</p>");
            return sb.ToString();
        }
    }
} 