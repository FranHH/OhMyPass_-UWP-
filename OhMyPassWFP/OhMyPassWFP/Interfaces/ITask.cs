using System;
using System.Collections.Generic;
using System.Text;

namespace OhMyPassWFP.Services
{
    public interface ITask
    {
        string ReadText(string fileName);
        void WriteText(string fileName, string data, bool activo, bool edit);
    }
}
