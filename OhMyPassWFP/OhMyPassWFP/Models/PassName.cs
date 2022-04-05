using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OhMyPass.models
{
    [Serializable]
    public class PassName
    {
        #region Constructores
        public PassName (string pass, string name, string grupo ,string nota, string icon, string date)
        {
            this.Pass = pass;
            this.Name = name;
            this.Grupo = grupo;
            this.Nota = nota;
            this.Id = GenID();
            this.IconFav = icon;
            this.Date = date;
        }
        public PassName(){}
        #endregion

        #region GET & SET
        public string Id { get; set; }
        public string Pass { get; set; }
        public string Name { get; set; }
        public string Grupo { get; set; }
        public string Nota { get; set; }
        public string IconFav { get; set; }
        public string Date { get; set; }
        #endregion

        #region Metodos
        override
        public string ToString()
        {
            string info = "Contraseña encriptada --> " + Pass + " \nNombre --> " + Name + " \nGrupo --> " + Grupo ;
            return info;
        }
        private string GenID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        

        #endregion
    }
}
