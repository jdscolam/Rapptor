using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Api.ApiCaller.RestSharp;

namespace Rapptor.Tests.Integration
{
    [TestFixture]
    public class RestSharpPostsServiceIntegrationSpec
    {
        private const string ACCESS_TOKEN = "[INSERT ACCESS TOKEN HERE!]";

        [Test]
        public void PostsServiceCanListPostsStarredByAUser()
        {
            //Setup
            const string userId = "me";
            var restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);
            var postsService = new PostsService(restSharpApiCaller);

            //Execute
            var starredPosts = postsService.RetrievePostsStarredByUser(userId).ToList();

            //Verify
            starredPosts.ShouldNotBeNull();
            starredPosts.Count.ShouldBeGreaterThan(0);
            foreach (var post in starredPosts)
            {
                post.YouStarred.HasValue.ShouldBeTrue();
                // ReSharper disable PossibleInvalidOperationException
                post.YouStarred.Value.ShouldBeTrue();
                // ReSharper restore PossibleInvalidOperationException
            }

            //Teardown
        }
    }
}
