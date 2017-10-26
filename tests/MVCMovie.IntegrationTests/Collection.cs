using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MVCMovie.IntegrationTest
{
    [CollectionDefinition("SystemCollection")]
    public class Collection : ICollectionFixture<TestContext>
    {

    }
}
