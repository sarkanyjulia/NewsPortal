﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Admin.ViewModel
{
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Üzenet lekérdezése, vagy beállítása.
        /// </summary>
        public String Message { get; private set; }

        /// <summary>
        /// Üzenet eseményargumentum példányosítása.
        /// </summary>
        /// <param name="message">Üzenet.</param>
        public MessageEventArgs(String message) { Message = message; }
    }
}
