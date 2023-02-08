**ASP.NET基础实战入门：发送邮件功能，Web.config 实现部署后的邮件发送**

[toc]



```C#
public static class MembershipHelper
{
    public static bool SendForgotPasswordEmail(UserModel user, string passwordResetUrl)
    {
        bool result = true;
        try
        {
            var email = new MailMessage();

            //email.From = new MailAddress("admin@domain.com");
            email.To.Add(new MailAddress(user.Email));

            email.Subject =  Resources.Email_PasswordReset_Title;
            email.IsBodyHtml = true;

            email.Body = Resources.Email_PasswordReset_Body +
                        "<a href='" + passwordResetUrl + "'>" + passwordResetUrl + "</a>";

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Send(email);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Caught exception sending password reset email ");
            result = false;
        }
        return result;
    }
}
```


# 参考

[ASP.NET Web Pages - WebMail 帮助器](https://www.runoob.com/aspnet/webpages-email.html)