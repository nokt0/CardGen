using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Controllers
{
    public class CardsController : Controller
    {
        private readonly UserCardDbContext _context;

        public CardsController(UserCardDbContext context)
        {
            _context = context;

        }

        // GET: Cards
        public IActionResult Index()
        {
            var currentUser = User.Identity.Name;
            var cardIdList = _context.UserCard.Where(item => item.User == currentUser).ToList();
            List<int> ids = new List<int>();
            foreach(var userCard in cardIdList)
            {
                ids.Add(userCard.CardId);
            }
            var cards = _context.Card.Where(card => ids.Contains(card.Id)).ToList();

            return View(cards);
        }

        // GET: Cards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Card
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // GET: Cards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubType,Cost,Text,Rarity,Type,Quality,Abilities")] Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();

                var cardFromDb = _context.Card.Single(item => 
                item.SubType == card.SubType &&
                item.Cost == card.Cost &&
                item.Text == card.Text &&
                item.Rarity == card.Rarity &&
                item.Type == card.Type &&
                item.Quality == card.Quality &&
                item. Abilities == card.Abilities);

                _context.UserCard.Add(new UserCard
                {
                    Id = 0,
                    CardId = cardFromDb.Id,
                    User = User.Identity.Name
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: Cards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Card.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubType,Cost,Text,Rarity,Type,Quality,Abilities")] Card card)
        {
            if (id != card.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.Id))
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
            return View(card);
        }

        // GET: Cards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Card
                .FirstOrDefaultAsync(m => m.Id == id);


            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Card.FindAsync(id);
            _context.Card.Remove(card);
            var transition = _context.UserCard.Single(item =>
            item.CardId == id &&
            item.User == User.Identity.Name);

            _context.UserCard.Remove(transition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Transfer(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var card = await _context.Card
                .FirstOrDefaultAsync(m => m.Id == id);

            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        [HttpPost, ActionName("Transfer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(IFormCollection collection)
        {

            int id;
            if (!Int32.TryParse(collection["id"].First(), out id))
            {
                return NotFound();
            }
            var username = collection["user"].First();

            var card = await _context.Card
                .FirstOrDefaultAsync(m => m.Id == id);

            if (card == null)
            {
                return NotFound();
            }

            var transition = await _context.UserCard.FirstOrDefaultAsync(item => item.CardId == id && item.User == username);

            if (transition == null)
            {
                return NotFound();
            }

            transition.User = username;
            _context.Update(transition);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

            private bool CardExists(int id)
        {
            return _context.Card.Any(e => e.Id == id);
        }


    }
}
