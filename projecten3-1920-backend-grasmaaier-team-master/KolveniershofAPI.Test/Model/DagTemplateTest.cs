using KolveniershofAPI.Model;
using KolveniershofAPI.Model.Model_EF;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KolveniershofAPI.Test.Model
{
    public class DagTemplateTest
    {

        #region AtelierToevoegenOpTijdstip_Testen
        [Fact]
        public void AtelierToevoegenOpTijdstip_AtelierIsNull()
        {
            DagTemplate dt = new DagTemplate();
            Assert.Throws<ArgumentException>(() => dt.AtelierToevoegenOpTijdstip(null, new TimeSpan(), new TimeSpan(5)));
        }

        [Fact]
        public void AtelierToevoegenOpTijdstip_StartIsNull()
        {
            DagTemplate dt = new DagTemplate();
            Atelier at = new Atelier("Marco");

            Assert.Throws<ArgumentException>(() => dt.AtelierToevoegenOpTijdstip(at, null, new TimeSpan()));
        }

        [Fact]
        public void AtelierToevoegenOpTijdstip_EndIsNull()
        {
            DagTemplate dt = new DagTemplate();
            Atelier at = new Atelier("Polo");

            Assert.Throws<ArgumentException>(() => dt.AtelierToevoegenOpTijdstip(at, new TimeSpan(), null));
        }

        [Fact]
        public void AtelierToevoegenOpTijdstip_AllesIngevuld()
        {
            //Arrange
            DagTemplate dt = new DagTemplate();
            Atelier at = new Atelier("Polo");
            TimeSpan ts1 = new TimeSpan();
            TimeSpan ts2 = new TimeSpan();
            //Act
            dt.AtelierToevoegenOpTijdstip(at, ts1, ts2);
            Atelier_Dag ad = dt.GetAtelierDagVanAtelier(at);
            //Assert
            Assert.Equal(at, ad.Atelier);
            Assert.Equal(ts1, ad.Start);
            Assert.Equal(ts2, ad.End);
            Assert.Equal("Polo", ad.Atelier.Naam);
        }
        #endregion

        #region GeefClientenVoorAtelier_Testen
        [Fact]
        public void GeefClientenVoorAtelier_AtelierIsNull()
        {
            DagTemplate dt = new DagTemplate();
            Assert.Throws<ArgumentException>(() => dt.GeefClientenVoorAtelier(null));
        }
        #endregion

        #region GeefBegeleidersVoorAtelier_Testen
        [Fact]
        public void GeefBegeleidersVoorAtelier_AtelierIsNull()
        {
            DagTemplate dt = new DagTemplate();
            Assert.Throws<ArgumentException>(() => dt.GeefBegeleidersVoorAtelier(null));
        }
        #endregion

        #region VoegClientenToeAanAtelier_Testen
        [Fact]
        public void VoegClientenToeAanAtelier_AtelierIsNull()
        {
            DagTemplate dt = new DagTemplate();
            Assert.Throws<ArgumentException>(() => dt.VoegClientenToeAanAtelier(null, new List<Client>()));
        }
        #endregion

        #region VoegBegeleidersToeAanAtelier_Testen
        [Fact]
        public void VoegBegeleidersToeAanAtelier_AtelierIsNull()
        {
            DagTemplate dt = new DagTemplate();
            Assert.Throws<ArgumentException>(() => dt.VoegBegeleidersToeAanAtelier(null, new List<Begeleider>()));
        }
        #endregion

    }
}
