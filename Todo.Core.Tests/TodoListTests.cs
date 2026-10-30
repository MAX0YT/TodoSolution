using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }
        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }
        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }
        [Fact]
        public void SaveLoad_PreservesItems()
        {
            var path = Path.GetTempFileName();
            try
            {
                var list = new TodoList();
                var a = list.Add("Buy milk");
                var b = list.Add("Read book");
                b.MarkDone();

                list.Save(path);
                var loaded = TodoList.Load(path);

                Assert.Equal(2, loaded.Count);
                Assert.Equal(a.Id, loaded.Items[0].Id);
                Assert.Equal("Buy milk", loaded.Items[0].Title);
                Assert.True(loaded.Items[1].IsDone);
            }
            finally { File.Delete(path); }
        }

        [Fact]
        public void Load_MissingFile_Throws()
        {
            Assert.Throws<FileNotFoundException>(() => TodoList.Load("no_such_file.json"));
        }
    }
}
