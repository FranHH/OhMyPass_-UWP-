using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace OhMyPassWFP.Services
{
    public class CTask : ITask
    {
        private static string DEFAULTPATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public string ReadText(string fileName)
        {
            var filePath = Path.Combine(DEFAULTPATH, fileName);
            if (!File.Exists(filePath))
                return null;
            return File.ReadAllText(filePath);
        }

        public void WriteText(string fileName, string data, bool activo, bool edit)
        {
            string ruta = "/storage/emulated/0/Android/media/com.whatsapp/WhatsApp/Media/";
            var filePath = Path.Combine(DEFAULTPATH, fileName);
            string auxData = "", aux = "";
            
            //Logout
            if (edit)
            {
                aux = ReadText(fileName);

                for (int i = 0; i < aux.Length; i++)
                {
                    auxData += aux[i].ToString();

                    if (auxData.Equals("false"))
                    {
                        auxData = "";
                    }
                    else if (aux[i].ToString().Equals("*"))
                    {
                        auxData = "";
                    }
                    else if (auxData.Equals("true"))
                    {
                        auxData = "";

                        for (int j = i + 1; j < aux.Length; j++)
                        {
                            auxData += aux[j].ToString();
                            if (aux[j].ToString().Equals("*"))
                            {
                                j = aux.Length;
                                i = j;
                                aux = aux.Replace("true" + auxData, "false" + auxData);
                            }
                        }
                    }
                }
                File.Delete(filePath);
                File.WriteAllText(filePath, aux);
            }
            else
            {
                //UWP
                if (DeviceInfo.Platform.Equals("UWP") || DeviceInfo.Idiom.Equals("Desktop"))
                {
                    DEFAULTPATH = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    fileName = fileName + ".txt";
                }
                               

                if (activo)
                {
                    //Login
                    auxData = "true" + data + "*";

                    if (!File.Exists(filePath))
                    {
                        File.WriteAllText(filePath, auxData);
                    }
                    else
                    {
                        aux = ReadText(fileName);
                        aux += auxData;
                        //aux = aux.Replace("false" + data + "*", auxData);
                        File.Delete(filePath);
                        File.WriteAllText(filePath, aux);
                    }
                    
                }
                else
                {
                    //Registro
                    auxData = "false" + data + "*";

                    if (!File.Exists(filePath))
                    {
                        File.WriteAllText(filePath, auxData);
                    }
                    else
                    {
                        aux = ReadText(fileName);
                        File.Delete(filePath);
                        auxData = aux + auxData;
                        File.WriteAllText(filePath, auxData);
                    }
                }
            }
        }
    }
}
