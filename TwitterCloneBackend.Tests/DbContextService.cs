using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterCloneBackend.Entities;

namespace TwitterCloneBackend.Tests
{
    public static class DbContextService
    {
        public static readonly DataContext _dataContext;
        static DbContextService()
        {
            // Initialize a new instance of DataContext
            // This will create an in-memory database for testing
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dataContext = new DataContext(options);
        }
    }
}
