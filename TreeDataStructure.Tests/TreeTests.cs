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
        [InlineData("South America")]
        [InlineData("America")]
        [InlineData("World")]
        public void SearchInDescendants_ReturnsNodeWithData(string data)
        {
            var node = CountryTree.SearchInDescendants(data);

            Assert.Equal(node.Data, data);
        }


        [Theory]
        [InlineData("World", 0)]
        [InlineData("Europe", 1)]
        [InlineData("South America", 2)]
        [InlineData("Paris", 3)]
        public void GetAscendants_ReturnsCorrectCount(string data, int numberOfAncestors)
        {
            var node = CountryTree.SearchInDescendants(data);

            Assert.Equal(node.GetAncestors().Count(), numberOfAncestors);
        }

        [Theory]
        [InlineData("World", 0)]
        [InlineData("Europe", 1)]
        [InlineData("South America", 2)]
        [InlineData("Paris", 3)]
        public void Level_GivenDifferentNodes_ReturnsCorrectLevel(string data, int level)
        {
            var node = CountryTree.SearchInDescendants(data);

            Assert.Equal(node.Level, level);
        }


        [Fact]
        public void GetDescendants_ReturnsCorrectCount()
        {
            var descendants = CountryTree.GetDescendants(CountryTree);
            var count = 0;

            foreach (var descendant in descendants)
            {
                count++;
            }

            Assert.Equal(count, 13);
        }


        [Theory]
        [InlineData("World", false)]
        [InlineData("Europe", false)]
        [InlineData("South America", true)]
        [InlineData("Paris", true)]
        public void IsLeaf_GivenLeafNodes_ReturnsCorrectValue(string data, bool isLeaf)
        {
            Tree<string> node = CountryTree.SearchInDescendants(data);
            if (data == "World")
                node = CountryTree;
            
            Assert.Equal(node.IsLeaf, isLeaf);
        }


        [Fact]
        public void RemoveChild_GivenNode_RemovesNodeFromChildrenCollection()
        {
            var animals = new Tree<string>("Animals");
            var lion = animals.AddChild("lion");
            var ape = animals.AddChild("ape");

            animals.RemoveChild(ape);

            Assert.False(animals.Children.Contains(ape));
        }


        [Fact]
        public void RemoveChild_GivenData_RemovesFirstNodeWithAccordingDataFromChildrenCollection()
        {
            var animals = new Tree<string>("Animals");
            var lion = animals.AddChild("lion");
            var ape = animals.AddChild("ape");

            animals.RemoveChild(ape);

            Assert.False(animals.Children.Contains(ape));
        }

    }
}
