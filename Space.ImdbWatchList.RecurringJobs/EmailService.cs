using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Space.ImdbWatchList.Dto;
using Space.ImdbWatchList.Infrastructure;

namespace Space.ImdbWatchList.RecurringJobs
{
    public class EmailService : IEmailService
    {
        private readonly EmailSenderSettings _emailSenderSettings;
        public EmailService(IOptions<EmailSenderSettings> emailSenderSettingsOptions)
        {
            _emailSenderSettings = emailSenderSettingsOptions.Value;
        }
        public void SendEmail(OfferDataDto offerDataDto)
        {
            var fromAddress = new MailAddress(_emailSenderSettings.FromAddress,_emailSenderSettings.FromName);
            var toAddress = new MailAddress(offerDataDto.Email, offerDataDto.UserName);
            string subject = "Film Offer";
            var body = _template.Replace("{{title}}", offerDataDto.Title)
                .Replace("{{poster-url}}", offerDataDto.Poster?.Link ?? "https://www.prokerala.com/movies/assets/img/no-poster-available.webp")
                .Replace("{{rating}}", offerDataDto.Rating.ToString())
                .Replace("{{description}}", offerDataDto.WikiShortDesc);

            var smtp = new SmtpClient
            {
                Host = _emailSenderSettings.Host,
                Port = _emailSenderSettings.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            };

            smtp.Credentials = new NetworkCredential(_emailSenderSettings.FromAddress, _emailSenderSettings.Password);


            using var message = new MailMessage(fromAddress, toAddress);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            smtp.Send(message);
        }

        private string _template = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">

<head>
    <meta charset=""UTF-8"">
    <meta content=""width=device-width, initial-scale=1"" name=""viewport"">
    <meta name=""x-apple-disable-message-reformatting"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta content=""telephone=no"" name=""format-detection"">
    <title></title>
    <!--[if (mso 16)]>
    <style type=""text/css"">
    a {text-decoration: none;}
    </style>
    <![endif]-->
    <!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]-->
    <!--[if gte mso 9]>
<xml>
    <o:OfficeDocumentSettings>
    <o:AllowPNG></o:AllowPNG>
    <o:PixelsPerInch>96</o:PixelsPerInch>
    </o:OfficeDocumentSettings>
</xml>
<![endif]-->
</head>

<body>
    <div class=""es-wrapper-color"">
        <!--[if gte mso 9]>
			<v:background xmlns:v=""urn:schemas-microsoft-com:vml"" fill=""t"">
				<v:fill type=""tile"" color=""#d9ead3""></v:fill>
			</v:background>
		<![endif]-->
        <table class=""es-wrapper"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
            <tbody>
                <tr>
                    <td class=""esd-email-paddings"" valign=""top"">
                        <table class=""es-header esd-header-popover"" cellspacing=""0"" cellpadding=""0"" align=""center"">
                            <tbody>
                                <tr>
                                    <td class=""esd-stripe"" align=""center"">
                                        <table class=""es-header-body"" width=""800"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"">
                                            <tbody>
                                                <tr>
                                                    <td class=""esd-structure es-p20t es-p20r es-p20l"" align=""left"">
                                                        <!--[if mso]><table width=""760"" cellpadding=""0"" cellspacing=""0""><tr><td width=""686"" valign=""top""><![endif]-->
                                                        <table cellpadding=""0"" cellspacing=""0"" align=""left"" class=""es-left"">
                                                            <tbody>
                                                                <tr>
                                                                    <td width=""686"" class=""esd-container-frame es-m-p20b"" align=""center"" valign=""top"">
                                                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td align=""center"" class=""esd-block-text"">
                                                                                        <p style=""color: #76a5af; font-size: 28px;"">{{title}}</p>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <!--[if mso]></td><td width=""20""></td><td width=""54"" valign=""top""><![endif]-->
                                                        <table cellpadding=""0"" cellspacing=""0"" class=""es-right"" align=""right"">
                                                            <tbody>
                                                                <tr>
                                                                    <td width=""54"" align=""left"" class=""esd-container-frame"">
                                                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td align=""left"" class=""esd-block-text"">
                                                                                        <p style=""color: #990000; font-size: 18px;"">{{rating}}</p>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <!--[if mso]></td></tr></table><![endif]-->
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table cellpadding=""0"" cellspacing=""0"" class=""es-content"" align=""center"">
                            <tbody>
                                <tr>
                                    <td class=""esd-stripe"" align=""center"">
                                        <table bgcolor=""#ffffff"" class=""es-content-body"" align=""center"" cellpadding=""0"" cellspacing=""0"" width=""800"">
                                            <tbody>
                                                <tr>
                                                    <td class=""es-p20t es-p20r es-p20l esd-structure"" align=""left"">
                                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                            <tbody>
                                                                <tr>
                                                                    <td width=""760"" class=""esd-container-frame"" align=""center"" valign=""top"">
                                                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td align=""center"" class=""esd-block-image"" style=""font-size: 0px;""><a target=""_blank"" href><img class=""adapt-img"" src=""{{poster-url}}"" alt width=""100%"" style=""display: block;""></a></td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table cellpadding=""0"" cellspacing=""0"" class=""es-content esd-footer-popover"" align=""center"">
                            <tbody>
                                <tr>
                                    <td class=""esd-stripe"" align=""center"">
                                        <table bgcolor=""#ffffff"" class=""es-content-body"" align=""center"" cellpadding=""0"" cellspacing=""0"" width=""800"">
                                            <tbody>
                                                <tr>
                                                    <td class=""es-p20t es-p20r es-p20l esd-structure"" align=""left"">
                                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                            <tbody>
                                                                <tr>
                                                                    <td width=""760"" class=""esd-container-frame"" align=""center"" valign=""top"">
                                                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td align=""left"" class=""esd-block-text"">
                                                                                        <p>{{description}}</p>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</body>

</html>";

    }
}