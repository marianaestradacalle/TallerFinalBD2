using Application;
using ArchUnitNET.Domain;
using Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using NetArchTest.Rules;
using RestApiService.ServiceName.Configuration;
using System.Reflection;

namespace Architecture.Tests
{
    public class CheckArchitectureRules
    {
        [Fact]
        public void DomainShouldNotHaveDependencies()
        {
            //Act
            var result = Types.InCurrentDomain().That().ResideInNamespace("Core")
                 .Should().OnlyHaveDependenciesOn("Core", "AutoMapper", "FluentValidation", "System", "MongoDB").GetResult();

            //Assert
            Assert.True(result.IsSuccessful, "La capa Core no debe tener ninguna dependencia con otras capas");
        }
        [Fact]
        public void ApplicationOnlyShouldDependOnCore()
        {
            //Act
            var result = Types.InCurrentDomain().That().ResideInNamespace("Application")
                 .Should().OnlyHaveDependenciesOn("Core", "Common", "System", "Application", "AutoMapper", "Microsoft", "MongoDB").GetResult();

            //Assert
            Assert.True(result.IsSuccessful, "La capa Application debe depender unicamente de Core");
        }

        [Fact]
        public void TypesInApplicationInterfacesFolderOnlyShouldBeInterfaces()
        {
            //Act
            var result = Types.InCurrentDomain()
                 .That()
                 .ResideInNamespace("Application.Interfaces")
                 .Should().BeInterfaces()
                 .GetResult();

            var result2 = Types.InCurrentDomain()
                 .That()
                 .ResideInNamespace("Application.Interfaces")
                 .Should().HaveNameStartingWith("I")
                 .GetResult();

            //Assert
            Assert.True(result.IsSuccessful && result2.IsSuccessful, "Todos los tipos de Application.Interfaces deben ser interfaces y respetar la guia de estilos para Interfaces");

        }
        [Fact]
        public void ApplicationShouldNotHaveMoreFolders()
        {
            //Act
            var result = Types.InCurrentDomain()
             .That()
             .ResideInNamespace("Application")
             .Should().ResideInNamespace("Application.Common")
             .Or().ResideInNamespace("Application.DTOs")
             .Or().ResideInNamespace("Application.Interfaces")
             .Or().ResideInNamespace("Application.Services")
             .Or().HaveName("ApplicationDependencyInjection")
             .GetResult();

            //Assert
            Assert.True(result.IsSuccessful, "Application no debe tener mas carpetas de las inicialmente especificadas");
        }
        [Fact]
        public void CoreShouldNotHaveMoreFolders()
        {
            //Act
            var result = Types.InCurrentDomain()
             .That()
             .ResideInNamespace("Core")
             .Should().ResideInNamespace("Core.Entities")
             .Or().ResideInNamespace("Core.Enumerations")
             .GetResult();

            //Assert
            Assert.True(result.IsSuccessful, "Core no debe tener mas carpetas de las inicialmente especificadas");

        }
        [Fact]
        public void InfrastructureShouldNotHaveMoreFolders()
        {
            //Act
            var result = Types.InCurrentDomain()
            .That()
            .ResideInNamespace("Infrastructure")
            .Should().ResideInNamespace("Infrastructure.Services")
            .Or().HaveName("InfrastructureDependencyInjection")
            .GetResult();

            //Assert
            Assert.True(result.IsSuccessful, "Infrastructure no debe tener mas carpetas de las inicialmente especificadas");
        }


        //Dependencias circulares visual studio no deja particularmente establecerlas
        [Fact]
        public void InfrastructureShouldNotDependOnEntryAssembly()
        {
            //Arrange
            var assemblyName = typeof(ServicesConfiguration).Assembly.GetName().ToString();

            //Act
            var result = Types.InCurrentDomain()
           .That().ResideInNamespace("Infrastructure")
           .ShouldNot().HaveDependencyOn(assemblyName)
           .GetResult();

            //Assert
            Assert.True(result.IsSuccessful,"La capa de infraestructura de no deberia depender del proyecto api");

        }
        [Fact]
        public void TypesInApplicationServicesShouldEndingService()
        {
            //Act
            var result = Types.InCurrentDomain().That().ResideInNamespace("Application.Services")
                .Should().HaveNameEndingWith("Service").GetResult();

            //Assert
            Assert.True(result.IsSuccessful,"Todos los tipos en la carpeta Services de Application deben de terminar en Service");
        }

    }
}