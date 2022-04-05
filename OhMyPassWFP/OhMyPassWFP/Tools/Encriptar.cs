using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class Encriptar{

    #region Declaracion de variables
    private const string CESAR = "abcdefghijklmñnopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ1234567890_-+,#$%&/()=¿?¡!|,.;:{}[]";
    private int desp = 3;
    #endregion

    #region Encriptar
    public string PassEncrip(string pass)
    {
        string passfinal = "", passSeg= "10*9Â", passSeg2= "ñÂ1■*", cifrado="";
        
        IList<int> passByte = new List<int>();

        byte[] bp = Encoding.ASCII.GetBytes(passSeg+pass+passSeg2);
        for (int i = 0; i < bp.Length; i++)
        {
            passByte.Add(0);
            passByte.Add(0);
            passByte.Add(0);
            passByte.Add(bp[i]);
        }

        foreach (int pb in passByte)
        {
            passfinal += pb.ToString();
        }

        for (int i = 0; i < passfinal.Length; i++)
        {
            int pos = getPostCesar(passfinal[i]);
            if (pos != -1)
            {
                int posi = pos + desp;
                while (pos >= CESAR.Length)
                {
                    posi = posi - CESAR.Length;
                }
                cifrado += CESAR[posi];
            }
            else
            {
                cifrado += passfinal[i];
            }
        }

        

        return cifrado;
    }
    #endregion

    #region Desencriptar
    public string PassDesEncrip(string pass)
    {
        IList<string> listaByteString = new List<string>();
        byte[] listaByte = null;
        string aux = "", passfinal = "", cifrado = "";
        bool e = false;

        for (int i = 0; i < pass.Length; i++)
        {
            int pos = getPostCesar(pass[i]);
            if (pos != -1)
            {
                int posi = pos - desp;
                while (posi < 0)
                {
                    posi = posi + CESAR.Length;
                }
                cifrado += CESAR[posi];
            }
            else
            {
                cifrado += pass[i];
            }
        }

        for (int i = 0; i < cifrado.Length; i++)
        {
            if (!cifrado[i].ToString().Equals("0"))
            {
                if (cifrado[i - 1].ToString().Equals("0"))
                {
                    if (i + 2 < cifrado.Length)
                    {
                        int auxnum = int.Parse(cifrado[i].ToString() + cifrado[i + 1].ToString() + cifrado[i + 2].ToString());
                        if (auxnum < 255)
                        {
                            aux = auxnum.ToString();
                            listaByteString.Add(aux);
                            i = i + 2;
                            e = true;
                        }
                        else
                        {
                            auxnum = 0;
                        }
                    }

                    if (!e)
                    {
                        aux = cifrado[i].ToString() + cifrado[i + 1].ToString();
                        listaByteString.Add(aux);
                        i = i + 1;
                    }
                }
                e = false;

                if (i == cifrado.Length)
                {
                    i = i - 1;
                }
            }
        }

        listaByte = new byte[listaByteString.Count()];
        int count = 0;
        foreach(string item in listaByteString)
        {
            listaByte[count] = byte.Parse(item);
            count ++;
        }

        passfinal = Encoding.Default.GetString(listaByte);
        passfinal = passfinal.Replace("??1?*", "").Replace("10*9?", "");
 
        return passfinal;
    }
    #endregion

    #region Encriptar name
    public string NameEncrypt(string name)
    {
        try
        {
            string textToEncrypt = name;
            string ToReturn = "";
            string publickey = "12345678";
            string secretkey = "87654321";
            byte[] secretkeyByte = { };
            secretkeyByte = Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = Encoding.UTF8.GetBytes(textToEncrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                ToReturn = Convert.ToBase64String(ms.ToArray());
            }
            return ToReturn;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }
    #endregion

    #region Desencriptar name
    public string DesEncryptName(string name)
    {
        try
        {
            string textToDecrypt = name;
            string ToReturn = "";
            string publickey = "12345678";
            string secretkey = "87654321";
            byte[] privatekeyByte = { };
            privatekeyByte = Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
            inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                ToReturn = encoding.GetString(ms.ToArray());
            }
            return ToReturn;
        }
        catch (Exception ae)
        {
            throw new Exception(ae.Message, ae.InnerException);
        }
    }
    #endregion

    #region Metodos
    private int getPostCesar(char caracter)
    {
        for(int i=0; i < CESAR.Length; i++)
        {
            if(caracter == CESAR[i])
            {
                return i;
            }
        }
        return -1;
    }
    #endregion

}