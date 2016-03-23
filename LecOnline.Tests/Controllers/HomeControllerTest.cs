// -----------------------------------------------------------------------
// <copyright file="HomeControllerTest.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Tests.Controllers
{
    using System.Web.Mvc;
    using LecOnline.Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the home controller.
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        /// <summary>
        /// Test for the main page.
        /// </summary>
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            // Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests for the about page.
        /// </summary>
        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            // Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        /// <summary>
        /// Tests for the contact page.
        /// </summary>
        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
