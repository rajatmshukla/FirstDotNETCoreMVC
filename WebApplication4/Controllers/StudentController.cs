﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentContext _Db;
        public StudentController(StudentContext Db)
        {
            _Db = Db;
        }
        public IActionResult StudentList()
        {
            try
            {
                var stdList = from a in _Db.tbl_Student
                              join b in _Db.Dept
                              on a.DeptID equals b.ID into Dep
                              from b in Dep.DefaultIfEmpty()

                              select new Student
                              {
                                  ID = a.ID,
                                  Name = a.Name,
                                  FName = a.FName,
                                  Mobile = a.Mobile,
                                  Email = a.Email,
                                  Description = a.Description,
                                  DeptID = a.DeptID,

                                  Department = b == null ? "" : b.Department
                              };


                return View(stdList);
            }
            catch (Exception ex)
            {

                return View();
            }


        }

        public IActionResult Create(Student obj)
        {
            loadDDL();
            return View(obj);
        }


        [HttpPost]
        public async Task<IActionResult> AddStudent(Student obj)
        {
            try
            {
                //if(ModelState.IsValid)
                {
                    if (obj.ID == 0)
                    {
                        _Db.tbl_Student.Add(obj);
                        await _Db.SaveChangesAsync();
                    }
                    else
                    {
                        _Db.Entry(obj).State = EntityState.Modified;
                        await _Db.SaveChangesAsync();
                    }


                    return RedirectToAction("StudentList");
                }


                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("StudentList");
            }

        }

        [HttpGet]
        public async Task<IActionResult> StudentList(string Stdsearch)
        {
            ViewData["GetStudentDetails"] = Stdsearch;
            var Stdquery = from a in _Db.tbl_Student
                           join b in _Db.Dept
                           on a.DeptID equals b.ID into Dep
                           from b in Dep.DefaultIfEmpty()
                           select new Student
                           {
                               ID = a.ID,
                               Name = a.Name,
                               FName = a.FName,
                               Mobile = a.Mobile,
                               Email = a.Email,
                               Description = a.Description,
                               DeptID = a.DeptID,

                               Department = b == null ? "" : b.Department
                           };
            if (!string.IsNullOrEmpty(Stdsearch))
            {
                Stdquery = Stdquery.Where(x => x.Name.Contains(Stdsearch));
            }
            return View(await Stdquery.AsNoTracking().ToListAsync());


        }


        //public async Task<IActionResult> Search(string searchString)
        //{
          
        //        var srh = from m in _Db.tbl_Student
        //                     select m;
        //        if (!String.IsNullOrEmpty(searchString))
        //        {
        //            srh = srh.Where(s => s.Name.Contains(searchString));

        //        }

        //        return View(await srh.ToListAsync());
        //}



        public async Task<IActionResult> DeleteStd(int id)
        {
            try
            {
                var std = await _Db.tbl_Student.FindAsync(id);
                if(std!=null)
                {
                    _Db.tbl_Student.Remove(std);
                    await _Db.SaveChangesAsync();
                }


                return RedirectToAction("StudentList");
            }
            catch (Exception ex)
            {
                return RedirectToAction("StudentList");
            }
        }

        private void loadDDL()
        {
            try
            {
                List<Departments> depList = new List<Departments>();
                depList = _Db.Dept.ToList();

                depList.Insert(0, new Departments { ID = 0, Department = "Please Select" });

                ViewBag.DepList = depList;


            }
            catch (Exception ex)
            {


            }
        }


    }
}
