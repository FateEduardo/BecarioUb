using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Prueba2
{
    



    class Deserializate
    {
        public
        List<Nodo> list = new List<Nodo>();
        public void Leer()
        {
                     try
            {
                String gt = File.ReadAllText("/input.json");
               list = JsonConvert.DeserializeObject<List<Nodo>>(gt);
            }
            catch
            {
                    
            }
        }
        public void Escribir()
        {
  

        }




    }
}