describe('Atelier Crud', function() {
    it('our app runs', function() {
        cy.visit('/');
      });
      it('we can login', function(){
          cy.get('[data-cy=username]').type('tycho.altink@mail.be').should('have.value', 'tycho.altink@mail.be');
          cy.get('[data-cy=password]').type('admin123').should('have.value', 'admin123');
          cy.get('[data-cy=login-button]').click();
          cy.url().should('eq', 'http://localhost:4200/administration');
      })
      it('The atelier button works', function(){
        cy.get('[data-cy=ateliers-button').click();
      })
      it('The atelier button navigates to the right page', function(){
        cy.url().should('eq', 'http://localhost:4200/administration/ateliers');
      })

      //Kan niet getest worden wegens nood aan afbeelding uploaden

      /*it('Atelier toevoegen', function(){
        cy.get('#nameField').type('Koffie zetten voor begeleiding');
        cy.get('.row > .btn').click();
      })*/

      it('Atelier aanpassen', function(){
        cy.wait(3000);
        cy.get(':nth-child(1) > div > .picto').click();
        cy.get('#nameField').clear().type('Ander Atelier');
        cy.get('.row > .btn-primary').click();
      })

      it('Atelier verwijderen', function(){
        cy.get('.btn-danger').click();
      })
});