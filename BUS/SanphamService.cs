using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SanphamService

    {
        public List<Sanpham> GetAll()
        {
            SanphamModel context = new SanphamModel();
            return context.Sanpham.ToList();
        }
        public void InsertUpdate(Sanpham sp)
        {
            SanphamModel context = new SanphamModel();
            context.Sanpham.AddOrUpdate(sp);
            context.SaveChanges();
        }
        public Sanpham FindById(string id)
        {
            SanphamModel context = new SanphamModel();
            return context.Sanpham.FirstOrDefault(p => p.MaSP == id);
        }
    }
}
