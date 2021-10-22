using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsMVC.Request;
using WinFormsMVC.View;
using WinFormsMVCUnitTest.Test.View;

namespace WinFormsMVCUnitTest.Test.Services.Base.GivenFormsManagementTest
{
    [TestClass]
    public class SimplyConnectedGivenFormsTest : GivenFormManagementTestFormat
    {
        protected WinFormsMVC.View.BaseForm DefaultBaseForm
        {
            get;
        }



        public SimplyConnectedGivenFormsTest()
        {
            DefaultBaseForm = new WinFormsMVC.View.BaseForm()
            {
                Text = "Default BaseForm"
            };

            var forms = BaseFormModel.CreateSimplyConnectedForms(DefaultBaseForm, BaseForm.MaxDepthTree);
            UpdateForms(forms);

            UpdateCommands(new List<Command>()
            {
                CreateDefaultCommand<BaseForm>(forms.First(), "Validation Text")
            });
        }


    }
}
