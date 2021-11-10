using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsMVC.View;

namespace WinFormsMVCUnitTest.Test.Request.Item.TextItem
{
    [TestClass]
    public class BackupTest : GenericBackupTest<string, WinFormsMVC.Request.Item.TextItem>
    {
        protected override string FormValue
        {
            get
            {
                return "My Form";
            }
        }

        protected override string NextValue
        {
            get
            {
                return "Next Value";
            }
        }
    }
}
