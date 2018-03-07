using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using PalindromeApi.Controllers;
using Xunit;

namespace Palindrome.UnitTests.Controllers
{
    public class ErrorControllerTests
    {
        private readonly ErrorController _errorController;

        public ErrorControllerTests()
        {
            _errorController = new ErrorController();
        }

        [Fact]
        public void returns_ok_if_no_exception_in_pipeline()
        {
            _errorController.ControllerContext.HttpContext = new DefaultHttpContext();
            var response = Assert.IsType<OkResult>(_errorController.Error());
        }
        
        [Fact]
        public void returns_internal_server_error_if_exception_in_pipeline()
        {
            var featuresCollection = new FeatureCollection();
            featuresCollection.Set<IExceptionHandlerFeature>(new ExceptionHandlerFeature { Error = new Exception("Some  Exception") });

            _errorController.ControllerContext.HttpContext = new DefaultHttpContext(featuresCollection);
            var response = Assert.IsType<StatusCodeResult>(_errorController.Error());

            Assert.Equal(response.StatusCode, (int)HttpStatusCode.InternalServerError);
        }
    }
}