﻿global using Basket.API.Model;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
global using Microsoft.eShopOnContainers.Services.Basket.API.Controllers;
global using Microsoft.Extensions.Logging;
global using Moq;
global using System;
global using System.Collections.Generic;
global using System.Security.Claims;
global using System.Threading.Tasks;
global using Xunit;
global using IBasketIdentityService = Microsoft.eShopOnContainers.Services.Basket.API.Services.IIdentityService;
global using Microsoft.eShopOnContainers.WebMVC.Controllers;
global using Microsoft.eShopOnContainers.WebMVC.Services;
global using Microsoft.eShopOnContainers.WebMVC.ViewModels;
global using BasketModel = Microsoft.eShopOnContainers.WebMVC.ViewModels.Basket;
