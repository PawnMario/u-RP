using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using uRP.Database.Model;

namespace uRP.Database.Repository
{
    class AccountRepository : IRepository<Account>
    {

        private readonly Context _context = new Context();

        public void Add(Account entity)
        {
            _context.Players.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Players;
        }

        public Account GetByID(long id)
        {
            return _context.Players.Find(id);
        }

        public void Remove(Account entity)
        {
            _context.Players.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Account entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<Character> GetPlayerCharacters(Account entity)
        {
            return _context.Characters.Where(x => x.gid == entity.member_id);
        }


        /* //If for example you want all players from a certain faction, you could do this:
        public IEnumerable<Player> GetByFactionId(int id)
        {
            return _context.Players.Where(x => x.FactionId == id);
        }
        */
    }
}
