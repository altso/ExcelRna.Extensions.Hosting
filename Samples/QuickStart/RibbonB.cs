using System.Runtime.InteropServices;
using System.Windows.Forms;
using ExcelDna.Extensions.Hosting;
using ExcelDna.Integration.CustomUI;

namespace QuickStart
{
    [ComVisible(true)]
    public class RibbonB : HostedExcelRibbon
    {
        private readonly ICustomService _customService;

        public RibbonB(ICustomService customService) => _customService = customService;

        public override string GetCustomUI(string ribbonId)
        {
            return @"
          <customUI xmlns='http://schemas.microsoft.com/office/2006/01/customui'>
          <ribbon>
            <tabs>
              <tab id='tab1' label='Ribbon B'>
                <group id='group1' label='My Group'>
                  <button id='button1' label='My Button' onAction='OnButtonPressed'/>
                </group >
              </tab>
            </tabs>
          </ribbon>
        </customUI>";
        }

        public void OnButtonPressed(IRibbonControl control)
        {
            MessageBox.Show("Hello from control " + control.Id + " " + _customService.GetString());
        }
    }
}