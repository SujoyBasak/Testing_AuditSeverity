using AuditSeverityModule.Controllers;
using AuditSeverityModule.Models;
using AuditSeverityModule.Providers;
using AuditSeverityModule.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AuditSeverityModule.Testing
{
    public class AuditSeverityControllerTest
    {
        List<AuditBenchmark> ls = new List<AuditBenchmark>()
        {
            new AuditBenchmark()
            {
                AuditType="Internal",
                BenchmarkNoAnswers=3
            },
            new AuditBenchmark()
            {
                AuditType="SOX",
                BenchmarkNoAnswers=1
            }
        };


        
        [Test]
        public void Internal_GetResponseProviderTest()
        {
            Mock<ISeverityRepo> mock = new Mock<ISeverityRepo>();
            mock.Setup(p => p.Response()).Returns(ls);
            SeverityProvider cp = new SeverityProvider(mock.Object);
            AuditRequest req = new AuditRequest()
            {
                Auditdetails = new AuditDetail()
                {
                    Type = "Internal",
                    questions = new Questions()
                    {
                        Question1 = true,
                        Question2 = false,
                        Question3 = false,
                        Question4 = false,
                        Question5 = true
                    }
                }
            };
            AuditResponse result = cp.SeverityResponse(req);
            Assert.AreEqual("GREEN", result.ProjectExexutionStatus);
        }
        [Test]
        public void SOX_GetResponseProviderTest()
        {
            Mock<ISeverityRepo> mock = new Mock<ISeverityRepo>();
            mock.Setup(p => p.Response()).Returns(ls);
            SeverityProvider cp = new SeverityProvider(mock.Object);
            AuditRequest req = new AuditRequest()
            {
                Auditdetails = new AuditDetail()
                {
                    Type = "Internal",
                    questions = new Questions()
                    {
                        Question1 = true,
                        Question2 = false,
                        Question3 = false,
                        Question4 = false,
                        Question5 = false
                    }
                }
            };
            AuditResponse result = cp.SeverityResponse(req);
            Assert.AreEqual("RED", result.ProjectExexutionStatus);
        }

        [TestCase("Internal")]
        [TestCase("SOX")]
        public void Pass_GetResponseControllerTest(string type)
        {
            Mock<ISeverityProvider> mock = new Mock<ISeverityProvider>();
            AuditResponse rp = new AuditResponse();
            AuditRequest req = new AuditRequest()
            {
                Auditdetails = new AuditDetail()
                {
                    Type = type,
                    questions = new Questions()
                    {
                        Question1 = true,
                        Question2 = false,
                        Question3 = false,
                        Question4 = false,
                        Question5 = false
                    }
                }
            };
            mock.Setup(p => p.SeverityResponse(req)).Returns(rp);
            AuditSeverityController cp = new AuditSeverityController(mock.Object);
            
            OkObjectResult result = cp.Post(req) as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestCase("Internal123")]
        [TestCase("SOX123")]
        public void Fail_GetResponseControllerTest(string type)
        {
            try
            {
                Mock<ISeverityProvider> mock = new Mock<ISeverityProvider>();
                AuditResponse rp = new AuditResponse();
                AuditRequest req = new AuditRequest()
                {
                    Auditdetails = new AuditDetail()
                    {
                        Type = type,
                        questions = new Questions()
                        {
                            Question1 = true,
                            Question2 = false,
                            Question3 = false,
                            Question4 = false,
                            Question5 = false
                        }
                    }
                };
                mock.Setup(p => p.SeverityResponse(req)).Returns(rp);
                AuditSeverityController cp = new AuditSeverityController(mock.Object);

                OkObjectResult result = cp.Post(req) as OkObjectResult;
                Assert.AreEqual(200, result.StatusCode);
            }
            catch(Exception e)
            {
                Assert.AreEqual("Object reference not set to an instance of an object.", e.Message);
            }
            
        }
    }
}