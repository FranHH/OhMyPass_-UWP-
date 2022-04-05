using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyPass.models
{
    [Serializable]
    public class UserKey
    {
        public string user = "";
        public string key = "";

        #region Constructores
        public UserKey(string user, string key, string numTlfn, string from)
        {
            this.key=key;
            this.user = user;
            this.NumTlfn = numTlfn;
            this.From = from;
       
        }
        public UserKey(){}

        public UserKey(string user)
        {
            this.user = user;
        }
        #endregion

        #region GET & SET
        public string GetKey()
        {
            return key;
        }
        public void SetKey(string key)
        {
            this.key = key;
        }
        public string GetUser()
        {
            return user;
        }
        public void SetUser(string user)
        {
            this.user = user;
        }

        public string NumTlfn { get; set; }
        public string From { get; set; }
        #endregion

    }
}
