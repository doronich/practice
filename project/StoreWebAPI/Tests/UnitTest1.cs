using System;
using DAL.Context;
using DAL.Entities;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests
{
    public class UnitTest1 {
        private readonly Repository<User> m_repository;
        ApplicationContext context = new ApplicationContext(new DbContextOptions<ApplicationContext>());

        public UnitTest1() {
            this.m_repository = new Repository<User>();
        }

    }
}
