describe('General tests', function() {
    it('our app runs', function() {
      cy.visit('/');
    });
    it('we can enter login values correctly', function(){
        cy.get('[data-cy=username]').type('tycho.altink@mail.be').should('have.value', 'tycho.altink@mail.be');
        cy.get('[data-cy=password]').type('admin123').should('have.value', 'admin123');
    })
    it('we can edit values correctly', function(){
        cy.get('[data-cy=username]')
        .type('{selectall}{backspace}')
        .type('tycho.altinc')
        .type('{backspace}')
        .type('k@mail.be')
        .should('have.value', 'tycho.altink@mail.be')
    })
    it('The login button works', function(){
      cy.get('[data-cy=login-button]').click()
      
    })
    it('loging in sends you to the right page', function() {
      cy.url().should('eq', 'http://localhost:4200/administration')
    })
    it('The logout button works', function(){
      cy.get('[data-cy=logout-button]').click()
    })
    it('loging out sends you to the right page', function() {
      cy.url().should('eq', 'http://localhost:4200/login')
    })
    it('Entering the wrong values does not allow a login', function(){
      cy.get('[data-cy=username]').type('foutemail@mail.be').should('have.value', 'foutemail@mail.be');
      cy.get('[data-cy=password]').type('fout').should('have.value', 'fout');
      cy.get('[data-cy=login-button]').click()
      cy.url().should('eq', 'http://localhost:4200/login')
    })

  });