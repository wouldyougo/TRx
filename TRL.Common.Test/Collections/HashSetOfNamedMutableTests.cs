using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Collections;
using TRL.Common.Test.Models;

namespace TRL.Common.Test.Collections
{
    [TestClass]
    public class HashSetOfNamedMutableTests
    {
        [TestMethod]
        public void Collections_HashSetOfNamedMutable_add_one_model_instance_just_once_test()
        {
            HashSetOfNamedMutable<Model> hs = new HashSetOfNamedMutable<Model>();

            Assert.AreEqual(0, hs.Count);

            Model model = new Model("Jack", 35);

            hs.Add(model);

            Assert.AreEqual(1, hs.Count);

            hs.Add(model);

            Assert.AreEqual(1, hs.Count);
        }

        [TestMethod]
        public void Collections_HashSetOfNamedMutable_add_new_model_instance_with_update_method_test()
        {
            HashSetOfNamedMutable<Model> hs = new HashSetOfNamedMutable<Model>();

            Assert.AreEqual(0, hs.Count);

            hs.Update(new Model("Jack", 35));

            Assert.AreEqual(1, hs.Count);
        }

        [TestMethod]
        public void Collections_HashSetOfNamedMutable_update_existing_model_fields_test()
        {
            HashSetOfNamedMutable<Model> hs = new HashSetOfNamedMutable<Model>();

            Assert.AreEqual(0, hs.Count);

            Model model = new Model("Jack", 35);
            hs.Update(model);
            Assert.AreEqual(1, hs.Count);

            Model update = new Model("Jack", 88);

            hs.Update(update);
            Assert.AreEqual(1, hs.Count);
            Assert.AreEqual(88, model.Value);
        }

        [TestMethod]
        public void Collections_HashSetOfNamedMutable_add_updates_existing_model_fields_test()
        {
            HashSetOfNamedMutable<Model> hs = new HashSetOfNamedMutable<Model>();

            Assert.AreEqual(0, hs.Count);

            Model model = new Model("Jack", 35);
            hs.Add(model);
            Assert.AreEqual(1, hs.Count);

            Model update = new Model("Jack", 88);

            hs.Add(update);
            Assert.AreEqual(1, hs.Count);
            Assert.AreEqual(88, model.Value);
        }

    }
}
