using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suraj.DbContexts;
using Suraj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suraj.Controllers
{
    public class EmployeeController : Controller
    {
        SurajKumarContext Db = new SurajKumarContext();

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserInfo lg)
        {
            var userln = Db.UserInfos.Where(u => u.Email == lg.Email).FirstOrDefault();

            if (userln == null)
            {
                TempData["Invalid"] = "Your Details Invalid";
            }
            else
            {
                if (userln.Name == lg.Name && userln.Email == lg.Email && userln.Password == lg.Password)
                {
                    return RedirectToAction("Table");
                }
                else
                {
                    TempData["Invalid"] = "Your Details Invalid";
                    return View();
                }
            }
            return View();
        }
        

        public IActionResult NewAccount()
        {
            return View();
        }
        [HttpPost]
         public IActionResult NewAccount(UserInfo a)
        {
            UserInfo newac = new UserInfo();

            newac.Name = a.Name;
            newac.Email = a.Email;
            newac.Address = a.Address;
            newac.Dept = a.Dept;
            newac.Password = a.Password;

            Db.UserInfos.Add(newac);
            Db.SaveChanges();

            return RedirectToAction("");
        }

        public IActionResult Table()
        {
            var data = Db.Employees.ToList();

            List<EmployeeModel> empmodel = new List<EmployeeModel>();

            foreach (var item in data)
            {
                empmodel.Add(new EmployeeModel
                {
                    Id= item.Id,
                    Name = item.Name,
                    Email=item.Email,
                    Address=item.Address,
                    Company=item.Company,
                    Dept=item.Dept,
                    Salary=item.Salary
                });
            }
            return View(empmodel);
        }
        public IActionResult AddDetails()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddDetails(EmployeeModel add)
        {
            Employee objnew = new Employee();
            objnew.Id = add.Id;
            objnew.Name = add.Name;
            objnew.Email = add.Email;
            objnew.Address = add.Address;
            objnew.Company = add.Company;
            objnew.Dept = add.Dept;
            objnew.Salary = add.Salary;
            if (add.Id == 0)
            {
                Db.Employees.Add(objnew);
                Db.SaveChanges();
            }
            else
            {
                Db.Entry(objnew).State = EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("Table");

            //return View();
        }
       
        public IActionResult Delete(int id)
        {
            SurajKumarContext dobj = new SurajKumarContext();
            var ditem = dobj.Employees.Where(m => m.Id == id).First();
            dobj.Employees.Remove(ditem);
            dobj.SaveChanges();

            return RedirectToAction("Table");
        }
       
        public IActionResult Edit(int id)
        {
            SurajKumarContext eobj = new SurajKumarContext();
            var uitem = eobj.Employees.Where(m => m.Id == id).FirstOrDefault();
            EmployeeModel mobj =  new EmployeeModel();
            mobj.Id = uitem.Id;
            mobj.Name = uitem.Name;
            mobj.Email = uitem.Email;
            mobj.Address = uitem.Address;
            mobj.Company = uitem.Company;
            mobj.Dept = uitem.Dept;
            mobj.Salary = uitem.Salary;


            return View("AddDetails", mobj);
        }

        private EmployeeModel EmployeeModel()
        {
            throw new NotImplementedException();
        }
    }


}
