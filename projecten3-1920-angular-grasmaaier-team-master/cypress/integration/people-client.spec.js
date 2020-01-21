describe('Client Crud', function() {
  it('our app runs', function() {
    cy.visit('/');
  });
  it('we can login', function(){
      cy.get('[data-cy=username]').type('tycho.altink@mail.be').should('have.value', 'tycho.altink@mail.be');
      cy.get('[data-cy=password]').type('admin123').should('have.value', 'admin123');
      cy.get('[data-cy=login-button]').click();
      cy.url().should('eq', 'http://localhost:4200/administration');
  })
  it('mock clients', function(){
    cy.server();
    cy.route({
      method:'GET',
      url: '/api/Persoon/clienten',
      status: 200,
      response: 'fixture:clients.json'
    })
  })
  it('The client button works', function(){
    cy.get('[data-cy=clients-button').click();
  })
  it('The client button navigates to the right page', function(){
    cy.url().should('eq', 'http://localhost:4200/administration/clients');
  })
/*   it('The backend returns 20 clients', function(){
    cy.get('[data-cy=client-card]').should('have.length', 20);
  }) */
  it('The client toevoegen button works', function(){
    cy.get('[data-cy=add-client]').click();
  })
  it('You can add a client', function() {
    cy.get('[data-cy=client-voornaam-add]').type('Wannes').should('have.value', 'Wannes');
    cy.get('[data-cy=client-familienaam-add]').type('De craene').should('have.value', 'De craene');
    cy.get('[data-cy=submit-client-add').click();
    cy.get('#exampleModal > .modal-dialog > .modal-content > .modal-footer > .btn').click();
  })
/*   it('There should now be 21 clients', function(){
    cy.get('[data-cy=client-card]').should('have.length', 21);
  }) */
  it('You click the edit a client button'), function(){
    cy.get('[data-cy=edit-client]').last().click();
  }
  it('you can edit the client', function(){
    cy.get('[data-cy=client-voornaam-edit]').type('2').should('have.value', 'Wannes2');
    cy.get('[data-cy=client-familienaam-edit]').type('De craene2').should('have.value', 'De craene2');
    cy.get('[data-cy=submit-client-edit').click();
  })
  it('you can delete a client', function(){
    cy.get('[data-cy=delete-client').click();
  })
  it('there are now 20 clients', function(){
    cy.get('[data-cy=client-card]').should('have.length', 20);
  })
  });