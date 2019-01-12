using Xunit;

namespace Piovra.Tests {
    public class EventAggregatorTests {
        [Fact]
        public void Test() {            
            Account account = new Account { Amount = 1 };

            EventAggregator.GetEvent<UpdateAccountEvent>().Subscribe(UpdateAccount);
            EventAggregator.GetEvent<UpdateAccountEvent>().Publish(account);

            Assert.True(account.Amount == 2);

            EventAggregator.GetEvent<UpdateAccountEvent>().Publish(account);

            Assert.True(account.Amount == 3);

            EventAggregator.GetEvent<UpdateAccountEvent>().Unsubscribe(UpdateAccount);

            Assert.True(account.Amount == 3);
        }

        static void UpdateAccount(Account account) => account.Amount++;

        public class Account {
            public int Amount { get; set; }
        }

        public class UpdateAccountEvent : EventAggregator.Event<Account> {}
    }
}
