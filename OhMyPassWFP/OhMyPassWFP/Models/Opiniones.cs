using System;
using System.Collections.Generic;
using System.Text;

namespace OhMyPassWFP.Models
{
    public class Opiniones
    {
        #region Constructores
        public Opiniones() { }
        public Opiniones(string op, string us, string fec, string punt)
        {
            this.Opinion = op;
            this.User = us;
            this.Fecha = fec;
            this.Puntuacion = punt;
            this.Id = GenID();
        }
        #endregion

        #region Get & Set
        public string Opinion { get; set; }
        public string User { get; set; }
        public string Fecha { get; set; }
        public string Puntuacion { get; set; }
        public string Id { get; set; }
        #endregion

        #region GenID
        private string GenID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
        #endregion
    }
}
