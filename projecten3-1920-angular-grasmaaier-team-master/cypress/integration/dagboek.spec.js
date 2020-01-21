describe('Dagboek', function() {
    it('our app runs', function() {
        cy.visit('/');
      });
      it('we can login', function(){
          cy.get('[data-cy=username]').type('tycho.altink@mail.be').should('have.value', 'tycho.altink@mail.be');
          cy.get('[data-cy=password]').type('admin123').should('have.value', 'admin123');
          cy.get('[data-cy=login-button]').click();
          cy.url().should('eq', 'http://localhost:4200/administration');
      })
      it('The dagboek button works', function(){
        cy.get('[data-cy=dagboek-button').click();
      })
});