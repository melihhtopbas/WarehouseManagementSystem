using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class ExportController : Controller
    {
        // GET: Admin/Export
        public ActionResult Index(int orderId)
        {
           WarehouseManagementSystemEntities1 _context = new WarehouseManagementSystemEntities1();
            var model = _context.Orders.Where(x=>x.Id== orderId).ToList();
            return View(model); 
        }
        public ActionResult PdfAndExcel()
        {
            return PartialView("~/Areas/Admin/Views/Export/PdfAndExcel.cshtml");
        }
        [HttpPost]
        public ActionResult ExportExcel()
        {
            WarehouseManagementSystemEntities1 entities = new WarehouseManagementSystemEntities1();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Müşteri Id"),
                                            new DataColumn("Gönderici Adı"),
                                            new DataColumn("Gönderici Telefonu"),
                                            new DataColumn("Alıcı Adı"),
                                            new DataColumn("Alıcı Telefonu"),
                                            });

            var orders = from order in entities.Orders
                            select order;

            foreach (var order in orders)
            {
                dt.Rows.Add(order.Id, order.SenderName,order.SenderPhone, order.RecipientName,order.RecipientPhone, order.Cities.Name,order.Countries.Name);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }
    }
}