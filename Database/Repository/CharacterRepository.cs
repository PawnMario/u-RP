using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using uRP.Database.Model;

namespace uRP.Database.Repository
{
    class CharacterRepository : IRepository<Character>
    {
        private readonly Context _context = new Context();

        public void Add(Character entity)
        {
            _context.Characters.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Character> GetAll()
        {
            return _context.Characters;
        }

        public Character GetByID(long id)
        {
            return _context.Characters.Find(id);
        }

        public void Remove(Character entity)
        {
            _context.Characters.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Character entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
