using Piovra.Ds;
using Xunit;

namespace Piovra.Tests {
    public class TrieTests {
        [Fact]
        public void Test() {
            var trie = new Trie();

            trie.AddWord("Canal");
            trie.AddWord("Candy");
            trie.AddWord("The");
            trie.AddWord("There");
            
            Assert.Equal(2, trie.GetCountPrefix("Can"));

            Assert.Equal(2, trie.GetCountPrefix("The"));
            Assert.Equal(2, trie.GetCountPrefix("Th"));
            Assert.Equal(2, trie.GetCountPrefix("T"));

            Assert.Equal(1, trie.GetCountPrefix("Cana"));

            Assert.True(trie.ContainsWord("Candy"));
            Assert.True(trie.ContainsWord("The"));
            Assert.True(trie.ContainsWord("There"));
            Assert.True(trie.ContainsWord("Canal"));

            Assert.False(trie.ContainsWord("Cana"));
            Assert.False(trie.ContainsWord("Ther"));
        }
    }
}
