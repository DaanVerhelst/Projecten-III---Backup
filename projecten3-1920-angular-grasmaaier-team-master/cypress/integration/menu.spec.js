describe('Menu', function() {
    it('our app runs', function() {
        cy.visit('/');
      });

      it('we can login', function(){
          cy.get('[data-cy=username]').type('tycho.altink@mail.be').should('have.value', 'tycho.altink@mail.be');
          cy.get('[data-cy=password]').type('admin123').should('have.value', 'admin123');
          cy.get('[data-cy=login-button]').click();
          cy.url().should('eq', 'http://localhost:4200/administration');
      })

      it('The menu button works', function(){
        cy.get('[data-cy=menu-button').click();
      })

      it('Klik op menu Maandag', function(){
        cy.wait(3000);
        cy.get('.card').contains('Maandag').click();
      })

      it('Verander menu Maandag', function(){
        cy.wait(3000);
        cy.get('[data-cy=Soep-textfield]').clear().type('Andere Soep').should('have.value', 'Andere Soep');
        cy.get('[data-cy=Groente-textfield]').clear().type('Andere Groentjes').should('have.value', 'Andere Groentjes');
        cy.get('[data-cy=Vlees-textfield]').clear().type('Andere Vleesekens').should('have.value', 'Andere Vleesekens');
        cy.get('[data-cy=Supplement-textfield]').clear().type('Andere Supplementen').should('have.value', 'Andere Supplementen');        
      })

      it('Klik knop opslaan Maandag', function(){
        cy.get('[data-cy=Knop-opslaan]').click();
      })

      it('Klik op menu Dinsdag', function(){
        cy.wait(3000);
        cy.get('.card').contains('Dinsdag').click();
      })

      it('Verander menu Dinsdag', function(){
        cy.wait(3000);
        cy.get('[data-cy=Soep-textfield]').clear().type('Andere Soep').should('have.value', 'Andere Soep');
        cy.get('[data-cy=Groente-textfield]').clear().type('Andere Groentjes').should('have.value', 'Andere Groentjes');
        cy.get('[data-cy=Vlees-textfield]').clear().type('Andere Vleesekens').should('have.value', 'Andere Vleesekens');
        cy.get('[data-cy=Supplement-textfield]').clear().type('Andere Supplementen').should('have.value', 'Andere Supplementen');        
      })

      it('Klik knop opslaan Dinsdag', function(){
        cy.get('[data-cy=Knop-opslaan]').click();
      })

      it('Controlleer inhoud van weekmenu', function(){
        cy.get(':nth-child(1) > .card-body > .card-text').should('contain.text', 'Soep: Andere Soep');
        cy.get(':nth-child(1) > .card-body > .card-text').should('contain.text', 'Groente: Andere Groentjes');
        cy.get(':nth-child(1) > .card-body > .card-text').should('contain.text', 'Vlees: Andere Vleesekens');
        cy.get(':nth-child(1) > .card-body > .card-text').should('contain.text', 'Supplement: Andere Supplementen');
      })
});