using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;
using System.ComponentModel.DataAnnotations;

namespace JMProject.Model
{
    public class DictionaryItem
    {
        public DictionaryItem()
        { }

        [Display(Name = "上级编号")]
        public string DicID { get; set; }
        [PrimaryKey]
        [Display(Name = "编号")]
        public string ItemID { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "名称不允许为空！")]
        [StringLength(30, ErrorMessage = "最大长度为30")]
        public string ItemName { get; set; }
        [Display(Name = "描述")]
        public string ItemDesc { get; set; }
        [Display(Name = "标识")]
        public string ItemFlag { get; set; }

    }

}
