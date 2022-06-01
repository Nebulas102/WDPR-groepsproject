using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace zmdh.Controllers
{
    public class ReportController : Controller
    {
        public readonly DBManager _context;
        private UserManager<ApplicationUser> _userManager;

        public ReportController(DBManager context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Report
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {
            var dBManager = _context.Reports.Include(r => r.Handler).Include(r => r.Message);
            return View(await dBManager.ToListAsync());
        }

        //de anonieme reports basis op chat id
        [Authorize(Roles = "Admin,Moderator")]
        [Route("Report/HandleMessageReports/{chatId}")]
        public IActionResult HandleMessageReports(int chatId)
        {
            var messages = _context.Messages.Where(m => m.ChatId == chatId)
                .Include(m => m.Author)
                .Include(m => m.Chat)
                .Include(m => m.Reports)
                .ToList();

            return View(messages);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [Route("Report/HandleMessageReports/{chatId}/{messageId}")]
        public IActionResult HandleReports(int chatId, int messageId)
        {
            var messages = _context.Messages.Where(m => m.ChatId == chatId && m.Id == messageId)
            .Include(m => m.Reports).ThenInclude(r => r.Handler)
            .Include(m => m.Reports).ThenInclude(r => r.Message)
            .ToList();

            return View(messages);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [Route("Report/HandleMessageReports/{chatId}/{messageId}/{reportId}")]
        public async Task<IActionResult> HandleReport(int chatId, int messageId, int reportId)
        {
            ReportViewModel reportViewModel = new ReportViewModel();

            var report = await _context.Messages.Where(m => m.ChatId == chatId && m.Id == messageId)
            .Include(m => m.Reports).ThenInclude(r => r.Message).ThenInclude(m => m.Author)
            .Select(m => m.Reports.Where(r => r.Id == reportId).SingleOrDefault())
            .SingleOrDefaultAsync();

            var currentReportMessage = await _context.Reports.Where(r => r.Id == reportId && r.MessageId == messageId)
            .Include(r => r.Message).ThenInclude(m => m.Author)
            .Select(r => r.Message)
            .SingleOrDefaultAsync();

            var applicationUserChat = await _context.ApplicationUserChats.Where(auc => auc.ApplicationUserId == currentReportMessage.AuthorId
            && auc.ChatId == chatId)
            .Include(auc => auc.ApplicationUser)
            .Include(auc => auc.Chat)
            .SingleOrDefaultAsync();

            reportViewModel.Report = report;
            reportViewModel.ApplicationUserChat = applicationUserChat;

            Console.WriteLine(reportViewModel.ApplicationUserChat.ApplicationUser.Email);

            ViewData["currentUser"] = await _userManager.GetUserAsync(HttpContext.User);

            return View(reportViewModel);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeApplicationUserChatStatus(string userId, int chatId, [Bind("ApplicationUserChat")] ReportViewModel reportViewModel)
        {
            var applicationUserChat = await _context.ApplicationUserChats.Where(auc => auc.ApplicationUserId == userId
            && auc.ChatId == chatId).SingleOrDefaultAsync();

            reportViewModel.ApplicationUserChat = applicationUserChat;

            if (ModelState.IsValid)
            {
                try
                {
                    if (reportViewModel.ApplicationUserChat.ApplicationUserChatStatus == "active")
                    {
                        reportViewModel.ApplicationUserChat.ApplicationUserChatStatus = "blocked";
                    }
                    else
                    {
                        reportViewModel.ApplicationUserChat.ApplicationUserChatStatus = "active";
                    }
                    _context.Update(reportViewModel.ApplicationUserChat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index", "Report");
            }
            return View(reportViewModel);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleMessageReport([Bind("Report")] ReportViewModel reportViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportViewModel.Report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(reportViewModel.Report.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Report");
            }
            return View(reportViewModel);
        }

        // GET: Report/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Handler)
                .Include(r => r.Message)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Report/Create
        public IActionResult Create()
        {
            ViewData["HandlerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Content");
            return View();
        }

        // POST: Report/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,isHandled,MessageId,HandlerId")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HandlerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", report.HandlerId);
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Content", report.MessageId);
            return View(report);
        }

        // GET: Report/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["HandlerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", report.HandlerId);
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Content", report.MessageId);
            return View(report);
        }

        // POST: Report/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,isHandled,MessageId,HandlerId")] Report report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HandlerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", report.HandlerId);
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Content", report.MessageId);
            return View(report);
        }

        // GET: Report/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Handler)
                .Include(r => r.Message)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
