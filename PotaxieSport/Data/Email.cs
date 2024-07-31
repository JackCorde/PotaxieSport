using System.Net;
using System.Net.Mail;

namespace PotaxieSport.Data
{
    public class Email
    {

        public void RegistroAficionado(string correo)
        {
            CorreoRegistro(correo);
        }


        void CorreoRegistro(string correo_receptor)
        {
            string correo_emisor = "JackCorderod@outlook.com";
            string clave_emisor = "Bas2do.Guitar";

            MailAddress receptor = new(correo_receptor);
            MailAddress emisor = new(correo_emisor);

            MailMessage email = new(emisor, receptor);
            email.Subject = "POTAXIE SPORT: REGISTRO AFICIONADO";
            email.Body = @"<!DOCTYPE html>
                            <html>
                                <head>
                                    <title>Torneos Deportivos Potaxie Sport</title>
                                </head>
                                <body>
                                    <h2>Te damos la bienvenida a esta gran comunidad de aficionados al Futbol</h2>
                                    <br>
                                    <p>Se ha registrado su correo electrónico: " + correo_receptor + @"</p><br>
                                    <p>Te estaremos notificando hacerca de nuestros nuevos torneos, equipos y partidos. Gracias por registrarte.</p>
                                </body>
                            </html>";

            email.IsBodyHtml = true;

            SmtpClient smtp = new();
            smtp.Host = "smtp.office365.com"; //Dependiendo del correo
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(correo_emisor, clave_emisor);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(email);
            }
            catch (System.Exception)
            {
                throw;
            }

        }


        public void Enviar(string correo, string newPass)
        {
            Correo(correo, newPass);
        }

        void Correo(string correo_receptor, string pass)
        {
            string correo_emisor = "JackCorderod@outlook.com";
            string clave_emisor = "Bas2do.Guitar";

            MailAddress receptor = new(correo_receptor);
            MailAddress emisor = new(correo_emisor);

            MailMessage email = new(emisor, receptor);
            email.Subject = "CODEFUSION INNOVATIONS: Recuperación de Contraseña";
            email.Body = @"<!DOCTYPE html>
                            <html>
                                <head>
                                    <title>Torneos Deportivos CodeFusion Innovations</title>
                                </head>
                                <body>
                                    <h2>Recuperación de Contraseña de Usuario</h2>
                                    <br>
                                    <p>Su clave de confirmación es: " + pass + @"</p><br>
                                    <p>Por favor no la comparta con nadie</p>
                                </body>
                            </html>";

            email.IsBodyHtml = true;

            SmtpClient smtp = new();
            smtp.Host = "smtp.office365.com"; //Dependiendo del correo
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(correo_emisor, clave_emisor);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(email);
            }
            catch (System.Exception)
            {
                throw;
            }

        }


    }
}
