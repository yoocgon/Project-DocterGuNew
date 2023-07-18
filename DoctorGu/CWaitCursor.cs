using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoctorGu
{
    /// <summary>
    /// 작업 도중에만 커서 모양을 모래시계로 변경하기 위함.
    /// </summary>
    /// <example>
    /// //바로 Dispose 메쏘드가 호출될 수 있도록 using을 사용함.
    /// using (new WaitCursor(this))
    /// {
    /// 	System.Threading.Thread.Sleep(1000);
    /// 	this.Text = DateTime.Now.ToString();
    /// }
    /// </example>
    public class CWaitCursor : IDisposable
    {
        private Form _frm;
        private Control[] _CtlsToDisable;
        public CWaitCursor(Form f) : this(f, new Control[] { })
        {
        }
        public CWaitCursor(Form frm, Control CtlToDisable) : this(frm, new Control[] { CtlToDisable })
        {
        }
        public CWaitCursor(Form frm, Control[] CtlsToDisable)
        {
            _frm = frm;
            _CtlsToDisable = CtlsToDisable;

            SetWait(true);
        }

        ~CWaitCursor()
        {
            Dispose();
        }

        private void SetWait(bool IsWaiting)
        {
            if (IsWaiting)
            {
                _frm.Cursor = Cursors.WaitCursor;
                if (_CtlsToDisable != null)
                {
                    foreach (var ctl in _CtlsToDisable)
                    {
                        ctl.Cursor = Cursors.WaitCursor;
                        ctl.Enabled = false;
                    }
                }
            }
            else
            {
                _frm.Cursor = Cursors.Default;
                if (_CtlsToDisable != null)
                {
                    foreach (var ctl in _CtlsToDisable)
                    {
                        ctl.Cursor = Cursors.Default;
                        ctl.Enabled = true;
                    }
                }
            }
        }

        public void Dispose()
        {
            try
            {
                SetWait(false);
            }
            catch (Exception) { }
        }
    }
}
