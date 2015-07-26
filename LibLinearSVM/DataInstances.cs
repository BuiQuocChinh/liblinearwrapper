using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibLinearSVM
{
    public class DataInstances
    {
        public int FeatureCount {get; set; }
        public List<List<KeyValuePair<int, double>>> Instances { get; set; }
        public List<double> Labels { get; set; }
        public DataInstances()
        {
            Instances = new List<List<KeyValuePair<int, double>>>();
            Labels = new List<double>();
        }
        public void Add(List<KeyValuePair<int,double>> x, double y)
        {
            if (x.Count > 0)
            {
                var ls = x.Where(a => a.Value != 0).ToList(); // remove zero value node
                ls.Sort((a,b) =>a.Key.CompareTo(b.Key));
                ls.Add(new KeyValuePair<int, double>(-1,0)); // add bias. Important! Without this bias, the API will be CRASHED!!!
                Instances.Add(ls);
                Labels.Add(y);
            }
        }
    }

}
