using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TreeDataStructure.Tests
{
    public class TreeTests
    {
        public Tree<string> CountryTree { get; private set; }

        public TreeTests()
        {
            CountryTree = CreateCountryTree();
        }

        public Tree<string> CreateCountryTree()
        {
            var world = new Tree<string>("World");

            var europe = world.AddChild("Europe");
            europe.AddChild("Germany").AddChild("Berlin");
            europe.AddChild("France").AddChild("Paris");
            europe.AddChild("Poland");
            europe.AddChild("Austria");

            var america = world.AddChild("America");

            var northAmerica = america.AddChild("North America");
            northAmerica.AddChild("United States").AddChild("Washington DC");
            northAmerica.AddChild("Canada");

            var southAmerica = america.AddChild("South America");

            return world;
        }


        [Fact]
        public void Constructor_WithoutParent_ReturnsRootNode()
        {
            var node = new Tree<string>("God");

            Assert.True(node.IsRoot);
        }



        [Theory]
        [InlineData("Europe")]
        [InlineData("South America")] // leaf
        [InlineData("America")]
        [InlineData("World")] // root
        public void SearchDescendants_CountryTree(string data)
        {
            var node = CountryTree.SearchDescendants(data);

            Assert.Equal(node.Data, data);
        }

        [Fact]
        public void GetDescendants_CountryTree()
        {
            var descendants = CountryTree.GetDescendants(CountryTree);
            var count = 0;

            foreach (var descendant in descendants)
            {
                count++;
            }

            Assert.Equal(count, 13);
        }

    }
}
