# Repozitorijum projektnog zadatka predmeta Veb programiranje u infrastrukturnim sistemima
---
Projekat predstavlja primer veb prodavnice, kao i potrebne funkcionalnosti za vođenje evidencije pomenute prodavnice.
Korišćene tehnologije:
 - Asp.Net Web Api 2
 - .NET 4.7.2
 - JavaScript
 - jQuery

Korišćeni alati:
 - Visual Studio 2019
 - Postman

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
     - [ ] Delete image when referencing object is deleted?
 - [X] Add data saving on application exit

 ---
 ## User Interface
 - [X] Create empty project with index.html
 - [X] Create folder structure (css, js, html)
 - [X] Add landing page 
     - [X] Signup / Login 
     - [X] Product details with product reviews 
     - [X] Product multi-part search
     - [ ] Product sorting
 - [X] Add user profile page
     - [X] Display profile info
     - [X] Edit profile
     - [X] Display favourite products for Buyer
     - [X] Display published products for Seller
        - [ ] Filter products
        - [ ] Sort products
     - [X] Display my orders
     - [X] Display my reviews
 - [X] Add admin dashboard
     - [X] Display profile info
     - [X] Display review awaiting approval
     - [X] Display all products
        - [ ] Filter products
        - [ ] Sort products
     - [X] Display all orders
        - [X] Allow order confirmation/cancelation
     - [X] Display all reviews
     - [X] Display all users
        - [X] Add new user
        - [X] Delete existing user
        - [X] Edit existing user
        - [ ] Search users
        - [ ] Sort users
 - [X] Add/Edit product page
     - [X] Product form
     - [X] Image upload
 - [X] Add/Edit user page
     - [X] User form
     - [ ] Separate edit form for Username and password
 - [X] Add/Edit review page
     - [X] Add review
     - [X] Image upload
     - [X] Edit review
 - [X] Site-wide input validation.
 - [X] Show popup for API related errors.
 
