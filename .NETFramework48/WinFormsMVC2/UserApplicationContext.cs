﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MVCTraining.Controller;
using MVCTraining.View;

namespace MVCTraining
{
    public class UserApplicationContext<T> : ApplicationContext where T : View.BaseForm
    {
        protected FormManager _form_manager;
        protected OperationManager _operation_manager;

        protected ViewFacade _facade;
        protected T _root_form;

        public UserApplicationContext()
        {
            _form_manager = new FormManager();
            _operation_manager = new OperationManager();

            _facade = new ViewFacade(_form_manager, _operation_manager);
            _form_manager.Facade = _facade;

            _facade.Forms = new List<BaseForm>()
            {
                new Form1(),
                new Form2(),
                new Form3()
            };
            _facade.Controllers = new List<Controller.Controller>()
            {
                new Form1Controller(_form_manager, _operation_manager),
                new Form2Controller(_form_manager, _operation_manager),
                new Form3Controller(_form_manager, _operation_manager)
            };

            _root_form = (T)typeof(T).GetConstructor(new Type[0]).Invoke(new object[0]);
            _root_form.Closed += new EventHandler(OnFormClosed);
            _form_manager.LaunchForm(null, _root_form);
        }

        protected void OnFormClosed(object sender, EventArgs e)
        {
            ExitThread();
        }
    }
}
