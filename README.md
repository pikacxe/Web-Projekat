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
     - [X] Add predefined JSON data for testing
         - [X] Admin data
         - [X] User data
         - [X] Product data
         - [X] Order data
         - [X] Review data
 - [X] Add password hashing
 - [X] Implement session control
     - [X] Implement jwt token authentification
     - [X] Implement authorization with RBAC for Buyer/Seller/Admin
 - [X] Create user controller with CRUD routes
 - [X] Create review controller with CRUD routes
 - [X] Create product controller with CRUD routes
     - [X] Implement favourites per Buyer
     - [X] Implement published products per Seller
 - [X] Create order controller with CRUD routes
 - [X] Create image upload controller
     - [X] On upload should return image path
 - [X] Add data saving on application exit

 ---
 ## User Interface
 - [X] Create empty project with index.html
 - [X] Create folder structure (css, js, html)
 - [X] Add landing page 
     - [X] Signup / Login 
     - [X] Product details with product reviews 
 - [ ] Add user profile page
     - [X] Display profile info
     - [ ] Edit profile
     - [X] Display favourite products
     - [X] Display my orders
     - [X] Display my reviews
 - [ ] Add admin dashboard
     - [ ] Display profile info
     - [ ] Display review awaiting approval
     - [ ] Display all products
     - [ ] Display all orders
     - [ ] Display all reviews
 - [ ] Add product page
     - [ ] Product form
     - [ ] Image upload
 - [ ] Add user page
     - [ ] User form
 - [X] Add review page
     - [X] Review form
     - [X] Image upload

 
