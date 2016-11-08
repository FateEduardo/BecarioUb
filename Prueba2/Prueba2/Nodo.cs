using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace Prueba2
{
    [DataContract]
  public   class Nodo
    {

        [DataMember]
        internal string source;
        [DataMember]
        internal string destination;
        [DataMember]
        internal DateTime ts;
        [DataMember]
        internal string type;
        [DataMember]
        internal int channe = 19;
        [DataMember]
        internal int lqi;
        [DataMember]
        internal string layer;
        [DataMember]
        internal int length;
        [DataMember]
        internal int rssi;
        [DataMember]
        internal string security;

    }
}
