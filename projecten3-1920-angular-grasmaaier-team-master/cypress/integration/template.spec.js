/*
  Door ongekende redenen is cypress niet in staat om 
  samen te werken met de Authorization van de backend.
  Daarom werkt de test enkel wanneer in TemplateController
  de authorizatie annotatie in commentaar geplaatst wordt.
*/

describe("Template", function() {
  it("our app runs", function() {
    cy.visit("/");
  });
  it("we can login", function() {
    cy.get("[data-cy=username]")
      .type("Tycho.Altink@mail.be")
      .should("have.value", "Tycho.Altink@mail.be");
    cy.get("[data-cy=password]")
      .type("admin123")
      .should("have.value", "admin123");
    cy.get("[data-cy=login-button]").click();
    cy.url().should("eq", "http://localhost:4200/administration");
  });
  it("The template button works", function() {
    cy.get("[data-cy=template-button").click();
  });
  it("The template button navigates to the right page", function() {
    cy.url().should("eq", "http://localhost:4200/administration/template");
    cy.wait(5000);
  });
  it("Navigate to Tuesday", function() {
    cy.get(
      ':nth-child(1) > app-week > .pb-3 > .row > [ng-reflect-dag-nr="2"] > .card > .card-body'
    ).click();
  });
  it("Delete an activity", function() {
    cy.wait(5000);
    cy.get("#vmList > :nth-child(2) > div > .picto").click();
    cy.get(".btn-danger").click();
  });
  it("Add client and attendant", function() {
    cy.get("#vmList > .ic > div > .picto").click();
    cy.get(".card-body > :nth-child(2) > :nth-child(3) > .btn-primary").click();
    cy.get(
      ":nth-child(1) > div > .list-group > :nth-child(1) > .float-right > input"
    ).click();
    cy.get(
      ":nth-child(2) > div > .list-group > :nth-child(1) > .float-right > input"
    ).click();
    cy.get(
      ":nth-child(2) > div > .list-group > :nth-child(2) > .float-right > input"
    ).click();
    cy.get(".btn-primary").click();
    cy.get(".mt-3 > :nth-child(1) > ul > :nth-child(1)").should(
      "have.text",
      "Annet Eerkens"
    );
    cy.get(":nth-child(2) > ul > :nth-child(1)").should(
      "have.text",
      "Alida Van Daele"
    );
    cy.get(":nth-child(2) > ul > :nth-child(2)").should(
      "have.text",
      "Annet Van Nifterik"
    );
  });

  it('Change time of activity to morning', function(){
    cy.get('#nmList > .ic > div > .picto').click();
    cy.get(':nth-child(1) > app-hour-picker > .input-group > .form-control > .d-flex > .hour-picker-left').clear().type("00");
  })
});
