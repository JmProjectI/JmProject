using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class NkReport_Progress
    {
        public NkReport_Progress()
        {
            Tjrq = "";
            Tsyqtext = "";
            Shrq = "";
            Shr = "";
            Zzrq = "";
            Zzr = "";
            Yjrq = "";
            Yjr = "";
            Fsrq = "";
            Fsr = "";
            Lsr = "";
        }

        #region 字段

        /// <summary>
        /// 编号
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// 主表编号
        /// </summary>
        public Guid Zid { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public string Tjrq { get; set; }

        /// <summary>
        /// 特殊描述
        /// </summary>
        public string Tsyqtext { get; set; }

        /// <summary>
        /// 审核日期
        /// </summary>
        public string Shrq { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string Shr { get; set; }

        /// <summary>
        /// 制作日期
        /// </summary>
        public string Zzrq { get; set; }

        /// <summary>
        /// 制作人
        /// </summary>
        public string Zzr { get; set; }

        /// <summary>
        /// 移交日期
        /// </summary>
        public string Yjrq { get; set; }

        /// <summary>
        /// 移交人
        /// </summary>
        public string Yjr { get; set; }

        /// <summary>
        /// 发送报告日期
        /// </summary>
        public string Fsrq { get; set; }

        /// <summary>
        /// 发送报告人
        /// </summary>
        public string Fsr { get; set; }

        /// <summary>
        /// 历史制作人
        /// </summary>
        public string Lsr { get; set; }

        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO NkReport(");
            sb.Append("Tjrq");
            sb.Append(",Tsyqtext");
            sb.Append(",Shrq");
            sb.Append(",Shr");
            sb.Append(",Zzrq");
            sb.Append(",Zzr");
            sb.Append(",Yjrq");
            sb.Append(",Yjr");
            sb.Append(",Fsrq");
            sb.Append(",Fsr");
            sb.Append(",Lsr");
            sb.Append(") values(");
            sb.Append("'" + Tjrq + "'");
            sb.Append("'" + Tsyqtext + "'");
            sb.Append("'" + Shrq + "'");
            sb.Append("'" + Shr + "'");
            sb.Append("'" + Zzrq + "'");
            sb.Append("'" + Zzr + "'");
            sb.Append("'" + Yjrq + "'");
            sb.Append("'" + Yjr + "'");
            sb.Append("'" + Fsrq + "'");
            sb.Append("'" + Fsr + "'");
            sb.Append("'" + Lsr + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}