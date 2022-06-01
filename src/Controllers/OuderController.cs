using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace zmdh
{
    public class OuderController : Controller
    {
        private readonly DBManager _context;

        public OuderController(DBManager context)
        {
            _context = context;
        }
    }
}
