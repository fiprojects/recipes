using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Info;

namespace RecipesWeb.Controllers
{
    public class BaseController : Controller
    {
        private const string AlgorithmCookieName = "RecommendingAlgorithm";
        private const string DefaultAlgorithmIdentifier = "TfIdf";

        protected RecommendingAlgorithm Algorithm
        {
            get
            {
                Request.Cookies.TryGetValue(AlgorithmCookieName, out var identifier);
                return RecommendingAlgorithms.Get(identifier) ?? RecommendingAlgorithms.Get(DefaultAlgorithmIdentifier);
            }
        }

        protected void SetAlgorithm(string identifier)
        {
            if (RecommendingAlgorithms.Get(identifier) == null)
            {
                return;
            }

            Response.Cookies.Append(AlgorithmCookieName, identifier, new CookieOptions { Expires = DateTime.Now.AddDays(1) });
        }
    }
}