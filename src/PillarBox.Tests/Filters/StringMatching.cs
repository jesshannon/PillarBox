using PillarBox.Business.Services.Filters;
using PillarBox.Data.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PillarBox.Tests.Filters
{
    public class StringMatching
    {
        [Fact]
        public void RegExMatch()
        {

            MessageFiltering filtering = new MessageFiltering(null, null);

            MessageFilter filter = new MessageFilter()
            {
                FieldName = "myField",
                IsRegularExpression = true,
                Pattern = "^[0-9]+$"
            };

            Dictionary<string, string> variables = new Dictionary<string, string>()
            {
                { "myField", "123"},
                { "otherField", "ABC"}
            };

            Assert.True(filtering.FilterApplies(filter, variables));
        }

        [Fact]
        public void RegExFail()
        {

            MessageFiltering filtering = new MessageFiltering(null, null);

            MessageFilter filter = new MessageFilter()
            {
                FieldName = "myField",
                IsRegularExpression = true,
                Pattern = "^[0-9]+$"
            };

            Dictionary<string, string> variables = new Dictionary<string, string>()
            {
                { "myField", "A123"},
                { "otherField", "ABC"}
            };

            Assert.False(filtering.FilterApplies(filter, variables));
        }

        [Fact]
        public void WildcardMatch()
        {

            MessageFiltering filtering = new MessageFiltering(null, null);

            MessageFilter filter = new MessageFilter()
            {
                FieldName = "myField",
                IsRegularExpression = false,
                Pattern = "*fox jumps*"
            };

            Dictionary<string, string> variables = new Dictionary<string, string>()
            {
                { "myField", "The quick brown fox jumps over the lazy dog."}
            };

            Assert.True(filtering.FilterApplies(filter, variables));
        }

        [Fact]
        public void WildcardFail()
        {

            MessageFiltering filtering = new MessageFiltering(null, null);

            MessageFilter filter = new MessageFilter()
            {
                FieldName = "myField",
                IsRegularExpression = false,
                Pattern = "*fox jumped*"
            };

            Dictionary<string, string> variables = new Dictionary<string, string>()
            {
                { "myField", "The quick brown fox jumps over the lazy dog."}
            };

            Assert.False(filtering.FilterApplies(filter, variables));
        }

        [Fact]
        public void WildcardComplexMatch()
        {

            MessageFiltering filtering = new MessageFiltering(null, null);

            MessageFilter filter = new MessageFilter()
            {
                FieldName = "myField",
                IsRegularExpression = false,
                Pattern = "*{}:<£R$L>A*"
            };

            Dictionary<string, string> variables = new Dictionary<string, string>()
            {
                { "myField", @"$%^IEGRSI(££%Y""NS(HG{]';lp)@:F{}:<£R$L>AW£{RQ!${L%EYLFCG{TR:{4~##"}
            };

            Assert.True(filtering.FilterApplies(filter, variables));
        }


    }
}
