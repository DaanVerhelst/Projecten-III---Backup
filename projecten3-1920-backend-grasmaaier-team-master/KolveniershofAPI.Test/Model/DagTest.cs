using KolveniershofAPI.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KolveniershofAPI.Test.Model
{
    public class DagTest
    {

        #region pasDuurAtelierDagAan_Testen
        [Fact]
        public void pasDuurAtelierDagAan()
        {
            Dag dag = new Dag();
            Atelier at = new Atelier();
            Assert.Throws<ArgumentException>(() => dag.pasDuurAtelierDagAan(at, null));
        }
        #endregion

        #region zetPersoonOpAfwezig_Testen
        [Fact]
        public void zetPersoonOpAfwezig_Null()
        {
            Dag dag = new Dag();
            Assert.Throws<ArgumentException>(() => dag.zetPersoonOpAfwezig(null));
        }
        [Fact]
        public void zetPersoonOpAfwezig_Juist()
        {
            Dag dag = new Dag();
            dag.zetPersoonOpAfwezig(new Client());
            Assert.Equal(1, dag.Dag_Personen.Count);
        }
        #endregion

        #region voegCommentaarToe_Testen
        [Fact]
        public void voegCommentaarToe_Null()
        {
            Dag dag = new Dag();
            Client c = new Client();
            Assert.Throws<ArgumentException>(() => dag.voegCommentaarToe(null, c));
        }
        [Fact]
        public void voegCommentaarToe_Leeg()
        {
            Dag dag = new Dag();
            Client c = new Client();
            Assert.Throws<ArgumentException>(() => dag.voegCommentaarToe("", c));
        }
        [Fact]
        public void voegCommentaarToe_PersoonNull()
        {
            Dag dag = new Dag();
            Client c = null;
            Assert.Throws<ArgumentException>(() => dag.voegCommentaarToe("test", c));
        }
        [Fact]
        public void voegCommentaarToe_Juist()
        {
            Dag dag = new Dag();
            Client c = new Client();
            dag.voegCommentaarToe("test", c);
            Assert.Equal(1, dag.Dag_Personen.Count);
        }
        #endregion

        #region verwijderAtelierUitDag_Testen
        [Fact]
        public void verwijderAtelierUitDag_onbestaandAtelier()
        {
            Dag dag = new Dag();
            Atelier at = new Atelier();
            Assert.Throws<ArgumentException>(() => dag.verwijderAtelierUitDag(at));
        }
        #endregion

        #region verwijderBegeleiderUitDag_Testen
        [Fact]
        public void verwijderBegeleiderUitDag_onbestaandeBegeleider()
        {
            Dag dag = new Dag();
            Atelier at = new Atelier();
            Begeleider b = new Begeleider();
            Assert.Throws<ArgumentException>(() => dag.verwijderBegeleiderUitActiviteit(at, b));
        }
        #endregion
    }
}
