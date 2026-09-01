using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;
using Microsoft.VisualBasic;

namespace Khetsophon_CSI402_8456.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Lab2()
    {
        // char bloodType = 'a', Grade = 'F',Test;
        // string fullName = "Khetsophon Ainautoon";
        // int age = 21,x,y,sum;
        // int roomNumber = 5-1010;
        // int year = 3;
        // string[] fluentProgramlanguage = {"Thai, English"};
        // int[] scores = {1};

        // x = 2;
        // y = 49;
        // sum = x + y;
        // // sum = x - y;
        // // sum = x * y;
        // // sum = x / y;
        // // sum = x % y;
        // // sum = x = y;

        // if(sum > 100) {
        //     ViewBag.sum=sum;
        // }
        // else {
        //     ViewBag.sum = "i ngo i want number more than 100";
        // }

        // List<string> names = new List<string> {"A", "B", "C", "D", "F"};

        // foreach(name in names){
        //     <p>@name</p>
        // }

        string fullName = "Khetsophon Ainautoon", roomNumber = "L001", programlanguage = "HTML, CSS, Javascript, JAVA";
        int year = 3, s1=0, s2=0, s3=10, s4=0, s5=0, s6=0, s7=0 ,s8=10, s9=10, s10=10, sum;
    
        sum = s1+s2+s3+s4+s5+s6+s7+s8+s9+s10;


        if(sum >= 80) {
            ViewBag.fullName = fullName;
            ViewBag.roomNumber = roomNumber;
            ViewBag.programlanguage = programlanguage;
            ViewBag.year = year;
            ViewBag.sum = "Your grade is A";
        } 
        else if(sum >= 76) {
            ViewBag.fullName = fullName;
            ViewBag.roomNumber = roomNumber;
            ViewBag.programlanguage = programlanguage;
            ViewBag.year = year;
            ViewBag.sum = "Your grade is B+";
        } else if(sum >= 70) {
            ViewBag.fullName = fullName;
            ViewBag.roomNumber = roomNumber;
            ViewBag.programlanguage = programlanguage;
            ViewBag.year = year;
            ViewBag.sum = "Your grade is B";
        }
        else if(sum >= 66) {
            ViewBag.fullName = fullName;
            ViewBag.roomNumber = roomNumber;
            ViewBag.programlanguage = programlanguage;
            ViewBag.year = year;
            ViewBag.sum = "Your grade is C+";
        } else if(sum >= 60) {
            ViewBag.fullName = fullName;
            ViewBag.roomNumber = roomNumber;
            ViewBag.programlanguage = programlanguage;
            ViewBag.year = year;
            ViewBag.sum = "Your grade is C";
        }
        else if(sum >= 56) {
            ViewBag.fullName = fullName;
            ViewBag.roomNumber = roomNumber;
            ViewBag.programlanguage = programlanguage;
            ViewBag.year = year;
            ViewBag.sum = "Your grade is D+";
        } else if(sum >= 50) {
            ViewBag.fullName = fullName;
            ViewBag.roomNumber = roomNumber;
            ViewBag.programlanguage = programlanguage;
            ViewBag.year = year;
            ViewBag.sum = "Your grade is D";
        } else{
            ViewBag.fullName = fullName;
            ViewBag.roomNumber = roomNumber;
            ViewBag.programlanguage = programlanguage;
            ViewBag.year = year;
            ViewBag.sum = "Your grade is F";
            
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
