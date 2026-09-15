using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;

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

        // 1.Declare Variable
        string fullName = "Khetsophon Ainautoon", roomNumber = "L001", programlanguage = "HTML, CSS, Javascript, JAVA";
        int year = 3;
        ViewBag.fullName = fullName;
        ViewBag.roomNumber = roomNumber;
        ViewBag.programlanguage = programlanguage;
        ViewBag.year = year;

        // 2.Score Array
        int[] scores = { 0, 0, 10, 0, 0, 0, 0, 10, 10, 10 };

        // 3.Calculate Total
        int totalScore = scores.Sum();

        // 4.Compare
        string grade = "";
        if(totalScore >= 80) grade = "A";
        else if(totalScore >= 76) grade = "B";
        else if(totalScore >= 70) grade = "B";
        else if(totalScore >= 66) grade = "C+";
        else if(totalScore >= 60) grade = "C";
        else if(totalScore >= 56) grade = "D+";
        else if(totalScore >= 50) grade = "D";
        else grade = "F";

        ViewBag.Grade = grade;

        // 5.Additional
        List<int> workFaileds = new List<int>();
        if (grade == "F")
        {
            for (int i = 0 ; i < scores.Length ; i++)
            {
                if (scores[i] < 5)
                {
                    // plus one because normal person they will confuse why start with 0 if not + 1 it will start with 0 1 2...
                    workFaileds.Add(i + 1);
                }
            }
        }
        ViewBag.workFaileds = workFaileds;


        return View();
    }
    public IActionResult Lab4(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
