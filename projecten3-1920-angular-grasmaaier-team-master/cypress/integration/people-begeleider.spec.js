describe('Begeleider Crud', function() {
    it('our app runs', function() {
        cy.visit('/');
      });
    it('we can login', function(){
        cy.get('[data-cy=username]').type('tycho.altink@mail.be').should('have.value', 'tycho.altink@mail.be');
        cy.get('[data-cy=password]').type('admin123').should('have.value', 'admin123');
        cy.get('[data-cy=login-button]').click();
        cy.url().should('eq', 'http://localhost:4200/administration');
    })
    it('The begeleider button works', function(){
        cy.get('[data-cy=begeleiders-button]').click();
    })
    it('The begeleider button navigates to the right page', function(){
        cy.url().should('eq', 'http://localhost:4200/administration/begeleiders');
    })
    it('Begeleider verwijderen', function(){
        cy.wait(3000);
        cy.get(':nth-child(2) > [data-cy=begeleider-card] > .d-flex > .btn-group > .btn-outline-danger').click();
    })
    it('Begeleider toevoegen', function(){
        cy.get('.btn-primary').click();
        cy.get('[data-cy=begeleider-voornaam]').type('Marco');
        cy.get('[data-cy=begeleider-familienaam]').type('Polo');
        cy.get('[data-cy=begeleider-email]').type('marco.polo@polomail.marco');
        cy.get('[data-cy=begeleider-password]').type('passwooooord');
        cy.get('[data-cy=begeleider-confirm-password]').type('passwooooord');
        cy.get('[data-cy=begeleider-isAdmin]').click();
        cy.get('[data-cy=begeleider-submit]').click();
        cy.get('#exampleModal > .modal-dialog > .modal-content > .modal-footer > .btn').click();
    })
    it('Begeleider aanpassen', function(){
        cy.wait(3000);
        cy.get(':nth-child(6) > [data-cy=begeleider-card] > .d-flex > .btn-group > .btn-outline-warning').click();
        cy.get('app-edit-begeleider > :nth-child(1) > form.ng-untouched > :nth-child(1) > [data-cy=begeleider-voornaam]').clear().type('Polo');
        cy.get('.ng-invalid.ng-dirty > :nth-child(1) > [data-cy=begeleider-familienaam]').clear().type('Marco');
        cy.get('form.ng-dirty > .text-center > [data-cy=begeleider-submit]').click();
        cy.get('#editModal > .modal-dialog > .modal-content > .modal-footer > .btn').click();
    })
    
})