using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Sand.Domain.Uow;
using Sand.Service.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Service.Domain;
using Test.Service.Service;
using Xunit;

namespace Sand.Test
{
    public class ServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadUnitOfWork _readUnitOfWork;
        private readonly IUsersRepository _usersRepository;
        private readonly IUsersService _usersService;
        public ServiceTest()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _readUnitOfWork = Substitute.For<IReadUnitOfWork>();
            _usersRepository = Substitute.For<IUsersRepository>();
            _usersService = Substitute.For<IUsersService>();
        }

        [Fact]
        public async Task TestUserAdd()
        {
            var query = new UsersQuery() { UserName = "测试" };
            var data= await _usersService.RetrieveAsync(query);
            if (data.FirstOrDefault()!=null)
            {
                Assert.True(data.First().UserName == "测试");
            }
        }
    }
}