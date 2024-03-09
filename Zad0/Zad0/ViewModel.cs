using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad0
{
    public class ViewModel
    {
        private Model _model;

        public ViewModel(Model model) 
        {
            _model=model;
        }
        public string Message 
        {
            get { return _model.Message; }
            set { _model.Message = value; }
        }
    }
}
