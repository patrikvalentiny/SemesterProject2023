## Exam presentation

## [LoginComponent](frontend/src/app/pages/register-and-login/login-view/login-view.component.ts)
- A FormGroup is created with the name 'loginForm' and the fields 'username' and 'password' are added to it.
- Data validation is done via the Validators class.
- User login is prevented if the form is invalid by disabling the login button.

## [LoginService](frontend/src/app/services/account.service.ts)
- Using a service promotes modularity, maintainability, and reusability of code.
- Api calls are made to the backend with the HttpClient class.
- A successful login API call returns a token that is stored in the local storage.

## [AccountController](api/Controllers/AccountController.cs)
- The login method checks if the user exists in the database and if the password is correct.
  - ### [AccountService - Authenticate](service/Services/AccountService.cs)
    - The Authenticate method checks if the user exists in the database and retrieves the user's password hash.
    - [PasswordRepository](infrastructure/Repositories/PasswordRepository.cs)
      - The PasswordRepository class is used to retrieve the password hash from the database.
      - Parametrized queries are used to prevent SQL injection.
    - The password hash is compared with the hash of the password entered by the user.
    - [Argon2idHasher](service/Password/Argon2idPasswordHashAlgorithm.cs)
      - A new password hash is generated with the Argon2id algorithm.
      - The return is a boolean of comparing the two hashes.
    - If validation succeeds, a user object is returned.
- If the login is successful, a token is generated and returned to the frontend.
  - ### [JwtService - IssueToken](service/Services/JwtService.cs)
    - The IssueToken method creates a token with the user's id. 
- Exception handling is done with the try-catch block and the BadRequest method.

