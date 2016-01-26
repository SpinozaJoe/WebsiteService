using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebsiteCommon.Data;
using System.Linq;
using WebsiteCommon.Client;
using System.Diagnostics;
using System.Collections.Generic;
using WebsiteCommon.Applications;

namespace WebsiteService.Tests.Database
{
    [TestClass]
    public class WebsiteRepositoryTest
    {
        private IWebsiteRepository m_repository;

        [TestInitialize]
        public void Init()
        {
            WebsiteDataContext context = new WebsiteDataContext();

            m_repository = new WebsiteRepository(context);
        }

        [TestMethod]
        public void TestAllCustomers()
        {
            var customers = m_repository.GetCustomers();

            foreach (Customer customer in customers)
            {
                string s = customer.LastName;
            }

            Assert.IsNotNull(customers);
            Assert.IsTrue(customers.Count() > 0);
        }

        [TestMethod]
        public void TestSelectCustomer()
        {
            var customer = m_repository.GetCustomer(2);

            Assert.IsNotNull(customer);
            Assert.AreEqual(2, customer.Id);
            Assert.AreEqual("Crest", customer.LastName);
        }

        [TestMethod]
        public void TestSelectOrderedCustomers()
        {
            var customers = m_repository.GetSortedCustomers();

            Assert.IsNotNull(customers);
            Assert.AreEqual("Minerva", customers.FirstOrDefault().FirstName);
        }

        [TestMethod]
        public void TestCreateEnumeration()
        {
            var list = Enumerable.Range(0, 10)
                .Select(i => char.ConvertFromUtf32(32 + (i * 2)));

            foreach (var item in list)
            {
                Debug.WriteLine(item.ToString());
            }

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestCompareSequences()
        {
            var list1 = Enumerable.Range(0, 10);
            var list2 = Enumerable.Range(0, 10)
                .Select(i => i * i);

            var clist1 = Enumerable.Repeat(new Customer(), 10);
            var clist2 = Enumerable.Repeat(new Customer(), 10);

            foreach (var item in list1.Union(list2))
            {
                Debug.WriteLine(item.ToString());
            }

            Assert.IsNotNull(list1);
        }

        [TestMethod]
        public void TestConvertLists()
        {
            // Convert list to new anonymous type
            var customers = m_repository.GetSortedCustomers().Select(c => new
                {
                    Name = c.LastName + ", " + c.FirstName
                });

            foreach (var item in customers)
            {
                Debug.WriteLine(item.Name.ToString());
            }

            Assert.IsNotNull(customers);
        }

        [TestMethod]
        public void TestJoinLists()
        {
            // Create a new list of apps
            var apps = new List<EmailApplication>() {
                new EmailApplication() {Id = 1, CustomerId = 1, IPAccessRestrictions = "123.43.56.2"},
                new EmailApplication() {Id = 2, CustomerId = 1, IPAccessRestrictions = "123.43.56.36"},
                new EmailApplication() {Id = 3, CustomerId = 1},
                new EmailApplication() {Id = 4, CustomerId = 2},
                new EmailApplication() {Id = 5, CustomerId = 3, IPAccessRestrictions = "123.43.45.1"}
            };

            var customers = new List<Customer>();

            foreach (var customer in m_repository.GetSortedCustomers())
            {
//                customer.Applications = apps.Where(a => a.CustomerId == customer.Id).ToList();
                customers.Add(customer);
            }

            // Join customers to applications (with filter to those with IP restrictions)
            var joinQuery = customers.Join(apps,
                c => c.Id,
                a => a.CustomerId,
                (c, a) => new
                    {
                        Name = c.LastName + ", " + c.FirstName,
                        IpRestrictions = a.IPAccessRestrictions
                    }).Where(w => !String.IsNullOrEmpty(w.IpRestrictions));

            foreach (var item in joinQuery)
            {
                Debug.WriteLine(item.Name.ToString() + " : " + item.IpRestrictions);
            }

            // Try to get customers with IP access restrictions
            var manyQuery = customers
                .Select(c => c.Applications
                    .Where(a => !String.IsNullOrEmpty(a.IPAccessRestrictions)));

            foreach (var item in manyQuery)
            {
                Debug.WriteLine(item.ToString());
            }

            // The above query is an enumerable of an enumerable, which ain't great
            // Using SelectMany will flatten a parent/child relationship and allow projection on either
            var customersWithIpRestrictions = customers
                    .SelectMany(c => c.Applications
                    .Where(a => !String.IsNullOrEmpty(a.IPAccessRestrictions)),
                    // project to customer
                    (c, a) => c)
                    .Distinct();

            foreach (var item in customersWithIpRestrictions)
            {
                Debug.WriteLine(item.LastName + ", " + item.FirstName);
            }

            Debug.WriteLine(customers.Sum(c => 1));

            var groupBy = apps.GroupBy(a => a.CustomerId,
                         a => a.Id,
                         (key, id) => new
                         {
                             ClientId = key,
                             NumApps = id.Sum(a => 1),
                             MeanApps = id.Sum() / id.Sum(a => 1),
                             Avg = id.Average()
                         });

            foreach (var item in groupBy)
            {
                Debug.WriteLine(item.ClientId + " has this many apps: " + item.NumApps + " with mean: " + item.Avg);
            }

            var modeJoin = apps.Join(customers,
                                     a => a.CustomerId,
                                     c => c.Id,
                                     (a, c) => new
                                     {
                                         ClientName = c.LastName + ", " + c.FirstName,
                                         ApplicationId = a.Id
                                     });

            var modeQuery = modeJoin.GroupBy(e => e.ClientName)
                                .OrderByDescending(group => group.Count())
                                .Select(group => group.Key);

            Debug.WriteLine("ClientId with most apps: " + modeQuery.FirstOrDefault());

            Assert.IsNotNull(joinQuery);
        }

        [TestMethod]
        public void TestAddApplication()
        {
            Customer customer = m_repository.GetCustomer(1);
            EmailApplication app = new EmailApplication()
            {
                ApplicationType = ApplicationType.Blog
            };

            customer.Applications.Add(app);

            m_repository.Save();
        }

        [TestMethod]
        public void JustTest()
        {
            C obj = new C();
            Tungsten.Main();
        }

    }

    public class ApplicationDetails
    {
        public string Name { get; set; }
        public int? CustomerId { get; set; }
    }
}
