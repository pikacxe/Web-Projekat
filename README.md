# Repozitorijum projektnog zadatka predmeta Veb programiranje u infrastrukturnim sistemima
---
Projekat predstavlja primer veb prodavnice, kao i potrebne funkcionalnosti za vođenje evidencije pomenute prodavnice.
Korišćene tehnologije:
 - Asp.Net Web Api 2
 - JavaScript
 - jQuery


 # Planirani "Road Map" - (podložno izmenama)
 
 ## Web API

 ---

 - [X] Create data models
     - [ ] Add predefined JSON data for testing
         - [X] Admin data
         - [X] User data
         - [ ] Product data
         - [ ] Order data
         - [ ] Review data
 - [X] Create user controller with CRUD routes
     - [X] Add password hashing (Possible migration to client side)
     - [ ] Implement session control
         - [ ] Add token auth (custom token / jwt)
     - [ ] Implement RBAC for Buyer/Seller/Admin
     - [ ] Add auth to admin routes
 - [X] Create review controller with CRUD routes
 - [X] Create product controller with CRUD routes
     - [ ] Implement faourites per user
 - [X] Create order controller with CRUD routes
 - [ ] Create image upload controller
     - [ ] On upload should return image path

 - [ ] Add data saving route?

 ---
 ## User Interface
 - [X] Create empty project with index.html
 - [ ] Create folder structure (css, js, html)
 - [ ] Add landing page 
     - [ ] Signup / Login 
     - [ ] Customize page per user base
 
