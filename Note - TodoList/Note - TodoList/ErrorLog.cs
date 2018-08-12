using System;
using System.IO;

namespace Note___TodoList
{
    /// <summary>
    /// The ErrorLog class
    /// use for writing Exception Message to a log file
    /// </summary>
    /// <remarks>
    /// <para>This class can write Exception message to log txt file</para>
    /// </remarks>
    static class ErrorLog
    {
        /// <summary>
        /// Write Exception to log file
        /// </summary>
        /// <param name="e">Exception </param>
        public static void Write(Exception e)
        {
            // use log.txt in the folder 
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                w.WriteLine(" :");
                w.WriteLine(" :{0}", e.Message);
                w.WriteLine(" :{0}", e.InnerException);
                w.WriteLine(" :{0}", e.Source);
                w.WriteLine(" :{0}", e.StackTrace);
                w.WriteLine("---------------------------------------------------------------------------------");
            }
        }
    }
}
