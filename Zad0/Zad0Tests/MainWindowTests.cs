using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zad0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad0.Tests
{
    [TestClass()]
    public class ViewModelTests
    {
        [TestMethod()]
        public void ViewModel_MessageProperty_ShouldReflectModelMessage()
        {
            Model model = new Model();
            model.Message = "Test Message";
            ViewModel viewModel = new ViewModel(model);

            string result=viewModel.Message;
            Assert.AreEqual(model.Message, result);
        }
        [TestMethod()]
        public void ViewModel_SetMessage_ShoulUpdateModelMessage() 
        {
            Model model = new Model();
            ViewModel viewModel = new ViewModel(model);

            viewModel.Message = "New Message";

            Assert.AreEqual("New Message", model.Message);
        }
    }
}