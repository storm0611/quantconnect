/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using Moq;
using NUnit.Framework;
using QuantConnect.Brokerages;
using QuantConnect.Tests.Brokerages;
using System;
using System.Collections.Generic;
using QuantConnect.Data.Market;
using QuantConnect.Orders;
using QuantConnect.Orders.Fees;
using QuantConnect.Securities;

namespace QuantConnect.Tests.Common.Brokerages
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class DefaultBrokerageModelTests
    {
        private readonly DefaultBrokerageModel _defaultBrokerageModel = new DefaultBrokerageModel();

        [Test]
        public void CanSubmitOrder_WhenMarketOnOpenOrderForFutures()
        {
            var order = GetMarketOnOpenOrder();
            var future = TestsHelpers.GetSecurity(securityType: SecurityType.Future, symbol: Futures.Indices.SP500EMini, market: Market.CME);
            var futureOption = TestsHelpers.GetSecurity(securityType: SecurityType.FutureOption, symbol: Futures.Indices.SP500EMini, market: Market.CME);
            Assert.IsFalse(_defaultBrokerageModel.CanSubmitOrder(future, order, out _));
            Assert.IsFalse(_defaultBrokerageModel.CanSubmitOrder(futureOption, order, out _));
        }

        [TestCase(SecurityType.Base)]
        [TestCase(SecurityType.Equity)]
        [TestCase(SecurityType.Option)]
        [TestCase(SecurityType.Forex)]
        [TestCase(SecurityType.Cfd)]
        [TestCase(SecurityType.Crypto)]
        [TestCase(SecurityType.Index)]
        [TestCase(SecurityType.IndexOption)]
        public void CanSubmitOrder_WhenMarketOnOpenOrderForOtherSecurityTypes(SecurityType securityType)
        {
            var order = GetMarketOnOpenOrder();
            var security = TestsHelpers.GetSecurity(securityType: securityType, market: Market.USA);
            Assert.IsTrue(_defaultBrokerageModel.CanSubmitOrder(security, order, out _));
        }

        private static Order GetMarketOnOpenOrder()
        {
            var order = new Mock<Order>();
            order.Setup(o => o.Type).Returns(OrderType.MarketOnOpen);
            return order.Object;
        }
    }
}
