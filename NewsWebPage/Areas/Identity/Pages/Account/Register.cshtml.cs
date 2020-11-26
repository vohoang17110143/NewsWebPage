using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MimeKit;
using NewsWebPage.Data;
using NewsWebPage.Models;
using NewsWebPage.Utility;

namespace NewsWebPage.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Name { get; set; }

            [Display(Name = "Số Điện thoại")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }

            [Display(Name = "Super Admin")]
            public string IsSuperAdmin { get; set; }

            [Display(Name = "Admin")]
            public string IsAdmin { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Name = Input.Name, Email = Input.Email, PhoneNumber = Input.PhoneNumber, Address = Input.Address };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //xác nhận email bằng cách gửi link url callback có chứa token xác nhận
                    //Tạo code token và chèn vào URL callback
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    string callbackUrl = Url.Action("Confirm", "AccountController1", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                    //Phân quyền lúc đăng ký người dùng.
                    //Nếu user hiện tại là SuperAdmin thì cho phép tạo tài khoảng cho nhân viên bán hàng hoặc tạo thêm 1 SuperAdmin mới
                    if (User.IsInRole(SD.SuperAdminEndUser))
                    {
                        if (!await _roleManager.RoleExistsAsync(SD.AdminEndUser))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(SD.AdminEndUser));
                        }
                        if (!await _roleManager.RoleExistsAsync(SD.SuperAdminEndUser))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(SD.SuperAdminEndUser));
                        }
                        if (bool.Parse(Input.IsSuperAdmin))
                        {
                            await _userManager.AddToRoleAsync(user, SD.SuperAdminEndUser);
                            ApplicationUser userFromDb = _db.ApplicationUser.Where(u => u.Email == Input.Email).FirstOrDefault();
                            userFromDb.Role = SD.SuperAdminEndUser;
                            _db.SaveChanges();
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, SD.AdminEndUser);
                            ApplicationUser userFromDb = _db.ApplicationUser.Where(u => u.Email == Input.Email).FirstOrDefault();
                            userFromDb.Role = SD.AdminEndUser;
                            userFromDb.isLockRole = true;
                            _db.SaveChanges();
                        }
                        //Tạo mail chứa url callback xác thực lúc tạo nhân viên mới, sau khi toàn tất thì redirect tới trang quản lý nhân viên
                        using (var client = new MailKit.Net.Smtp.SmtpClient())
                        {
                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress("News Page", "ngayvuive1999@gmail.com"));
                            message.To.Add(new MailboxAddress("Not Reply", user.Email));
                            message.Subject = "Confirm your email to join us";
                            message.Body = new TextPart(MimeKit.Text.TextFormat.Text)
                            {
                                Text = "Bạn đã được Admin của trang chúng tôi đăng ký tài khoản nhân viên với tài khoản email là: "
                                + user.Email + " và mật khẩu: " + Input.Password
                                + " để xác thực nhấn: " + callbackUrl + " Trước khi đăng nhập vào trang quản lý đơn hàng, hãy đổi mật khẩu để có thể xử lý đơn hàng!"
                            };

                            client.Connect("smtp.gmail.com", 465, true);
                            client.Authenticate("ngayvuive1999@gmail.com", "bainao1234");
                            client.Send(message);
                            client.Disconnect(true);
                            return Redirect("https://localhost:44305/Admin/AdminSite");
                        }
                    }
                    //Nếu chưa đăng nhập trước đó hoặc đang là user-customer thì chỉ cho phép tạo tài khoản cho khách hàng
                    else
                    {
                        if (!await _roleManager.RoleExistsAsync(SD.Reader))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(SD.Reader));
                        }
                        await _userManager.AddToRoleAsync(user, SD.Reader);
                        ApplicationUser userFromDb = _db.ApplicationUser.Where(u => u.Email == Input.Email).FirstOrDefault();
                        userFromDb.Role = SD.Reader;
                        _db.SaveChanges();
                        //Tạo thư gửi mail đến hòm thư mà khách hàng nhập lúc đăng ký sau đó redirect tới trang login
                        using (var client = new MailKit.Net.Smtp.SmtpClient())
                        {
                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress("News Page", "ngayvuive1999@gmail.com"));
                            message.To.Add(new MailboxAddress("Not Reply", user.Email));
                            message.Subject = "Confirm your email to join us";
                            message.Body = new TextPart(MimeKit.Text.TextFormat.Text)
                            {
                                Text = "Bạn đã đăng ký với tài khoản email là: "
                                + user.Email + " và mật khẩu: " + Input.Password
                                + " để xác thực nhấn: " + callbackUrl
                            };

                            client.Connect("smtp.gmail.com", 465, true);
                            client.Authenticate("ngayvuive1999@gmail.com", "bainao1234");
                            client.Send(message);
                            client.Disconnect(true);
                            return RedirectToPage("Login");
                        }
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}