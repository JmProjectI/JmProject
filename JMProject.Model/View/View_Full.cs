using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Model.Sys;

namespace JMProject.Model.View
{
    public class View_Full
    {
        public View_Full()
        {
        }

        public Nksc NkscModel { get; set; }

        public List<Nksc_fz> Nksc_fzModel { get; set; }

        public List<View_KS> Nksc_ksModel { get; set; }

        public List<Nksc_Zcyw> Nksc_ZcywModel { get; set; }
        public List<Nksc_Zcyw> Nksc_CzZcywModel { get; set; }
        public List<Nksc_Zcyw> Nksc_FczZcywModel { get; set; }
        public List<Nksc_Jkyw> Nksc_JkywModel { get; set; }
        public List<Nksc_Bxyw> Nksc_BxywModel { get; set; }
        public List<Nksc_cghtsq> Nksc_cghtsqModel { get; set; }
        public List<Nksc_cghtsq> Nksc_ZxcghtsqModel { get; set; }

        public List<Nksc_qlqd> Nksc_qlqdModel { get; set; }

        public S_YJDZ CustomModel { get; set; }//客户 发件相关信息

        //是超级用户还是客户
        public string UserInfoOK { get; set; }
    }
}
