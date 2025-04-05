
# Solterra - Activities API Documentation

---

## Hobby API

`/api/Hobby/`

```
GET     /api/Hobby/List             - Get all hobbies  
GET     /api/Hobby/Find/{id}        - Get a hobby by ID  
POST    /api/Hobby/Add              - Add a hobby  
PUT     /api/Hobby/Update/{id}      - Update a hobby  
DELETE  /api/Hobby/Delete/{id}      - Delete a hobby  
```

---

## Experience API

`/api/Experience/`

```
GET     /api/Experience/List             - Get all experiences  
GET     /api/Experience/Find/{id}        - Get an experience by ID  
POST    /api/Experience/Add              - Add an experience  
PUT     /api/Experience/Update/{id}      - Update an experience  
DELETE  /api/Experience/Delete/{id}      - Delete an experience  
```

---

## Mood API

`/api/Mood/`

```
GET     /api/Mood/List             - Get all moods  
GET     /api/Mood/Find/{id}        - Get a mood by ID  
POST    /api/Mood/Add              - Add a mood  
PUT     /api/Mood/Update/{id}      - Update a mood  
DELETE  /api/Mood/Delete/{id}      - Delete a mood  
```

---

## ExperienceMood API (Bridge)

`/api/ExperienceMood/`

```
GET     /api/ExperienceMood/List                     - Get all experience-mood associations  
GET     /api/ExperienceMood/Find/{id}                - Get an experience-mood association by ID  
POST    /api/ExperienceMood/Add                      - Add an experience-mood association  
POST    /api/ExperienceMood/Update/{id}              - Update an experience-mood association  
DELETE  /api/ExperienceMood/Delete/{id}              - Delete an experience-mood association  
GET     /api/ExperienceMood/ListForExperience/{id}   - List moods for a specific experience  
```

---

## UserActivity API (Bridge)

`/api/UserActivity/`

```
GET     /api/UserActivity/List             - Get all user activity records  
GET     /api/UserActivity/Find/{id}        - Get a user activity record by ID  
POST    /api/UserActivity/Add              - Create a new user activity  
POST    /api/UserActivity/Update/{id}      - Update an existing user activity  
DELETE  /api/UserActivity/Delete/{id}      - Delete a user activity  
GET     /api/UserActivity/ListForUser/{id} - Get all user activity records for a specific user  
POST    /api/UserActivity/LinkUser         - Create a user activity association  
POST    /api/UserActivity/UnlinkUser/{id}  - Delete a user activity association  
```

---

## Inventories

`/api/InventoriesAPI/`

```
GET     /api/InventoriesAPI                                  - List all inventories  
GET     /api/InventoriesAPI/UserInventory/{userId}           - Get a specific user's inventory  
POST    /api/InventoriesAPI/AddToInventory/{userId}/{quantity}/{itemId} - Add item to inventory  
POST    /api/InventoriesAPI/EditInventory/{id}/{userId}/{itemId}/{quantity} - Edit inventory entry  
DELETE  /api/InventoriesAPI/{id}                             - Delete an inventory entry  
```

---

## Items

`/api/ItemsAPI/`

```
GET     /api/ItemsAPI                                      - List all items (no types)  
GET     /api/ItemsAPI/WithTypes                            - List all items with types  
GET     /api/ItemsAPI/ItemsAPI/GetItemsForType             - Get items by type  
GET     /api/ItemsAPI/{id}                                 - Get item by ID  
DELETE  /api/ItemsAPI/{id}                                 - Delete item by ID  
POST    /api/ItemsAPI/ItemsAPI/LinkItemToType/{itemId}/{typeId}     - Link item to type  
POST    /api/ItemsAPI/ItemsAPI/UnlinkItemToType/{itemId}/{typeId}   - Unlink item from type  
POST    /api/ItemsAPI/ItemsAPI/CreateItem/{name}/{description}/{value} - Create new item  
POST    /api/ItemsAPI/ItemsAPI/EditItem                    - Edit existing item  
```

---

## ItemTypes

`/api/ItemTypesAPI/`

```
GET     /api/ItemTypesAPI                                      - List all item types  
GET     /api/ItemTypesAPI/{id}                                 - Get item type by ID  
GET     /api/ItemTypesAPI/ItemTypesAPI/GetTypesForItem         - Get types linked to an item  
POST    /api/ItemTypesAPI/ItemTypesAPI/CreateItemType/{type}   - Create new item type  
DELETE  /api/ItemTypesAPI/ItemTypesAPI/DeleteItemType/{id}     - Delete item type by ID  
```

---

## Users

`/api/UsersAPI/`

```
PUT     /api/UsersAPI/updateInventorySpace/{userId}/{spaceRemoved}  - Update inventory space  
GET     /api/UsersAPI                                               - List all users  
GET     /api/UsersAPI/{id}                                          - Get user by ID  
DELETE  /api/UsersAPI/{id}                                          - Delete user by ID  
POST    /api/UsersAPI/PostUser/{username}/{password}               - Create new user  
POST    /api/UsersAPI/EditUser/{id}/{username}/{password}/{solshards} - Edit user  
```

---

## Pets

`/api/PetApi/`

```
GET     /api/PetApi/List           - List all pets  
POST    /api/PetApi/CreatePetAdmin - Create pet as admin  
POST    /api/PetApi/CreatePetUser  - Create pet as user  
POST    /api/PetApi/UpdatePetAdmin - Update pet as admin  
DELETE  /api/PetApi/DeletePet      - Delete a pet  
```

---

## Species

`/api/SpeciesApi/`

```
GET     /api/SpeciesApi/List        - List all species  
POST    /api/SpeciesApi/CreateSpecies   - Create new species  
POST    /api/SpeciesApi/UpdateSpecies   - Update existing species  
DELETE  /api/SpeciesApi/DeleteSpecies   - Delete existing species  
```
