using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Dtos
{
    public class MailerDominModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string MessageBody { get; set; }
        /// <summary>
        /// SendTo
        /// </summary>
            public string Receiver { get; set; }
            /// <summary>
            /// From 
            /// </summary>
            public string Sender { get; set; }
            public string Host { get; set; }
            public string Password { get; set; }
    }
}
