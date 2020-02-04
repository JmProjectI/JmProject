using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using NPOI.SS.Util;
using JMProject.Model.View;
using NPOI.HSSF.Util;

namespace JMProject.Common
{
    /// <summary>
    /// Excel文件操作类
    /// </summary>
    public static class ExcelHelper
    {
        private static ICellStyle GetHeadStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            //新建一个字体样式对象
            IFont font = workbook.CreateFont();
            //设置字体加粗样式
            font.Boldweight = short.MaxValue;
            font.FontHeight = 240;
            font.FontName = "等线";
            //使用SetFont方法将字体样式添加到单元格样式中
            style.SetFont(font);
            return style;
        }

        private static ICellStyle GetsubHeadStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Left;
            style.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            //设置字体加粗样式
            font.Boldweight = short.MaxValue;
            font.FontHeight = 220;
            font.FontName = "等线";
            style.SetFont(font);
            return style;
        }

        private static ICellStyle GetsubCenterHeadStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            //设置字体加粗样式
            font.Boldweight = short.MaxValue;
            font.FontHeight = 220;
            font.FontName = "等线";
            style.SetFont(font);
            return style;
        }

        private static ICellStyle GetfootStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Left;
            style.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            //设置字体加粗样式
            font.Boldweight = short.MaxValue;
            font.FontHeight = 270;
            font.FontName = "等线";
            style.SetFont(font);
            return style;
        }

        private static ICellStyle GetTitleStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();

            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            //设置字体加粗样式
            font.FontHeight = 220;
            font.FontName = "等线";
            style.SetFont(font);
            return style;
        }

        private static ICellStyle GetItemStyle1(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();

            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;

            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Left;
            style.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            //设置字体加粗样式
            font.FontHeight = 220;
            font.FontName = "等线";
            style.SetFont(font);
            return style;
        }

        private static ICellStyle GetItemStyle2(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();

            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Right;
            style.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            //设置字体加粗样式
            font.FontHeight = 220;
            font.FontName = "等线";
            style.SetFont(font);
            return style;
        }

        #region 导出固定资产
        public static MemoryStream Export_GD(DataTable dt, int[] widths, string[] heads, string sheetName, string dwname, string ydate)
        {
            //先创建一个流
            MemoryStream ms = new MemoryStream();
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    //新建一个excel
                    HSSFWorkbook workbook = new HSSFWorkbook();
                    //excel样式
                    HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
                    //创建一个sheet
                    ISheet sheet = workbook.CreateSheet(sheetName);
                    //给指定sheet的内容设置每列宽度（index从0开始，width1000相当于excel设置的列宽3.81）
                    for (int i = 0; i < widths.Length; i++)
                    {
                        sheet.SetColumnWidth(i, widths[i]);
                    }

                    //第一行
                    var row1 = sheet.CreateRow(0);
                    row1.Height = 200;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row1.CreateCell(i);
                    }

                    //第二行
                    var row2 = sheet.CreateRow(1);
                    row2.Height = 600;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row2.CreateCell(i);
                    }
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 10));
                    ICell headcell = row2.GetCell(0);
                    headcell.SetCellValue(sheetName);
                    headcell.CellStyle = GetHeadStyle(workbook);


                    //第三行
                    var row3 = sheet.CreateRow(2);
                    row3.Height = 350;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row3.CreateCell(i);
                    }
                    row3.GetCell(0).SetCellValue("单位名称：");
                    row3.GetCell(0).CellStyle = GetsubHeadStyle(workbook);
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 4));
                    row3.GetCell(1).SetCellValue(dwname);
                    row3.GetCell(1).CellStyle = GetsubHeadStyle(workbook);
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 5, 7));
                    row3.GetCell(5).SetCellValue(ydate);
                    row3.GetCell(5).CellStyle = GetsubHeadStyle(workbook);
                    row3.GetCell(10).SetCellValue("单位：元");
                    row3.GetCell(10).CellStyle = GetsubHeadStyle(workbook);

                    //第四行
                    var row4 = sheet.CreateRow(3);
                    row4.Height = 500;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell TitleCell = row4.CreateCell(i);
                        TitleCell.SetCellValue(heads[i]);
                        TitleCell.CellStyle = GetTitleStyle(workbook);
                    }

                    for (var r = 0; r < dt.Rows.Count; r++)
                    {
                        var row_r = sheet.CreateRow(r + 4);
                        row_r.Height = 500;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ICell itemCell = row_r.CreateCell(i);
                            itemCell.SetCellValue(dt.Rows[r][i].ToString());
                            if (i == 0 || i == 1 || i == 2 || i == 6 || i == 7 || i == 8 || i == 10)
                            {
                                itemCell.CellStyle = GetItemStyle1(workbook);
                            }
                            else
                            {
                                itemCell.CellStyle = GetItemStyle2(workbook);
                            }
                        }
                    }

                    //末行
                    var rowLast = sheet.CreateRow(16);
                    rowLast.Height = 400;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        rowLast.CreateCell(i);
                    }
                    rowLast.GetCell(0).SetCellValue("院长签字：");
                    rowLast.GetCell(0).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(3).SetCellValue("会计签字：");
                    rowLast.GetCell(3).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(7).SetCellValue("填表人：");
                    rowLast.GetCell(7).CellStyle = GetfootStyle(workbook);

                    sheet.AddMergedRegion(new CellRangeAddress(4, 15, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
                    sheet.AddMergedRegion(new CellRangeAddress(6, 7, 1, 1));
                    sheet.AddMergedRegion(new CellRangeAddress(8, 9, 1, 1));
                    sheet.AddMergedRegion(new CellRangeAddress(10, 11, 1, 1));
                    sheet.AddMergedRegion(new CellRangeAddress(12, 13, 1, 1));
                    sheet.AddMergedRegion(new CellRangeAddress(14, 15, 1, 1));

                    sheet.AddMergedRegion(new CellRangeAddress(4, 15, 6, 6));
                    sheet.AddMergedRegion(new CellRangeAddress(0, 1, 7, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 3, 7, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(4, 5, 7, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(6, 7, 7, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(8, 9, 7, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(10, 11, 7, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(12, 13, 7, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(14, 15, 7, 7));

                    //写入流
                    workbook.Write(ms);
                    ms.Flush();
                    return ms;
                }
                catch (Exception ex)
                {
                    //
                }
            }
            return null;
        }
        #endregion

        #region 导出无形资产
        public static MemoryStream Export_WX(DataTable dt, int[] widths, string[] heads, string sheetName, string dwname, string ydate)
        {
            //先创建一个流
            MemoryStream ms = new MemoryStream();
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    //新建一个excel
                    HSSFWorkbook workbook = new HSSFWorkbook();
                    //excel样式
                    HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
                    //创建一个sheet
                    ISheet sheet = workbook.CreateSheet(sheetName);
                    //给指定sheet的内容设置每列宽度（index从0开始，width1000相当于excel设置的列宽3.81）
                    for (int i = 0; i < widths.Length; i++)
                    {
                        sheet.SetColumnWidth(i, widths[i]);
                    }

                    //第一行
                    var row1 = sheet.CreateRow(0);
                    row1.Height = 200;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row1.CreateCell(i);
                    }

                    //第二行
                    var row2 = sheet.CreateRow(1);
                    row2.Height = 600;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row2.CreateCell(i);
                    }
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));
                    ICell headcell = row2.GetCell(0);
                    headcell.SetCellValue(sheetName);
                    headcell.CellStyle = GetHeadStyle(workbook);


                    //第三行
                    var row3 = sheet.CreateRow(2);
                    row3.Height = 350;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row3.CreateCell(i);
                    }
                    row3.GetCell(0).SetCellValue("单位名称：");
                    row3.GetCell(0).CellStyle = GetsubHeadStyle(workbook);
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 3));
                    row3.GetCell(1).SetCellValue(dwname);
                    row3.GetCell(1).CellStyle = GetsubHeadStyle(workbook);
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 6));
                    row3.GetCell(4).SetCellValue(ydate);
                    row3.GetCell(4).CellStyle = GetsubHeadStyle(workbook);
                    row3.GetCell(8).SetCellValue("单位：元");
                    row3.GetCell(8).CellStyle = GetsubHeadStyle(workbook);

                    //第四行
                    var row4 = sheet.CreateRow(3);
                    row4.Height = 500;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell TitleCell = row4.CreateCell(i);
                        TitleCell.SetCellValue(heads[i]);
                        TitleCell.CellStyle = GetTitleStyle(workbook);
                    }

                    for (var r = 0; r < dt.Rows.Count; r++)
                    {
                        var row_r = sheet.CreateRow(r + 4);
                        row_r.Height = 500;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ICell itemCell = row_r.CreateCell(i);
                            itemCell.SetCellValue(dt.Rows[r][i].ToString());
                            if (i == 0 || i == 1 || i == 5 || i == 6 || i == 8)
                            {
                                itemCell.CellStyle = GetItemStyle1(workbook);
                            }
                            else if (i == 2 || i == 3 || i == 4)
                            {
                                if (dt.Rows[r][1].ToString() == "")
                                {
                                    itemCell.SetCellValue("");
                                }
                                itemCell.CellStyle = GetItemStyle2(workbook);
                            }
                            else
                            {
                                itemCell.CellStyle = GetItemStyle2(workbook);
                            }
                        }
                    }

                    //末行
                    var rowLast = sheet.CreateRow(12);
                    rowLast.Height = 400;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        rowLast.CreateCell(i);
                    }
                    rowLast.GetCell(0).SetCellValue("院长签字：");
                    rowLast.GetCell(0).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(3).SetCellValue("会计签字：");
                    rowLast.GetCell(3).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(6).SetCellValue("填表人：");
                    rowLast.GetCell(6).CellStyle = GetfootStyle(workbook);

                    sheet.AddMergedRegion(new CellRangeAddress(4, 11, 0, 0));

                    sheet.AddMergedRegion(new CellRangeAddress(4, 11, 5, 5));

                    //写入流
                    workbook.Write(ms);
                    ms.Flush();
                    return ms;
                }
                catch (Exception ex)
                {
                    //
                }
            }
            return null;
        }
        #endregion

        #region 导出存货
        public static MemoryStream Export_CH(DataTable dt, int[] widths, string[] heads, string sheetName, string dwname, string ydate)
        {
            //先创建一个流
            MemoryStream ms = new MemoryStream();
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    //新建一个excel
                    HSSFWorkbook workbook = new HSSFWorkbook();
                    //excel样式
                    HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
                    //创建一个sheet
                    ISheet sheet = workbook.CreateSheet(sheetName);
                    //给指定sheet的内容设置每列宽度（index从0开始，width1000相当于excel设置的列宽3.81）
                    for (int i = 0; i < widths.Length; i++)
                    {
                        sheet.SetColumnWidth(i, widths[i]);
                    }

                    //第一行
                    var row1 = sheet.CreateRow(0);
                    row1.Height = 200;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row1.CreateCell(i);
                    }

                    //第二行
                    var row2 = sheet.CreateRow(1);
                    row2.Height = 600;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row2.CreateCell(i);
                    }
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));
                    ICell headcell = row2.GetCell(0);
                    headcell.SetCellValue(sheetName);
                    headcell.CellStyle = GetHeadStyle(workbook);


                    //第三行
                    var row3 = sheet.CreateRow(2);
                    row3.Height = 350;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row3.CreateCell(i);
                    }
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 8));
                    row3.GetCell(0).SetCellValue(ydate);
                    row3.GetCell(0).CellStyle = GetsubCenterHeadStyle(workbook);

                    //第四行
                    var row4 = sheet.CreateRow(3);
                    row4.Height = 350;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row4.CreateCell(i);
                    }
                    row4.GetCell(0).SetCellValue("单位名称：");
                    row4.GetCell(0).CellStyle = GetsubHeadStyle(workbook);
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 3));
                    row4.GetCell(1).SetCellValue(dwname);
                    row4.GetCell(1).CellStyle = GetsubHeadStyle(workbook);
                    row4.GetCell(8).SetCellValue("单位：元");
                    row4.GetCell(8).CellStyle = GetsubHeadStyle(workbook);

                    //第五行
                    var row5 = sheet.CreateRow(4);
                    row5.Height = 500;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell TitleCell = row5.CreateCell(i);
                        TitleCell.SetCellValue(heads[i]);
                        TitleCell.CellStyle = GetTitleStyle(workbook);
                    }

                    for (var r = 0; r < dt.Rows.Count; r++)
                    {
                        var row_r = sheet.CreateRow(r + 5);
                        row_r.Height = 500;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ICell itemCell = row_r.CreateCell(i);
                            itemCell.SetCellValue(dt.Rows[r][i].ToString());
                            if (i == 0 || i == 1 || i == 5 || i == 6 || i == 8)
                            {
                                itemCell.CellStyle = GetItemStyle1(workbook);
                            }
                            else if (i == 2 || i == 3 || i == 4)
                            {
                                if (dt.Rows[r][1].ToString() == "药房" || dt.Rows[r][1].ToString() == "药库" || dt.Rows[r][1].ToString() == "")
                                {
                                    itemCell.SetCellValue("");
                                }
                                itemCell.CellStyle = GetItemStyle2(workbook);
                            }
                            else
                            {
                                if (dt.Rows[r][6].ToString() == "药房" || dt.Rows[r][1].ToString() == "药库")
                                {
                                    itemCell.SetCellValue("");
                                }
                                itemCell.CellStyle = GetItemStyle2(workbook);
                            }
                        }
                    }

                    //末行
                    var rowLast = sheet.CreateRow(20);
                    rowLast.Height = 400;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        rowLast.CreateCell(i);
                    }
                    rowLast.GetCell(0).SetCellValue("院长签字：");
                    rowLast.GetCell(0).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(3).SetCellValue("会计签字：");
                    rowLast.GetCell(3).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(6).SetCellValue("填表人：");
                    rowLast.GetCell(6).CellStyle = GetfootStyle(workbook);

                    sheet.AddMergedRegion(new CellRangeAddress(5, 19, 0, 0));

                    sheet.AddMergedRegion(new CellRangeAddress(5, 15, 5, 5));

                    //写入流
                    workbook.Write(ms);
                    ms.Flush();
                    return ms;
                }
                catch (Exception ex)
                {
                    //
                }
            }
            return null;
        }
        #endregion

        #region 导出在建工程
        public static MemoryStream Export_Zjgc(DataTable dt, int[] widths, string[] heads, string sheetName, string dwname, string ydate)
        {
            //先创建一个流
            MemoryStream ms = new MemoryStream();
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    //新建一个excel
                    HSSFWorkbook workbook = new HSSFWorkbook();
                    //excel样式
                    HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
                    //创建一个sheet
                    ISheet sheet = workbook.CreateSheet(sheetName);
                    //给指定sheet的内容设置每列宽度（index从0开始，width1000相当于excel设置的列宽3.81）
                    for (int i = 0; i < widths.Length; i++)
                    {
                        sheet.SetColumnWidth(i, widths[i]);
                    }

                    //第一行
                    var row1 = sheet.CreateRow(0);
                    row1.Height = 200;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row1.CreateCell(i);
                    }

                    //第二行
                    var row2 = sheet.CreateRow(1);
                    row2.Height = 600;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row2.CreateCell(i);
                    }
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));
                    ICell headcell = row2.GetCell(0);
                    headcell.SetCellValue(sheetName);
                    headcell.CellStyle = GetHeadStyle(workbook);


                    //第三行
                    var row3 = sheet.CreateRow(2);
                    row3.Height = 350;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row3.CreateCell(i);
                    }
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 8));
                    row3.GetCell(0).SetCellValue(ydate);
                    row3.GetCell(0).CellStyle = GetsubCenterHeadStyle(workbook);

                    //第四行
                    var row4 = sheet.CreateRow(3);
                    row4.Height = 350;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row4.CreateCell(i);
                    }
                    row4.GetCell(0).SetCellValue("单位名称：");
                    row4.GetCell(0).CellStyle = GetsubHeadStyle(workbook);
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 3));
                    row4.GetCell(1).SetCellValue(dwname);
                    row4.GetCell(1).CellStyle = GetsubHeadStyle(workbook);
                    row4.GetCell(8).SetCellValue("单位：元");
                    row4.GetCell(8).CellStyle = GetsubHeadStyle(workbook);

                    //第五行
                    var row5 = sheet.CreateRow(4);
                    row5.Height = 500;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell TitleCell = row5.CreateCell(i);
                        TitleCell.SetCellValue(heads[i]);
                        TitleCell.CellStyle = GetTitleStyle(workbook);
                    }

                    for (var r = 0; r < dt.Rows.Count; r++)
                    {
                        var row_r = sheet.CreateRow(r + 5);
                        row_r.Height = 500;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ICell itemCell = row_r.CreateCell(i);
                            itemCell.SetCellValue(dt.Rows[r][i].ToString());
                            if (i == 0 || i == 1 || i == 5 || i == 6 || i == 8)
                            {
                                itemCell.CellStyle = GetItemStyle1(workbook);
                            }
                            else if (i == 2 || i == 3 || i == 4)
                            {
                                itemCell.CellStyle = GetItemStyle2(workbook);
                            }
                            else
                            {
                                itemCell.CellStyle = GetItemStyle2(workbook);
                            }
                        }
                    }

                    //末行
                    var rowLast = sheet.CreateRow(dt.Rows.Count + 5);
                    rowLast.Height = 400;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        rowLast.CreateCell(i);
                    }
                    rowLast.GetCell(0).SetCellValue("院长签字：");
                    rowLast.GetCell(0).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(3).SetCellValue("会计签字：");
                    rowLast.GetCell(3).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(6).SetCellValue("填表人：");
                    rowLast.GetCell(6).CellStyle = GetfootStyle(workbook);

                    sheet.AddMergedRegion(new CellRangeAddress(5, 9, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(5, 6, 5, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(8, 9, 5, 5));

                    //写入流
                    workbook.Write(ms);
                    ms.Flush();
                    return ms;
                }
                catch (Exception ex)
                {
                    //
                }
            }
            return null;
        }
        #endregion

        #region 导出应付账款
        public static MemoryStream Export_YF(DataTable dt, int[] widths, string[] heads, string sheetName, string dwname, string ydate)
        {
            //先创建一个流
            MemoryStream ms = new MemoryStream();
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    //新建一个excel
                    HSSFWorkbook workbook = new HSSFWorkbook();
                    //excel样式
                    HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
                    //创建一个sheet
                    ISheet sheet = workbook.CreateSheet(sheetName);
                    //给指定sheet的内容设置每列宽度（index从0开始，width1000相当于excel设置的列宽3.81）
                    for (int i = 0; i < widths.Length; i++)
                    {
                        sheet.SetColumnWidth(i, widths[i]);
                    }

                    //第一行
                    var row1 = sheet.CreateRow(0);
                    row1.Height = 200;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row1.CreateCell(i);
                    }

                    //第二行
                    var row2 = sheet.CreateRow(1);
                    row2.Height = 600;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row2.CreateCell(i);
                    }
                    //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));
                    ICell headcell = row2.GetCell(0);
                    headcell.SetCellValue(sheetName);
                    headcell.CellStyle = GetHeadStyle(workbook);


                    //第三行
                    var row3 = sheet.CreateRow(2);
                    row3.Height = 350;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row3.CreateCell(i);
                    }
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 8));
                    row3.GetCell(0).SetCellValue(ydate);
                    row3.GetCell(0).CellStyle = GetsubCenterHeadStyle(workbook);

                    //第四行
                    var row4 = sheet.CreateRow(3);
                    row4.Height = 350;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row4.CreateCell(i);
                    }
                    row4.GetCell(0).SetCellValue("单位名称：");
                    row4.GetCell(0).CellStyle = GetsubHeadStyle(workbook);
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 3));
                    row4.GetCell(1).SetCellValue(dwname);
                    row4.GetCell(1).CellStyle = GetsubHeadStyle(workbook);
                    row4.GetCell(8).SetCellValue("单位：元");
                    row4.GetCell(8).CellStyle = GetsubHeadStyle(workbook);

                    //第五行
                    var row5 = sheet.CreateRow(4);
                    row5.Height = 500;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell TitleCell = row5.CreateCell(i);
                        TitleCell.SetCellValue(heads[i]);
                        TitleCell.CellStyle = GetTitleStyle(workbook);
                    }

                    for (var r = 0; r < dt.Rows.Count; r++)
                    {
                        var row_r = sheet.CreateRow(r + 5);
                        row_r.Height = 500;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ICell itemCell = row_r.CreateCell(i);
                            itemCell.SetCellValue(dt.Rows[r][i].ToString());
                            if (i == 0 || i == 1 || i == 5 || i == 6 || i == 8)
                            {
                                itemCell.CellStyle = GetItemStyle1(workbook);
                            }
                            else if (i == 2 || i == 3 || i == 4)
                            {
                                if (dt.Rows[r][1].ToString() == "其中" || dt.Rows[r][1].ToString() == "")
                                {
                                    itemCell.SetCellValue("");
                                }
                                itemCell.CellStyle = GetItemStyle2(workbook);
                            }
                            else
                            {
                                if (dt.Rows[r][6].ToString() == "其中" || dt.Rows[r][6].ToString() == "")
                                {
                                    itemCell.SetCellValue("");
                                }
                                itemCell.CellStyle = GetItemStyle2(workbook);
                            }
                        }
                    }

                    //末行
                    var rowLast = sheet.CreateRow(dt.Rows.Count + 5);
                    rowLast.Height = 400;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        rowLast.CreateCell(i);
                    }
                    rowLast.GetCell(0).SetCellValue("院长签字：");
                    rowLast.GetCell(0).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(3).SetCellValue("会计签字：");
                    rowLast.GetCell(3).CellStyle = GetfootStyle(workbook);
                    rowLast.GetCell(6).SetCellValue("填表人：");
                    rowLast.GetCell(6).CellStyle = GetfootStyle(workbook);

                    sheet.AddMergedRegion(new CellRangeAddress(5, 10, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(5, 10, 5, 5));

                    //写入流
                    workbook.Write(ms);
                    ms.Flush();
                    return ms;
                }
                catch (Exception ex)
                {
                    //
                }
            }
            return null;
        }
        #endregion


        public static MemoryStream Export_Week(DateTime sday, List<TecCusServiceWeek> result)
        {
            //先创建一个流
            MemoryStream ms = new MemoryStream();
            //新建一个excel
            HSSFWorkbook workbook = new HSSFWorkbook();
            //excel样式
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
            //创建一个sheet
            ISheet sheet = workbook.CreateSheet();
            //给指定sheet的内容设置每列宽度（index从0开始，width1000相当于excel设置的列宽3.81）
            int columnCount = 14;
            for (int i = 0; i < columnCount; i++)
            {
                sheet.SetColumnWidth(i, 3000);
            }

            //第一行
            var row1 = sheet.CreateRow(0);
            row1.Height = 400;
            for (int i = 0; i < columnCount; i++)
            {
                row1.CreateCell(i);
            }
            //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 13));
            ICell headcell = row1.GetCell(0);
            headcell.SetCellValue(sday.ToString("yy年M月份周总结(" + sday.ToString("yyyy-MM-dd") + "~" + sday.AddDays(5).ToString("yyyy-MM-dd") + ")"));

            for (int i = 0; i < columnCount; i++)
            {
                row1.GetCell(i).CellStyle = GetHeadStyle(workbook);
                //headcell.CellStyle = GetHeadStyle(workbook);
            }

            //第二行
            var row2 = sheet.CreateRow(1);
            row2.Height = 400;
            for (int i = 0; i < columnCount; i++)
            {
                row2.CreateCell(i);
            }
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 2));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 3, 4));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 5, 6));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 7, 8));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 9, 10));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 11, 13));

            for (int i = 0; i < columnCount; i++)
            {
                row2.GetCell(i).CellStyle = GetHeadStyle(workbook);
                //headcell.CellStyle = GetHeadStyle(workbook);
            }

            ICell hc0 = row2.GetCell(0);
            hc0.SetCellValue("姓名");
            hc0.CellStyle = GetHeadStyle(workbook);

            ICell hc1 = row2.GetCell(1);
            hc1.SetCellValue(sday.ToString("ddd").Replace("周", "星期"));
            hc1.CellStyle = GetHeadStyle(workbook);

            ICell hc2 = row2.GetCell(3);
            hc2.SetCellValue(sday.AddDays(1).ToString("ddd").Replace("周", "星期"));
            hc2.CellStyle = GetHeadStyle(workbook);

            ICell hc3 = row2.GetCell(5);
            hc3.SetCellValue(sday.AddDays(2).ToString("ddd").Replace("周", "星期"));
            hc3.CellStyle = GetHeadStyle(workbook);

            ICell hc4 = row2.GetCell(7);
            hc4.SetCellValue(sday.AddDays(3).ToString("ddd").Replace("周", "星期"));
            hc4.CellStyle = GetHeadStyle(workbook);

            ICell hc5 = row2.GetCell(9);
            hc5.SetCellValue(sday.AddDays(4).ToString("ddd").Replace("周", "星期"));
            hc5.CellStyle = GetHeadStyle(workbook);

            ICell hc6 = row2.GetCell(11);
            hc6.SetCellValue(sday.AddDays(5).ToString("ddd").Replace("周", "星期"));
            hc6.CellStyle = GetHeadStyle(workbook);

            //第三行
            var row3 = sheet.CreateRow(2);
            row3.Height = 400;
            for (int i = 0; i < columnCount; i++)
            {
                row3.CreateCell(i);
            }
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 0, 0));

            for (int i = 1; i < 7; i++)
            {
                ICell hcyc = row3.GetCell(i * 2 - 1);
                hcyc.SetCellValue("远程");
                hcyc.CellStyle = GetItemStyle1(workbook);

                ICell hcsm = row3.GetCell(i * 2);
                hcsm.SetCellValue("上门");
                hcsm.CellStyle = GetItemStyle1(workbook);

            }

            if (result != null && result.Count > 0)
            {
                for (var r = 0; r < result.Count; r++)
                {
                    var row_r = sheet.CreateRow(r + 3);
                    row_r.Height = 400;
                    for (int i = 0; i < columnCount; i++)
                    {
                        ICell itemCell = row_r.CreateCell(i);
                        if (i == 0)
                        {
                            itemCell.SetCellValue(result[r].ZsName);
                        }
                        else if (i == 1)
                        {
                            itemCell.SetCellValue(result[r].yc1);
                        }
                        else if (i == 2)
                        {
                            itemCell.SetCellValue(result[r].sm1);
                        }
                        else if (i == 3)
                        {
                            itemCell.SetCellValue(result[r].yc2);
                        }
                        else if (i == 4)
                        {
                            itemCell.SetCellValue(result[r].sm2);
                        }
                        else if (i == 5)
                        {
                            itemCell.SetCellValue(result[r].yc3);
                        }
                        else if (i == 6)
                        {
                            itemCell.SetCellValue(result[r].sm3);
                        }
                        else if (i == 7)
                        {
                            itemCell.SetCellValue(result[r].yc4);
                        }
                        else if (i == 8)
                        {
                            itemCell.SetCellValue(result[r].sm4);
                        }
                        else if (i == 9)
                        {
                            itemCell.SetCellValue(result[r].yc5);
                        }
                        else if (i == 10)
                        {
                            itemCell.SetCellValue(result[r].sm5);
                        }
                        else if (i == 11)
                        {
                            itemCell.SetCellValue(result[r].yc6);
                        }
                        else if (i == 12)
                        {
                            itemCell.SetCellValue(result[r].sm6);
                        }
                        else
                        {
                            itemCell.SetCellValue("");
                        }
                        itemCell.CellStyle = GetItemStyle1(workbook);
                    }
                }
                sheet.AddMergedRegion(new CellRangeAddress(2, 2 + result.Count, 13, 13));
                row3.GetCell(13).CellStyle = GetItemStyle1(workbook);
            }
            //写入流
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }

        public static MemoryStream Export_Month(string Title, DataTable dt, string cCusText, string tCusText)
        {
            //先创建一个流
            MemoryStream ms = new MemoryStream();
            //新建一个excel
            HSSFWorkbook workbook = new HSSFWorkbook();
            //excel样式
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
            //创建一个sheet
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
            //给指定sheet的内容设置每列宽度（index从0开始，width1000相当于excel设置的列宽3.81）
            int[] headwidth = new int[] { 3000, 3000, 3000, 6000, 6000 };

            int columnCount = 5;
            for (int i = 0; i < columnCount; i++)
            {
                sheet.SetColumnWidth(i, headwidth[i]);
            }

            //第一行
            var row0 = sheet.CreateRow(0);
            row0.Height = 400;
            for (int i = 0; i < columnCount; i++)
            {
                row0.CreateCell(i);
            }
            //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, columnCount - 1));
            ICell headcell0 = row0.GetCell(0);
            headcell0.SetCellValue(Title);
            for (int i = 0; i < columnCount; i++)
            {
                row0.GetCell(i).CellStyle = GetHeadStyle(workbook);
            }

            string[] headAry = new string[] { "姓名", "远程次数", "上门次数", "服务次数多的单位", "服务超两次的单位" };
            //第一行
            var row1 = sheet.CreateRow(1);
            row1.Height = 400;
            for (int i = 0; i < columnCount; i++)
            {
                ICell headcell = row1.CreateCell(i);
                headcell.SetCellValue(headAry[i]);
                headcell.CellStyle = GetHeadStyle(workbook);
            }

            for (var r = 0; r < dt.Rows.Count; r++)
            {
                var row_r = sheet.CreateRow(r + 2);
                row_r.Height = 500;
                for (int i = 0; i < columnCount; i++)
                {
                    ICell itemCell = row_r.CreateCell(i);
                    if (i < dt.Columns.Count)
                    {
                        itemCell.SetCellValue(dt.Rows[r][i].ToString());
                    }
                    else
                    {
                        if (r == 0)
                        {
                            if (i == 3)
                            {
                                itemCell.SetCellValue(cCusText.Replace(',', '\n'));
                            }
                            else
                            {
                                itemCell.SetCellValue(tCusText.Replace(',', '\n'));
                            }
                        }
                        else
                        {
                            itemCell.SetCellValue("");
                        }
                    }
                    itemCell.CellStyle = GetItemStyle1(workbook);
                }
            }
            sheet.AddMergedRegion(new CellRangeAddress(2, dt.Rows.Count + 1, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(2, dt.Rows.Count + 1, 4, 4));

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                IRow row = HSSFCellUtil.GetRow(i, sheet);
                for (int j = 3; j <= 4; j++)
                {
                    ICell singleCell = HSSFCellUtil.GetCell(row, (short)j);
                    singleCell.CellStyle = GetItemStyle1(workbook);
                }
            }

            //写入流
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }

        public static void Create(string filepath, DataTable data)
        {
            Create(filepath, data, "Sheet1");
        }

        public static void Create(string filepath, DataTable data, string sheetname)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ICell cell;
            ISheet sheet = workbook.CreateSheet(sheetname);

            int columCount = data.Columns.Count;
            int rowCount = data.Rows.Count;

            for (int column = 0; column < columCount; column++)
            {
                cell = sheet.CreateRow(0).CreateCell(column);
                cell.SetCellValue(data.Rows[0][columCount].ToString());
            }
            for (int i = 1; i <= rowCount; i++)
            {
                for (int column = 0; column < columCount; column++)
                {
                    cell = sheet.CreateRow(i).CreateCell(column);
                    cell.SetCellValue(data.Rows[i][columCount].ToString());
                }
            }

            FileStream sw = File.Create(filepath);
            workbook.Write(sw);
            sw.Close();
        }

        public static List<string> GetSheetName(string filepath)
        {
            if (Path.GetExtension(filepath) == ".xls")
            {
                return GetSheetName_972003(filepath);
            }
            else
            {
                return GetSheetName_2007Up(filepath);
            }
        }

        private static List<string> GetSheetName_2007Up(string filepath)
        {
            List<string> names = new List<string>();

            XSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }

            int sheetcount = hssfworkbook.Count;
            for (int i = 0; i < sheetcount; i++)
            {
                string sheetname = hssfworkbook.GetSheetName(i);
                names.Add(sheetname);
            }
            return names;
        }

        private static List<string> GetSheetName_972003(string filepath)
        {
            List<string> names = new List<string>();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            int sheetcount = hssfworkbook.Count;
            for (int i = 0; i < sheetcount; i++)
            {
                string sheetname = hssfworkbook.GetSheetName(i);
                names.Add(sheetname);
            }
            return names;
        }

        public static DataTable Read(string filepath)
        {
            return Read(filepath, "Sheet1");
        }

        public static DataTable Read(string filepath, string sheetname)
        {
            if (Path.GetExtension(filepath) == ".xls")
            {
                return Read_972003(filepath, sheetname);
            }
            else
            {
                return Read_2007Up(filepath, sheetname);
            }
        }

        private static DataTable Read_972003(string filepath, string sheetname)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            ISheet sheet = hssfworkbook.GetSheet(sheetname);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                return dt;
            }
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                if (row.FirstCellNum < 0)
                {
                    continue;
                }
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        private static DataTable Read_2007Up(string filepath, string sheetname)
        {
            DataTable dt = new DataTable();

            XSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }

            ISheet sheet = hssfworkbook.GetSheet(sheetname);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                return dt;
            }
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                if (row.FirstCellNum < 0)
                {
                    continue;
                }
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        private static DataTable ReadEval_972003(string filepath, string sheetname)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            HSSFFormulaEvaluator evalor = new HSSFFormulaEvaluator(hssfworkbook);

            ISheet sheet = hssfworkbook.GetSheet(sheetname);
            sheet.ForceFormulaRecalculation = true;
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                return dt;
            }
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    ICell cell = row.GetCell(j);
                    if (cell != null)
                    {
                        if (cell.CellType == CellType.Formula)
                        {
                            var formulaValue = evalor.Evaluate(cell);
                            if (formulaValue.CellType == CellType.Numeric)
                            {
                                dataRow[j] = formulaValue.NumberValue;
                            }
                            else if (formulaValue.CellType == CellType.String)
                            {
                                dataRow[j] = formulaValue.StringValue;
                            }
                        }
                        else
                        {
                            dataRow[j] = cell.ToString();
                        }
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        private static DataTable ReadEval_2007Up(string filepath, string sheetname)
        {
            DataTable dt = new DataTable();

            XSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }

            XSSFFormulaEvaluator evalor = new XSSFFormulaEvaluator(hssfworkbook);

            ISheet sheet = hssfworkbook.GetSheet(sheetname);
            sheet.ForceFormulaRecalculation = true;
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                return dt;
            }
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    ICell cell = row.GetCell(j);
                    if (cell != null)
                    {
                        if (cell.CellType == CellType.Formula)
                        {
                            var formulaValue = evalor.Evaluate(cell);
                            if (formulaValue.CellType == CellType.Numeric)
                            {
                                dataRow[j] = formulaValue.NumberValue;
                            }
                            else if (formulaValue.CellType == CellType.String)
                            {
                                dataRow[j] = formulaValue.StringValue;
                            }
                        }
                        else
                        {
                            dataRow[j] = cell.ToString();
                        }
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }


    }
}
